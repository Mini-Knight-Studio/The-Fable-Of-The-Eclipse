using System;
using System.Threading;
using Loopie;

class Intro_MiniKnightStudio : Component
{
    public Text text;

    float timer = 0f;

    public float preTextDelay = 2f;
    public float textFadeInDelay = 2f;
    public float onTextDelay = 2f;
    public float textFadeOutDelay = 2f;
    public float afterTextDelay = 2f; 

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
        
    }

    void OnUpdate()
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

    void UpdateDelayPreText()
    {
        if (timer >= preTextDelay)
        {
            currentState = introState.FADE_IN_TEXT;
            timer = 0f; 
        }
    }
    void UpdateFadeInText()
    {
        if (timer >= preTextDelay)
        {
            currentState = introState.DELAY_ON_TEXT;
            timer = 0f; 
        }
    }
    void UpdateOnText()
    {
        if (timer >= preTextDelay)
        {
            currentState = introState.FADE_OUT_TEXT;
            timer = 0f; 
        }
    }
    void UpdateFadeOutText()
    {
        if (timer >= preTextDelay)
        {
            currentState = introState.DELAY_AFTER_TEXT;
            timer = 0f; 
        }
    }
    void UpdateDelayAfterText()
    {
        if (timer >= preTextDelay)
        {
            // Deactivate all
            timer = 0f; 
        }
    }
};