using System;
using System.Threading;
using Loopie;

class IntroMiniKnightStudio : Component
{
    // Modyfiable values
    public Entity backgroundEntity;
    private Image background;

    public Entity textEntity;
    private Text text;

    public float preTextDelay = 2f;
    public float textFadeInDelay = 2f;
    public float onTextDelay = 2f;
    public float textFadeOutDelay = 2f;
    public float afterTextDelay = 2f;

    // Internal values
    private float timer = 0f;
    private float currentTextOpacity = 0f;
    private float currentBackgroundOpacity = 1f;

    private bool hasIntroEnded = false;

    private enum introState
    {
        DELAY_PRE_TEXT,
        FADE_IN_TEXT,
        DELAY_ON_TEXT,
        FADE_OUT_TEXT,
        DELAY_AFTER_TEXT
    }
    private introState currentState = introState.DELAY_PRE_TEXT;

    void OnCreate()
    {
        if (textEntity != null)
        {
            text = textEntity.GetComponent<Text>();
        }
        else
        {
            Debug.Log("Error: There is no Text Entity assigned.");
        }

        if (backgroundEntity != null)
        {
            background = backgroundEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no background Image Entity assigned.");
        }
    }

    void OnUpdate()
    {
        if (!hasIntroEnded)
        {
            timer += Time.deltaTime;

            switch (currentState)
            {
                case introState.DELAY_PRE_TEXT: UpdateDelayPreText(); break;
                case introState.FADE_IN_TEXT: UpdateFadeInText(); break;
                case introState.DELAY_ON_TEXT: UpdateOnText(); break;
                case introState.FADE_OUT_TEXT: UpdateFadeOutText(); break;
                case introState.DELAY_AFTER_TEXT: UpdateDelayAfterText(); break;
            }
        }
    }

    void UpdateDelayPreText()
    {
        if (timer >= preTextDelay)
        {
            textEntity.SetActive(true);
            currentState = introState.FADE_IN_TEXT;
            timer = 0f;
        }
    }

    void UpdateFadeInText()
    {
        if (timer >= textFadeInDelay)
        {
            currentState = introState.DELAY_ON_TEXT;
            timer = 0f;
        }
        else
        {
            currentTextOpacity = Mathf.Lerp(0, 1, timer / textFadeInDelay);
            // Assign currentTextOpacity to text.
        }
    }

    void UpdateOnText()
    {
        if (timer >= onTextDelay)
        {
            currentState = introState.FADE_OUT_TEXT;
            timer = 0f;
        }
    }

    void UpdateFadeOutText()
    {
        if (timer >= textFadeOutDelay)
        {
            textEntity.SetActive(false);
            currentState = introState.DELAY_AFTER_TEXT;
            timer = 0f;
        }
        else
        {
            currentTextOpacity = Mathf.Lerp(1, 0, timer / textFadeOutDelay);
            // Assign currentTextOpacity to text.
        }
    }

    void UpdateDelayAfterText()
    {
        if (timer >= afterTextDelay)
        {
            backgroundEntity.SetActive(false);
            timer = 0f;
            hasIntroEnded = true;
        }
        else
        {
            currentBackgroundOpacity = Mathf.Lerp(1, 0, timer / afterTextDelay);
            Vector4 color = new Vector4(0, 0, 0, currentBackgroundOpacity);
            background.SetTint(color);
        }
    }
};