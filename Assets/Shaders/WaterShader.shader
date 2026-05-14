[vertex]
#version 460 core
layout (location = 0) in vec3 a_Position;
layout (location = 1) in vec2 a_TexCoord;
layout (location = 2) in vec3 a_Normal;
layout (location = 3) in vec3 a_Tangent;
layout (location = 4) in vec4 a_Color;

struct Light
{
    vec4 l_ColorIntensity;          // Color + Intensity
    vec4 l_PositionType;            // Position + Type
    vec4 l_DirectionInnerCone;      // Direction + Inner Cone Angle
    vec4 l_AttenuationOuterCone;    // Attenuation + Outer Cone Angle
    vec4 l_SMapAndSColor;           // ShadowMap number + Shadow Color
};

layout (std140, binding = 0) uniform Matrices
{
    mat4 lp_Projection;
    mat4 lp_View;
};

layout (std140, binding = 1) uniform Lighting
{
    vec4 lp_CameraWorldPosLightCount;
    Light lp_lights[16];
};

uniform mat4 lp_Transform;
uniform vec2 u_Tiling = vec2(1);

out vec2 v_TexCoord;
out vec3 v_WorldPos;
out mat3 v_TBNMatrix;

void main()
{
    vec4 localPos = vec4(a_Position, 1.0);
    vec4 worldPos = lp_Transform * localPos;
    gl_Position = lp_Projection * lp_View * worldPos;

    v_TexCoord = a_TexCoord * u_Tiling;
    
    // Use Normal Matrix for transformations to handle scaling correctly
    mat3 normalMatrix = mat3(transpose(inverse(lp_Transform)));
    vec3 normal = normalize(normalMatrix * a_Normal);
    vec3 tangent = normalize(normalMatrix * a_Tangent);
    
    v_WorldPos = worldPos.xyz;
    vec3 biTangent = cross(normal, tangent);
    v_TBNMatrix = mat3(tangent, biTangent, normal);
}

[fragment]
#version 460 core
in vec2 v_TexCoord;
in vec3 v_WorldPos;
in mat3 v_TBNMatrix;

out vec4 FragColor;

struct Light
{
    vec4 l_ColorIntensity;
    vec4 l_PositionType;
    vec4 l_DirectionInnerCone;
    vec4 l_AttenuationOuterCone;
    vec4 l_SMapAndSColor;
};

layout (std140, binding = 0) uniform Matrices
{
    mat4 lp_Projection;
    mat4 lp_View;
};

layout (std140, binding = 1) uniform Lighting
{
    vec4 lp_CameraWorldPosLightCount;
    Light lp_lights[16];
};

uniform sampler2D u_WaterMapA;
uniform sampler2D u_WaterMapB;
uniform sampler2D u_Albedo;
uniform sampler2D lp_SceneDepth;
uniform sampler2D u_FoamTexture;

uniform bool u_UseAlbedo = false;
uniform bool u_IsOrthographic = false; // Toggle this based on camera mode

uniform vec2 u_TilingA = vec2(1);
uniform vec2 u_TilingB = vec2(1);
uniform vec2 u_TilingAlbedo = vec2(1);
uniform vec2 u_TilingFoam = vec2(10.0, 10.0);

uniform vec2 directionA = vec2(-0.08, 0.1);
uniform vec2 directionB = vec2(-0.08, 0.06);
uniform vec2 directionAlbedo = vec2(-0.08, 0.06);

uniform float u_AlbedoDistortionStrength = 0.02;
uniform float speedA = 1.06;
uniform float speedB = 0.93;
uniform float speedAlbedo = 1.0;

uniform float u_Roughness = 128.0; 
uniform vec4 u_FoamColor = vec4 (1.0);
uniform vec4 u_Specular = vec4(0.5);

uniform float u_MinAlpha = 0.39;
uniform float u_MaxAlpha = 1.0;
uniform float u_Fresnel = 0.2;
uniform float u_FoamThreshold = 1.43;
uniform float u_FoamDistortionStrength = 0.9;

uniform vec4 u_ShallowColor = vec4(0.2, 0.5, 0.9, 1.0);
uniform vec4 u_DeepColor = vec4(1.0);
uniform float u_DepthMaxDistance = 3.0;

uniform float lp_Time = 0;
uniform float lp_Near = 0.3;
uniform float lp_Far = 200.0;

// Optimized for both Perspective and Orthographic
float linearDepth(float d) 
{
    if (u_IsOrthographic) {
        return lp_Near + d * (lp_Far - lp_Near);
    }
    return lp_Near * lp_Far / (lp_Far - d * (lp_Far - lp_Near));
}

void main()
{
    // Texture Sampling & Distortion
    vec4 texNormalA = texture(u_WaterMapA, (v_TexCoord * u_TilingA + directionA * lp_Time * speedA));
    vec4 texNormalB = texture(u_WaterMapB, (v_TexCoord * u_TilingB + directionB * lp_Time * speedB));
    vec2 waterDistortion = (texNormalA.rg + texNormalB.rg - 1.0) * u_AlbedoDistortionStrength;
    vec4 texAlbedo = texture(u_Albedo, (v_TexCoord * u_TilingAlbedo + directionAlbedo * lp_Time * speedAlbedo) + waterDistortion);

    // Normal Blending
    vec3 normalA = normalize(v_TBNMatrix * (texNormalA.xyz * 2.0 - 1.0));
    vec3 normalB = normalize(v_TBNMatrix * (texNormalB.xyz * 2.0 - 1.0));
    vec3 blendedNormal = normalize(normalA + normalB);

    // View Direction calculation
    vec3 viewDir;
    if (u_IsOrthographic) {
        // Parallel view direction extracted from the view matrix
        viewDir = normalize(vec3(lp_View[0][2], lp_View[1][2], lp_View[2][2]));
    } else {
        viewDir = normalize(lp_CameraWorldPosLightCount.xyz - v_WorldPos);
    }

    // Depth & Foam
    vec2 screenUV = gl_FragCoord.xy / textureSize(lp_SceneDepth, 0);
    float sceneDepthRaw = texture(lp_SceneDepth, screenUV).r;
    float sceneDepthLinear = linearDepth(sceneDepthRaw);
    float waterDepthLinear = linearDepth(gl_FragCoord.z);
    float depthDiff = sceneDepthLinear - waterDepthLinear;
    
    float foamDistortion = (texNormalA.r + texNormalB.r) * 0.5;
    float foamAmount = 1.0 - smoothstep(0.0, u_FoamThreshold, depthDiff + foamDistortion * u_FoamDistortionStrength);
    vec2 foamUV = v_TexCoord * u_TilingFoam + (directionA + directionB) * lp_Time * speedA;
    vec3 foamSample = texture(u_FoamTexture, foamUV).rgb;

    // Lighting Loop
    vec3 totalDiffuse = vec3(0.0);
    vec3 totalSpecular = vec3(0.0);

    for (int i = 0; i < int(lp_CameraWorldPosLightCount.w); ++i)
    {
        int type = int(lp_lights[i].l_PositionType.w);
        vec3 lightDir;
        float attenuation = 1.0;

        if (type == 0) { // Ambient
            totalDiffuse += lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w;
            continue;
        } 
        else if (type == 1) { // Directional
            lightDir = normalize(-lp_lights[i].l_DirectionInnerCone.xyz);
        } 
        else { // Point (3) or Spot (2)
            vec3 toLight = lp_lights[i].l_PositionType.xyz - v_WorldPos;
            float d = length(toLight);
            lightDir = toLight / d;
            attenuation = 1.0 / (lp_lights[i].l_AttenuationOuterCone.x + lp_lights[i].l_AttenuationOuterCone.y * d + lp_lights[i].l_AttenuationOuterCone.z * d * d);
            
            if (type == 2) { // Spot cone logic
                float angle = dot(lp_lights[i].l_DirectionInnerCone.xyz, -lightDir);
                float inner = cos(radians(lp_lights[i].l_DirectionInnerCone.w));
                float outer = cos(radians(lp_lights[i].l_AttenuationOuterCone.w));
                attenuation *= smoothstep(outer, inner, angle);
            }
        }

        // Diffuse
        float diff = max(dot(blendedNormal, lightDir), 0.0);
        totalDiffuse += diff * lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w * attenuation;

        // Specular (Blinn-Phong)
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(blendedNormal, halfwayDir), 0.0), u_Roughness);
        totalSpecular += u_Specular.xyz * spec * lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w * attenuation;
    }

    // Fresnel and Depth Color
    float fresnel = u_Fresnel + (1.0 - u_Fresnel) * pow(1.0 - max(dot(blendedNormal, viewDir), 0.0), 5.0);
    float depthFactor = clamp(depthDiff / u_DepthMaxDistance, 0.0, 1.0);
    float depthAlpha = mix(u_MinAlpha, u_MaxAlpha, depthFactor);
    float transparency = clamp(fresnel + depthAlpha, u_MinAlpha, u_MaxAlpha);
    
    vec3 depthColor = mix(u_ShallowColor.xyz, u_DeepColor.xyz, depthFactor);
    vec3 surfaceColor = u_UseAlbedo ? texAlbedo.rgb * depthColor : depthColor;
    
    // Final Composition
    vec3 waterResult = totalDiffuse * surfaceColor + totalSpecular;
    vec3 finalColor = mix(waterResult, foamSample * u_FoamColor.rgb, foamAmount);
    float finalAlpha = mix(transparency, 1.0, foamAmount);
    
    FragColor = vec4(finalColor, finalAlpha);
}