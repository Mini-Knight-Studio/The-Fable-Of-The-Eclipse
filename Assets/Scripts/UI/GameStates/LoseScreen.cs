using System;
using System.Collections;
using Loopie;

public class LoseScreen : Component
{
    [Header("References")]
    public Entity backgroundEntity;
    public Entity vignetteEntity;
    public Entity loseTextEntity;
    public Entity respawnTextEntity;

    Image backgroundImage;
    Image vignetteImage;
    Text loseText;
    Text respawnText;

    [Header("Settings")]
    [ShowInInspector]
    float slowmoDuration = 1;
    [ShowInInspector]
    float showAnimationDuration = 2;
    [ShowInInspector]
    float hideAnimationDuration = 2;

    [Header("Colors")]
    [ShowInInspector] Color colorBackground;
    [ShowInInspector] Color colorLoseText;
    [ShowInInspector] Color colorRespawnText;



    bool canRecieveInput = false;


    void OnCreate()
    {
        backgroundImage = backgroundEntity.GetComponent<Image>();
        vignetteImage = vignetteEntity.GetComponent<Image>();
        loseText = loseTextEntity.GetComponent<Text>();
        respawnText = respawnTextEntity.GetComponent<Text>();

        InitializeColors();
    }

    void OnUpdate()
    {
        if (!canRecieveInput)
            return;

        if (Input.IsKeyDown(KeyCode.A))
        {
            Player.Instance.GoToLastCheckpoint();
            Player.Instance.PlayerHealth.Reset();
            CloseLoseScreen();
        }
    }

    private void InitializeColors()
    {
        Vector4 startBackgroundColor = colorBackground.rgba;
        Vector4 startLoseColor = colorLoseText.rgba;
        Vector4 startRespawnColor = colorRespawnText.rgba;

        Vector4 startVignetteColor = new Vector4(0, 0, 0, 0);
        Vector4 targetVignetteColor = new Vector4(1, 1, 1, 1);

        startBackgroundColor.w = 0;
        startLoseColor.w = 0;
        startRespawnColor.w = 0;

        backgroundImage.SetTint(startBackgroundColor);
        vignetteImage.SetTint(startVignetteColor);
        loseText.SetColor(startLoseColor);
        respawnText.SetColor(startRespawnColor);
    }

    public void OpenLoseScreen()
    {
        entity.SetActive(true);
        StartCoroutine(Show());
    }

    public void CloseLoseScreen()
    {
        StartCoroutine(Hide());
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
        float targetTime = showAnimationDuration / 2;

        Vector4 startBackgroundColor = colorBackground.rgba;
        Vector4 startLoseColor = colorLoseText.rgba;
        Vector4 startRespawnColor = colorRespawnText.rgba;

        Vector4 startVignetteColor = new Vector4(0,0,0,0);
        Vector4 targetVignetteColor = new Vector4(1,1,1,1);

        startBackgroundColor.w = 0;
        startLoseColor.w = 0;
        startRespawnColor.w = 0;

        backgroundImage.SetTint(startBackgroundColor);
        vignetteImage.SetTint(startVignetteColor);
        loseText.SetColor(startLoseColor);
        respawnText.SetColor(startRespawnColor);

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > targetTime)
                break;

            float lerpStep = timer / targetTime;
            backgroundImage.SetTint(Vector4.Lerp(startBackgroundColor, colorBackground.rgba, lerpStep));
            vignetteImage.SetTint(Vector4.Lerp(startVignetteColor, targetVignetteColor, lerpStep));

            yield return null;
        }

        backgroundImage.SetTint(colorBackground.rgba);
        vignetteImage.SetTint(targetVignetteColor);

        timer = 0;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > targetTime)
                break;

            float lerpStep = timer / targetTime;
            loseText.SetColor(Vector4.Lerp(startLoseColor, colorLoseText.rgba, lerpStep));
            respawnText.SetColor(Vector4.Lerp(startRespawnColor, colorRespawnText.rgba, lerpStep));

            yield return null;
        }

        loseText.SetColor(colorLoseText.rgba);
        respawnText.SetColor(colorRespawnText.rgba);

        yield return new WaitForUnscaledSeconds(0.5f);
        canRecieveInput = true;
    }

    IEnumerator Hide()
    {
        canRecieveInput = false;

        float timer = 0;
        float targetTime = hideAnimationDuration / 2;

        Vector4 startBackgroundColor = colorBackground.rgba;
        Vector4 startLoseColor = colorLoseText.rgba;
        Vector4 startRespawnColor = colorRespawnText.rgba;

        Vector4 startVignetteColor = new Vector4(0, 0, 0, 0);
        Vector4 targetVignetteColor = new Vector4(1, 1, 1, 1);

        startBackgroundColor.w = 0;
        startLoseColor.w = 0;
        startRespawnColor.w = 0;

        backgroundImage.SetTint(colorBackground.rgba);
        vignetteImage.SetTint(targetVignetteColor);
        loseText.SetColor(colorLoseText.rgba);
        respawnText.SetColor(colorRespawnText.rgba);


        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > targetTime)
                break;

            float lerpStep = timer / targetTime;

            loseText.SetColor(Vector4.Lerp(colorLoseText.rgba, startLoseColor, lerpStep));
            respawnText.SetColor(Vector4.Lerp(colorRespawnText.rgba, startRespawnColor, lerpStep));

            yield return null;
        }


        loseText.SetColor(startLoseColor);
        respawnText.SetColor(startRespawnColor);

        Time.timeScale = 1;

        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > targetTime)
                break;

            float lerpStep = timer / targetTime;
            backgroundImage.SetTint(Vector4.Lerp(colorBackground.rgba, startBackgroundColor, lerpStep));
            vignetteImage.SetTint(Vector4.Lerp(targetVignetteColor, startVignetteColor, lerpStep));
           

            yield return null;
        }

        backgroundImage.SetTint(startBackgroundColor);
        vignetteImage.SetTint(startVignetteColor);



        entity.SetActive(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};