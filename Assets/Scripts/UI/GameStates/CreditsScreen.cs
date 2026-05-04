using System;
using System.Collections;
using Loopie;

public class CreditsScreen : Component
{
    [Header("References")]
    public Entity backgroundEntity;
    public Entity decorationEntity;
    public Entity creditsTextEntity;
    public string targetSceneUUID;

    Image backgroundImage;
    Image decorationImage;
    Text creditsText;

    [Header("Settings")]
    [ShowInInspector]
    float slowmoDuration = 1;
    [ShowInInspector]
    float showAnimationDuration = 2;
    [ShowInInspector]
    float creditsAnimationDuration = 10;
    [ShowInInspector]
    Vector2 startCreditsPosition;
    [ShowInInspector]
    Vector2 endCreditsPosition;

    [Header("Colors")]
    [ShowInInspector] Color colorBackground;


    void OnCreate()
    {
        backgroundImage = backgroundEntity.GetComponent<Image>();
        decorationImage = decorationEntity.GetComponent<Image>();
        creditsText = creditsTextEntity.GetComponent<Text>();

        Initialize();
    }

    private void Initialize()
    {
        Vector4 startBackgroundColor = colorBackground.rgba;
        startBackgroundColor.w = 0;
        backgroundImage.SetTint(startBackgroundColor);
        decorationImage.SetTint(startBackgroundColor);

        RectTransform creditsRect = creditsTextEntity.GetComponent<RectTransform>();
        creditsRect.anchored_position = startCreditsPosition;
    }

    public void OpenCreditsScreen()
    {
        entity.SetActive(true);
        StartCoroutine(Show());
    }


    IEnumerator Show()
    {

        float timer = 0;

        while (true)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / slowmoDuration);

            float curvedT = Mathf.Lerp(1f, 0f, t, Mathf.LerpCurve.ExponentialOut, 1, Mathf.LerpStrengthMode.Power);

            Time.timeScale = curvedT;

            if (t >= 1f)
            {
                Time.timeScale = 0f;
                break;
            }

            yield return null;
        }

        timer = 0;
        float targetTime = showAnimationDuration;

        Vector4 startBackgroundColor = colorBackground.rgba;
        startBackgroundColor.w = 0;

        backgroundImage.SetTint(startBackgroundColor);
        decorationImage.SetTint(startBackgroundColor);

        RectTransform creditsRect = creditsTextEntity.GetComponent<RectTransform>();

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > targetTime)
                break;

            float lerpStep = timer / targetTime;
            backgroundImage.SetTint(Vector4.Lerp(startBackgroundColor, colorBackground.rgba, lerpStep));
            decorationImage.SetTint(Vector4.Lerp(startBackgroundColor, colorBackground.rgba, lerpStep));

            yield return null;
        }

        backgroundImage.SetTint(colorBackground.rgba);

        timer = 0;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > creditsAnimationDuration)
                break;
            float lerpStep = timer / creditsAnimationDuration;
            creditsRect.anchored_position = (Vector2.Lerp(startCreditsPosition, endCreditsPosition, lerpStep));
            yield return null;
        }

        yield return new WaitForUnscaledSeconds(2);

        Time.timeScale = 1;
        SceneManager.LoadSceneByID(targetSceneUUID);
    }


    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};