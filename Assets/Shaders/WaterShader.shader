[vertex]
#version 460 core
layout (location = 0) in vec3 a_Position;
layout (location = 1) in vec2 a_TexCoord;
layout (location = 2) in vec3 a_Normal;
layout (location = 3) in vec3 a_Tangent;

layout (std140, binding = 0) uniform Matrices {
    mat4 lp_Projection;
    mat4 lp_View;
};

uniform mat4 lp_Transform;
uniform vec2 u_Tiling = vec2(1.0);

out vec2 v_TexCoord;
out vec3 v_WorldPos;
out mat3 v_TBNMatrix;

void main() {
    vec4 worldPos = lp_Transform * vec4(a_Position, 1.0);
    v_WorldPos = worldPos.xyz;
    gl_Position = lp_Projection * lp_View * worldPos;

    v_TexCoord = a_TexCoord * u_Tiling;

    // Normal matrix to handle non-uniform scaling correctly
    mat3 normalMatrix = transpose(inverse(mat3(lp_Transform)));
    vec3 T = normalize(normalMatrix * a_Tangent);
    vec3 N = normalize(normalMatrix * a_Normal);
    T = normalize(T - dot(T, N) * N); // Re-orthogonalize TBN
    vec3 B = cross(N, T);
    
    v_TBNMatrix = mat3(T, B, N);
}

[fragment]
#version 460 core

in vec2 v_TexCoord;
in vec3 v_WorldPos;
in mat3 v_TBNMatrix;
out vec4 FragColor;

struct Light {
    vec4 l_ColorIntensity;          // Color (rgb) + Intensity (w)
    vec4 l_PositionType;            // Position (xyz) + Type (w: 0=Amb, 1=Dir, 2=Spot, 3=Point)
    vec4 l_DirectionInnerCone;      // Direction (xyz) + Inner Cone Angle (w)
    vec4 l_AttenuationOuterCone;    // Atten Factors (xyz) + Outer Cone Angle (w)
    vec4 l_SMapAndSColor;           // Shadow Data
};

layout (std140, binding = 0) uniform Matrices {
    mat4 lp_Projection;
    mat4 lp_View;
};

layout (std140, binding = 1) uniform Lighting {
    vec4 lp_CameraWorldPosLightCount; // xyz: Pos, w: Count
    Light lp_lights[16];
};

uniform sampler2D u_WaterMapA;
uniform sampler2D u_WaterMapB;
uniform sampler2D u_Albedo;
uniform sampler2D lp_SceneDepth;
uniform sampler2D u_FoamTexture;

uniform bool u_UseAlbedo = false;
uniform vec2 u_TilingA = vec2(1);
uniform vec2 u_TilingB = vec2(1);
uniform vec2 u_TilingAlbedo = vec2(1);
uniform vec2 u_TilingFoam = vec2(10.0);

uniform vec2 directionA = vec2(-0.08, 0.1);
uniform vec2 directionB = vec2(-0.08, 0.06);
uniform vec2 directionAlbedo = vec2(-0.08, 0.06);

uniform float speedA = 1.06;
uniform float speedB = 0.93;
uniform float speedAlbedo = 1.0;

uniform float u_AlbedoDistortionStrength = 0.02;
uniform float u_Roughness = 128.0;
uniform vec4 u_FoamColor = vec4(1.0);
uniform vec4 u_Specular = vec4(0.5);
uniform float u_MinAlpha = 0.39;
uniform float u_MaxAlpha = 1.0;
uniform float u_Fresnel = 0.2;
uniform float u_FoamThreshold = 1.43;
uniform float u_FoamDistortionStrength = 0.5;

uniform vec4 u_ShallowColor = vec4(0.2, 0.5, 0.9, 1.0);
uniform vec4 u_DeepColor = vec4(1.0);
uniform float u_DepthMaxDistance = 3.0;

uniform float lp_Time = 0;
uniform float lp_Near = 0.3;
uniform float lp_Far = 200.0;

// Helper to linearize depth based on projection type
float getLinearDepth(float depthSample) {
    // Detect Orthographic vs Perspective via Projection Matrix [3][3]
    // In Perspective, [3][3] is 0; in Orthographic, it is 1.
    bool isOrthographic = lp_Projection[3][3] == 1.0;
    
    if (isOrthographic) {
        return lp_Near + depthSample * (lp_Far - lp_Near);
    } else {
        float z = depthSample * 2.0 - 1.0; 
        return (2.0 * lp_Near * lp_Far) / (lp_Far + lp_Near - z * (lp_Far - lp_Near));
    }
}

void main() {
    // Normal mapping with panning
    vec4 texNormalA = texture(u_WaterMapA, v_TexCoord * u_TilingA + directionA * lp_Time * speedA);
    vec4 texNormalB = texture(u_WaterMapB, v_TexCoord * u_TilingB + directionB * lp_Time * speedB);
    
    // Wave distortion vector
    vec2 waterDistortion = (texNormalA.rg + texNormalB.rg - 1.0) * u_AlbedoDistortionStrength;
    
    // Blend world-space normals
    vec3 normalA = normalize(v_TBNMatrix * (texNormalA.xyz * 2.0 - 1.0));
    vec3 normalB = normalize(v_TBNMatrix * (texNormalB.xyz * 2.0 - 1.0));
    vec3 blendedNormal = normalize(normalA + normalB);
    
    vec3 viewDir = normalize(lp_CameraWorldPosLightCount.xyz - v_WorldPos);

    // Depth-based effects (Foam & Shoreline)
    vec2 screenUV = gl_FragCoord.xy / textureSize(lp_SceneDepth, 0);
    float sceneDepth = getLinearDepth(texture(lp_SceneDepth, screenUV).r);
    float waterDepth = getLinearDepth(gl_FragCoord.z);
    float depthDiff = sceneDepth - waterDepth;

    // Foam Logic
    vec2 foamUV = v_TexCoord * u_TilingFoam + (directionA + directionB) * lp_Time * speedA;
    vec3 foamSample = texture(u_FoamTexture, foamUV + waterDistortion * u_FoamDistortionStrength).rgb;
    float foamAmount = 1.0 - smoothstep(0.0, u_FoamThreshold, depthDiff + (texNormalA.r * u_FoamDistortionStrength));
    foamAmount = clamp(foamAmount, 0.0, 1.0);

    // Lighting
    vec3 totalDiffuse = vec3(0.0);
    vec3 totalSpecular = vec3(0.0);
    int lightCount = int(lp_CameraWorldPosLightCount.w);

    for (int i = 0; i < lightCount; ++i) {
        vec3 lightDir;
        float attenuation = 1.0;
        int type = int(lp_lights[i].l_PositionType.w);

        if (type == 0) { // Ambient
            totalDiffuse += lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w;
            continue;
        } else if (type == 1) { // Directional
            lightDir = normalize(-lp_lights[i].l_DirectionInnerCone.xyz);
        } else { // Point/Spot
            vec3 toLight = lp_lights[i].l_PositionType.xyz - v_WorldPos;
            float d = length(toLight);
            lightDir = toLight / d;
            attenuation = 1.0 / (lp_lights[i].l_AttenuationOuterCone.x + lp_lights[i].l_AttenuationOuterCone.y * d + lp_lights[i].l_AttenuationOuterCone.z * d * d);
            
            if (type == 2) { // Spot
                float angle = dot(lp_lights[i].l_DirectionInnerCone.xyz, -lightDir);
                attenuation *= smoothstep(cos(radians(lp_lights[i].l_AttenuationOuterCone.w)), cos(radians(lp_lights[i].l_DirectionInnerCone.w)), angle);
            }
        }

        // Blinn-Phong Diffuse & Specular
        float diff = max(dot(blendedNormal, lightDir), 0.0);
        totalDiffuse += diff * lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w * attenuation;

        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(blendedNormal, halfwayDir), 0.0), u_Roughness);
        totalSpecular += u_Specular.xyz * spec * lp_lights[i].l_ColorIntensity.xyz * lp_lights[i].l_ColorIntensity.w * attenuation;
    }

    // Colors and Transparency
    float fresnel = u_Fresnel + (1.0 - u_Fresnel) * pow(1.0 - max(dot(blendedNormal, viewDir), 0.0), 5.0);
    float depthFactor = clamp(depthDiff / u_DepthMaxDistance, 0.0, 1.0);
    vec3 depthColor = mix(u_ShallowColor.xyz, u_DeepColor.xyz, depthFactor);
    
    if (u_UseAlbedo) {
        vec2 albedoUV = v_TexCoord * u_TilingAlbedo + directionAlbedo * lp_Time * speedAlbedo;
        depthColor *= texture(u_Albedo, albedoUV + waterDistortion).rgb;
    }

    vec3 waterResult = (totalDiffuse * depthColor) + totalSpecular;
    vec3 finalColor = mix(waterResult, foamSample * u_FoamColor.rgb, foamAmount);
    
    float alpha = clamp(fresnel + (depthFactor * u_MaxAlpha), u_MinAlpha, 1.0);
    FragColor = vec4(finalColor, mix(alpha, 1.0, foamAmount));
}