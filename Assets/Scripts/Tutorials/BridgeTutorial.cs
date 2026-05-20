using System;
using System.Collections;
using Loopie;

public class BridgeTutorial : Component
{
    public Entity bridgeEntity;
    public Entity bridgeMeshEntity;
    public Entity dialogEntity;

    public float minIntensityGlow;
    public float maxIntensityGlow;
    public float blinkDuration;
    public float blinkRestoreDuration;

    private RollingBridge bridgeLogic;
    private MeshRenderer meshRenderer;
    private DialogTrigger trigger;
    private Material meshMaterial;

    bool glow;
    void OnCreate()
    {
        bridgeLogic = bridgeEntity.GetComponent<RollingBridge>();
        meshRenderer = bridgeMeshEntity.GetComponent<MeshRenderer>();
        trigger = dialogEntity.GetComponent<DialogTrigger>();
        meshMaterial = meshRenderer.GetInstancedMaterial();
    }

    void OnPostCreate()
    {
        trigger.OnDialogStart += StartGlow;
        trigger.OnDialogEnd += EndGlow;
    }

    public void StartGlow()
    {
        if (bridgeLogic.IsDown())
            return;

        glow = true;
        StartCoroutine(Glow());
    }

    public void EndGlow()
    {
        glow = false;
    }
    
    IEnumerator Glow()
    {
        bool goingUp = true;
        float currentGlow = 0;
        float timer = 0;

        while (glow)
        {
            timer+= Time.deltaTime; 
            if(goingUp)
                currentGlow = Mathf.Lerp(minIntensityGlow, maxIntensityGlow, timer/blinkDuration);
            else
                currentGlow = Mathf.Lerp(maxIntensityGlow, minIntensityGlow, timer/blinkDuration);
            
            meshMaterial.SetFloat("u_EmissiveIntensity", currentGlow);

            if (currentGlow >= maxIntensityGlow)
            {
                yield return new WaitForSeconds(0.2f);
                goingUp = false;
                timer = 0;
            }
            else if (currentGlow <= minIntensityGlow)
            {
                yield return new WaitForSeconds(0.2f);
                goingUp = true;
                timer = 0;
            }

            yield return null;

        }

        timer = 0;
        float resetGlow = 0;
        while (true)
        {
            timer += Time.deltaTime;
            resetGlow = Mathf.Lerp(currentGlow, minIntensityGlow, timer / blinkDuration);

            meshMaterial.SetFloat("u_EmissiveIntensity", resetGlow);
            if (resetGlow <= 0)
                break;
            yield return null;
        }
    }

    void OnDestroy()
    {
        trigger.OnDialogStart -= StartGlow;
        trigger.OnDialogEnd -= EndGlow;

        StopAllOwnedCoroutines();
    }
};