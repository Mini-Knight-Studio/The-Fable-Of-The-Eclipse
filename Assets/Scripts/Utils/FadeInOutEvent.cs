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
    public event Action OnFadeOutComplete;

    [Header("Settings")]
    public bool StartFadingOut = false;

    [ReadOnly]
    [ShowInInspector]
    private bool isRunning = false;
    
    void OnCreate()
    {
        fadeInImage = fadeInImageEntity.GetComponent<Image>();
        fadeOutImage = fadeOutImageEntity.GetComponent<Image>();

        if(StartFadingOut)
        {
            StartFade(false, true);
        }
    }


    public void StartFade(bool doIn = true, bool doOut = true)
    {
        if (isRunning) return;

        isRunning = true;
        StartCoroutine(FadeSequence(doIn, doOut));
    }

    IEnumerator FadeSequence(bool doIn, bool doOut)
    {
        isRunning = true;
        float elapsedTime;
        Vector4 currentColor;
        float halfStatic = timeStatic / 2f;

        if (doIn)
        {
            fadeInImageEntity.SetActive(true);
            elapsedTime = 0f;

            while (elapsedTime < fadeInTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeInTime);

                currentColor = fadeInImage.GetTint();
                currentColor.w = alpha;
                fadeInImage.SetTint(currentColor);
                yield return null;
            }

            OnFadeInComplete?.Invoke();

            yield return new WaitForSeconds(halfStatic);

            if (doOut && fadeInImageEntity != fadeOutImageEntity)
            {
                fadeInImageEntity.SetActive(false);
            }
        }

        if (doOut)
        {
            fadeOutImageEntity.SetActive(true);

            currentColor = fadeOutImage.GetTint();
            currentColor.w = 1f;
            fadeOutImage.SetTint(currentColor);

            yield return new WaitForSeconds(halfStatic);

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

            fadeOutImageEntity.SetActive(false);
            OnFadeOutComplete?.Invoke();
        }

        isRunning = false;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};