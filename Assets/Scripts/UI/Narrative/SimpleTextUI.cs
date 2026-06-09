using System;
using System.Collections;
using Loopie;

class SimpleTextUI : Component
{
    public Entity containerEntity;
    public Entity textEntity;
    [HideInInspector] public Text text;
    private CanvasGroup canvasGroup;

    public static SimpleTextUI Instance { get; private set; }
    void OnCreate()
    {
        if (Instance == null)
            Instance = this;
        else
            return;
        
        text = textEntity.GetComponent<Text>();
        canvasGroup = containerEntity.GetComponent<CanvasGroup>();
        containerEntity.SetActive(false);
    }

    public void SetText(string value)
    {
        if (text != null)
            text.SetText(value);
    }

    public void Open(float fadeInDuration = 0)
    {
        StopAllOwnedCoroutines();
        containerEntity.SetActive(true);
        if(canvasGroup != null && fadeInDuration > 0)
        {
            canvasGroup.Alpha = 0;
            StartCoroutine(FadeCanvas(0f, 1f, fadeInDuration));
        }
    }

    public void Close(float fadeOutDuration = 0)
    {
        StopAllOwnedCoroutines();
        
        if (canvasGroup != null && fadeOutDuration > 0)
        {
            canvasGroup.Alpha = 1;
            StartCoroutine(FadeCanvas(1f, 0f, fadeOutDuration));
        }
        else
        {
            containerEntity.SetActive(false);
        }
    }

    IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        canvasGroup.Alpha = startAlpha;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.Alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }

        canvasGroup.Alpha = endAlpha;

        if(endAlpha == 0f)
        {
            containerEntity.SetActive(false);
        }
    }
};