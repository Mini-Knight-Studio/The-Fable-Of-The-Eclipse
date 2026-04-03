using System;
using Loopie;

class IntroBookCover : Component
{
    // Modyfiable values
    public Entity backgroundEntity;
    private Image background;

    public Entity introMiniKnightStudioEntity;
    private IntroMiniKnightStudio introMiniKnightStudioScript;

    public Entity openingMusicEntity;
    private AudioSource openingMusicAudioSource;

    public float preAnimationDelay = 2f;
    public float inAnimationDelay = 2f;

    // Internal values
    private float totalPreAnimationDelay = 0f;
    private float timer = 0f;
    private float currentBackgroundOpacity = 1f;

    private bool hasIntroEnded = false;
    private bool openingMusicHasPlayed = false;

    private enum introState
    {
        DELAY_PRE_ANIMATION,
        IN_ANIMATION
    }
    private introState currentState = introState.DELAY_PRE_ANIMATION;

    void OnCreate()
    {
        if (backgroundEntity != null)
        {
            background = backgroundEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no background Image Entity assigned.");
        }

        if (introMiniKnightStudioEntity != null)
        {
            introMiniKnightStudioScript = introMiniKnightStudioEntity.GetComponent<IntroMiniKnightStudio>();
            totalPreAnimationDelay = preAnimationDelay + introMiniKnightStudioScript.preTextDelay + introMiniKnightStudioScript.textFadeInDelay + introMiniKnightStudioScript.onTextDelay + introMiniKnightStudioScript.textFadeOutDelay + introMiniKnightStudioScript.afterTextDelay;
        }
        else
        {
            Debug.Log("Error: There is no IntroMiniKnightStudio Entity assigned.");
        }

        if (openingMusicEntity != null)
        {
            openingMusicAudioSource = openingMusicEntity.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Error: There is no Opening Music Entity assigned.");
        }
    }

    void OnUpdate()
    {
        if (!hasIntroEnded)
        {
            timer += Time.deltaTime;

            switch (currentState)
            {
                case introState.DELAY_PRE_ANIMATION: UpdateDelayPreAnimation(); break;
                case introState.IN_ANIMATION: UpdateInAnimation(); break;
            }
        }
    }

    void UpdateDelayPreAnimation()
    {
        if (timer >= totalPreAnimationDelay)
        {
            currentState = introState.IN_ANIMATION;
            timer = 0f;
        }
    }

    void UpdateInAnimation()
    {
        if (timer >= inAnimationDelay)
        {
            backgroundEntity.SetActive(false);
            hasIntroEnded = true;
            timer = 0f;
        }
        else
        {
            if (!openingMusicHasPlayed)
            {
                openingMusicAudioSource.Play();
                openingMusicHasPlayed = true;
            }

            currentBackgroundOpacity = Mathf.Lerp(1, 0, timer / inAnimationDelay);
            Vector4 color = new Vector4(1, 1, 1, currentBackgroundOpacity);
            background.SetTint(color);
        }
    }

    public float GetTotalPreAnimationDelay()
    {
        return totalPreAnimationDelay;
    }

    public bool HasOpeningMusicPlayed()
    {
        return openingMusicHasPlayed;
    }
};