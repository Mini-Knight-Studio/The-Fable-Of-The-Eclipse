using System;
using System.Collections;
using Loopie;

public class FadeInOutEvent : Component
{
    [Header("Images")]
    public Entity fadeInImageEntity;
    private Image fadeInImage;
    public Entity fadeOutImageEntity;
    private Image fadeOutImage;

    [Header("Fade Times")]
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    [Header("Static Time")]
    public float timeStatic = 1f;

    public event Action OnFadeInComplete;

    [ReadOnly]
    [ShowInInspector]
    private bool isRunning = false;
    
    void OnCreate()
    {
        fadeInImage = fadeInImageEntity.GetComponent<Image>();
        fadeOutImage = fadeOutImageEntity.GetComponent<Image>();
    }


    public void StartFade()
    {
        if (isRunning) return;

        isRunning = true;
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        Vector4 currentColor = fadeInImage.GetTint();
        currentColor.w = 0f;
        fadeInImage.SetTint(currentColor);

        currentColor = fadeOutImage.GetTint();
        currentColor.w = 1f;
        fadeOutImage.SetTint(currentColor);

        fadeInImageEntity.SetActive(true);
        fadeOutImageEntity.SetActive(false);

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInTime);

            currentColor = fadeInImage.GetTint();
            currentColor.w = alpha;
            fadeInImage.SetTint(currentColor);

            yield return null;
        }

        yield return new WaitForSeconds(timeStatic/2);
        fadeInImageEntity.SetActive(false);
        OnFadeInComplete?.Invoke();
        fadeOutImageEntity.SetActive(true);
        yield return new WaitForSeconds(timeStatic/2);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeOutTime);

            currentColor = fadeOutImage.GetTint();
            currentColor.w = alpha;
            fadeOutImage.SetTint(currentColor);

            yield return null;
        }
        isRunning = false;

        fadeInImageEntity.SetActive(false);
        fadeOutImageEntity.SetActive(false);

        //OnFadeInComplete = null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};