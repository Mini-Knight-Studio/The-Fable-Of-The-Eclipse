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
    public float maxIntensity = 10.0f;    // Sube esto mucho para un flash potente
    public float blinkDuration = 0.05f;   // Tiempo en segundos que se queda arriba (0.05 es casi instantáneo)
    public float defaultIntensity = 0.030f;

    void OnCreate()
    {
        if (meshEntity != null)
        {
            targetRenderer = meshEntity.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                // Importante: Crea la instancia para no afectar a otros enemigos
                instancedMaterial = targetRenderer.GetInstancedMaterial();
            }
        }
    }

    public void TriggerBlink()
    {
        if (instancedMaterial == null) return;

        // Detenemos cualquier parpadeo que esté a medias para empezar uno nuevo limpio
        StopAllCoroutines();
        StartCoroutine(FlashSequence());
    }

    private IEnumerator FlashSequence()
    {
        // 1. Subida instantánea al máximo
        instancedMaterial.SetFloat("u_EmissiveIntensity", maxIntensity);

        // 2. Espera el tiempo exacto que tú quieras (ej: 0.05 segundos)
        yield return new WaitForSeconds(blinkDuration);

        // 3. Bajada instantánea al valor normal
        instancedMaterial.SetFloat("u_EmissiveIntensity", defaultIntensity);
    }
}