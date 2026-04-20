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
    private bool openingHasStarted = false;

    private enum introState
    {
        DELAY_PRE_ANIMATION,
        IN_ANIMATION
    }
    private introState currentState = introState.DELAY_PRE_ANIMATION;

    private SpriteAnimator animator;
    private bool quickStartAnimation = true;

    void OnCreate()
    {
        
        if (backgroundEntity != null)
        {
            background = backgroundEntity.GetComponent<Image>();
            animator = entity.GetComponent<SpriteAnimator>();
        }
        else
        {
            Debug.Log("Error: There is no background Image+Sprite Animator Entity assigned.");
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
        if (quickStartAnimation)
        {
            animator.Play();
            animator.Stop();
            quickStartAnimation = false;
        }

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
        if (!openingHasStarted)
        {
            animator.Play();
            openingMusicAudioSource.Play();
            openingHasStarted = true;
        }
        else if (animator.CurrentFrame == animator.FrameCount - 1)
        {
            backgroundEntity.SetActive(false);
            hasIntroEnded = true;
        }
    }

    public float GetTotalPreAnimationDelay()
    {
        return totalPreAnimationDelay;
    }

    public bool HasOpeningStarted()
    {
        return openingHasStarted;
    }
};