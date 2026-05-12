using System;
using System.Collections;
using Loopie;

class Gem_Idle : Component
{
    [Header("References")]
    public Entity gemModel;
    public Entity interactionPrompt;

    [Header("Settings")]
    public float amplitude = 0.5f;
    public float speed = 2.0f;
    private Vector3 startLocalPos;
    private float time;


    void OnCreate()
    {
        startLocalPos = transform.local_position;
        time = 0f;

        interactionPrompt.SetActive(false);
        StartCoroutine(GemShineCoroutine());
    }

    void OnUpdate()
    {
        time += Time.deltaTime * speed;

        float offsetY = (float)Math.Sin(time) * amplitude;

        transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
    }

    IEnumerator GemShineCoroutine()
    {
        if (gemModel == null) yield break;
        Material gemMaterial = gemModel.GetComponent<MeshRenderer>().GetInstancedMaterial();

        float pulseSpeed = 1f;

        float minIntensity = 0.3f;
        float maxIntensity = 0.6f;

        float minRoughness = 0.1f;
        float maxRoughness = 30f;

        float elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(elapsedTime * pulseSpeed) + 1f) / 2f);
            gemMaterial.SetFloat("u_EmissiveIntensity", intensity);

            float roughness = Mathf.Lerp(minRoughness, maxRoughness, (Mathf.Sin(elapsedTime * pulseSpeed * 2f) + 1f) / 2f);
            gemMaterial.SetFloat("u_Roughness", roughness);

            yield return null;
        }

    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};