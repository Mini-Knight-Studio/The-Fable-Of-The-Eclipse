using System;
using System.Collections;
using Loopie;

public class BlinkEffect : Component
{
    private MeshRenderer targetRenderer;
    private Material instancedMaterial;

    [Header("References")]
    public Entity meshEntity;

    [Header("Blink Settings")]
    public float maxIntensity = 10.0f;
    public float blinkDuration = 0.05f;
    public float defaultIntensity = 0.030f;

    void OnCreate()
    {
        if (meshEntity != null)
        {
            targetRenderer = meshEntity.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                instancedMaterial = targetRenderer.GetInstancedMaterial();
            }
        }
    }

    public void TriggerBlink()
    {
        if (instancedMaterial == null) return;

        StopAllCoroutines();
        StartCoroutine(FlashSequence());
    }

    private IEnumerator FlashSequence()
    {
        instancedMaterial.SetFloat("u_EmissiveIntensity", maxIntensity);

        yield return new WaitForSeconds(blinkDuration);

        instancedMaterial.SetFloat("u_EmissiveIntensity", defaultIntensity);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}