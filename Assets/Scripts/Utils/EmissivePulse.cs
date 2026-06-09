using System;
using Loopie;

public class EmissivePulse : Component
{
    public Entity meshEntity;
    public float min = 0.0f;
    public float max = 1.0f;
    public float speed = 2.0f;
    public bool useIrregularPulse = false;
    
    public bool randomizeStart = true; 

    private Material mat;
    private float elapsedTime = 0f;

    void OnPostCreate()
    {
        if (meshEntity != null && meshEntity.HasComponent<MeshRenderer>())
            mat = meshEntity.GetComponent<MeshRenderer>().GetInstancedMaterial();

        if (randomizeStart)
            elapsedTime = Loopie.Random.Range(0f, 100f); 
    }

    void OnUpdate()
    {
        if (mat == null) return;

        elapsedTime += Time.deltaTime;

        float pulseFactor = 0f;

        if (useIrregularPulse)
        {
            float wave1 = Mathf.Sin(elapsedTime * speed);
            float wave2 = Mathf.Sin(elapsedTime * speed * 3.5f);
            float wave3 = Mathf.Cos(elapsedTime * speed * 7.2f);

            pulseFactor = (wave1 + wave2 + wave3) / 3.0f;
            pulseFactor = (pulseFactor + 1.0f) / 2.0f;
        }
        else
        {
            pulseFactor = (Mathf.Sin(elapsedTime * speed) + 1.0f) / 2.0f;
        }
        float currentIntensity = min + (max - min) * pulseFactor;

        mat.SetFloat("u_EmissiveIntensity", currentIntensity);
    }

    public void Reset()
    {
        mat.SetFloat("u_EmissiveIntensity", 0);
    }
}