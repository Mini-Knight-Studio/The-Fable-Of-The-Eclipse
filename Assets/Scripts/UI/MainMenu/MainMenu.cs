using System;
using Loopie;

class MainMenu : Component
{
    // Buttons
    [Header("Buttons")]
    public Entity newGameEntity;
    private Button newGameButton;
    private NewGame newGameScript;

    public Entity continueEntity;
    private Button continueButton;
    private Load continueScript;

    public Entity settingsEntity;
    private Button settingsButton;
    private SceneTransition settingsScript;

    public Entity exitEntity;
    private Button exitButton;
    private Exit exitScript;

    // Audio
    [Header("Audio")]
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    public Entity selectSfxEntity;
    private AudioSource selectSfxAudioSource;

    private bool loopMusicHasPlayed = false;
    private float openingMusicDelay = 2f;
    private float openingMusicTimer = 0f;
    
    // Intro Book Cover
    [Header("Others")]
    public Entity introBookCoverEntity;
    private IntroBookCover introBookCoverScript;

    private float preMainMenuDelay = 0f;
    private float preMainMenuDelayTimer = 0f;

    // Load
    public Entity LoadSettingsEntity;
    private LoadSettings loadSettingsScript;

    private bool settingsLoaded = false;
    private bool globalDatabaseLoaded = false;

    // On Start
    public Entity closeBookEntity;
    private SpriteAnimator closeBookAnimator;
    public Entity passPageEntity;
    private SpriteAnimator passPageAnimator;
    public Entity invertedPassPageEntity;
    private SpriteAnimator invertedPassPageAnimator;

    [HideInInspector]
    public static bool quickStartAnimations = true;
    [HideInInspector]
    public static bool hasPlayedIntro = false;
    [HideInInspector]
    public static bool invertedPassPagePlayed = true;

    private bool canCallScripts = false;

    public Entity uiManagerEntity;
    private UIManager uiManagerScript;

    void OnCreate()
    {
        // Buttons
        if (newGameEntity != null)
        {
            newGameButton = newGameEntity.GetComponent<Button>();
            newGameScript = newGameEntity.GetComponent<NewGame>();
        }
        else
        {
            Debug.Log("Error: There is no NewGame Entity assigned.");
        }

        if (continueEntity != null)
        {
            continueButton = continueEntity.GetComponent<Button>();
            continueScript = continueEntity.GetComponent<Load>();
        }
        else
        {
            Debug.Log("Error: There is no Continue Entity assigned.");
        }

        if (settingsEntity != null)
        {
            settingsButton = settingsEntity.GetComponent<Button>();
            settingsScript = settingsEntity.GetComponent<SceneTransition>();
        }
        else
        {
            Debug.Log("Error: There is no Settings Entity assigned.");
        }

        if (exitEntity != null)
        {
            exitButton = exitEntity.GetComponent<Button>();
            exitScript = exitEntity.GetComponent<Exit>();
        }
        else
        {
            Debug.Log("Error: There is no Exit Entity assigned.");
        }

        // External
        if (introBookCoverEntity != null)
        {
            introBookCoverScript = introBookCoverEntity.GetComponent<IntroBookCover>();
            preMainMenuDelay = introBookCoverScript.GetTotalPreAnimationDelay() + introBookCoverScript.inAnimationDelay;
        }
        else
        {
            Debug.Log("Error: There is no IntroBookCover Entity assigned.");
        }

        if (loopMusicEntity != null)
        {
            loopMusicAudioSource = loopMusicEntity.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Error: There is no Loop Music Entity assigned.");
        }

        if (selectSfxEntity != null)
        {
            selectSfxAudioSource = selectSfxEntity.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Error: There is no Loop Music Entity assigned.");
        }

        // Load Settings
        if (LoadSettingsEntity != null)
        {
            loadSettingsScript = LoadSettingsEntity.GetComponent<LoadSettings>();
        }
        else
        {
            Debug.Log("Error: There is no LoadSettingsEntity Entity assigned.");
        }

        // On Start
        if (closeBookEntity != null)
        {
            closeBookAnimator = closeBookEntity.GetComponent<SpriteAnimator>();
        }
        else
        {
            Debug.Log("Error: There is no closeBookEntity Entity assigned.");
        }

        if (passPageEntity != null)
        {
            passPageAnimator = passPageEntity.GetComponent<SpriteAnimator>();
        }
        else
        {
            Debug.Log("Error: There is no passPageEntity Entity assigned.");
        }

        if (invertedPassPageEntity != null)
        {
            invertedPassPageAnimator = invertedPassPageEntity.GetComponent<SpriteAnimator>();
        }
        else
        {
            Debug.Log("Error: There is no passPageEntity Entity assigned.");
        }

        if (uiManagerEntity != null)
        {
            uiManagerScript = uiManagerEntity.GetComponent<UIManager>();
        }
        else
        {
            Debug.Log("Error: There is no UIManager Entity assigned.");
        }
    }
    void OnUpdate()
    {
        // On Start
        if (!globalDatabaseLoaded)
        {
            LoadDatabases();
        }

        if (quickStartAnimations)
        {
            PrepareAnimations();
            Open();
        }

        HandleMusic();

        // Nullify Input while in intro.
        preMainMenuDelayTimer += Time.deltaTime;
        preMainMenuDelay = introBookCoverScript.GetTotalPreAnimationDelay() + introBookCoverScript.inAnimationDelay;

        if (!hasPlayedIntro)
        {
            if (preMainMenuDelayTimer < preMainMenuDelay)
                return;

            hasPlayedIntro = true;
        }
        else if (hasPlayedIntro)
        {
            introBookCoverScript.introMiniKnightStudioEntity.SetActive(false);
            introBookCoverEntity.SetActive(false);

            if (!invertedPassPagePlayed)
            {
                invertedPassPageEntity.SetActive(true);
                invertedPassPageAnimator.Play();
                invertedPassPagePlayed = true;
            }
            else
            {
                if (invertedPassPageAnimator.CurrentFrame == invertedPassPageAnimator.StartFrame)
                {
                    uiManagerScript.BlockNavigation = false;
                }
                else 
                {
                    uiManagerScript.BlockNavigation = true;
                }

                if (invertedPassPageAnimator.CurrentFrame == invertedPassPageAnimator.FrameCount - 1)
                {
                    invertedPassPageEntity.SetActive(false);
                    uiManagerScript.BlockNavigation = false;
                }
            }
        }

        if (canCallScripts)
        {
            HandleConfirm();
        }
    }
    void HandleConfirm()
    {
        if (closeBookAnimator.CurrentFrame == closeBookAnimator.FrameCount - 1)
        {
            exitScript.ExitGame();
        }

        if (passPageAnimator.CurrentFrame == passPageAnimator.FrameCount - 1)
        {
            loopMusicAudioSource.Stop();
            quickStartAnimations = false;
            invertedPassPagePlayed = false;
            canCallScripts = false;
            Settings.quickStartAnimations = true;
            Settings.invertedPassPagePlayed = false;
            Pause.isPaused = false;

            if (newGameButton.Hovered)
            {
                newGameScript.StartNewGame();
            }
            else if (continueButton.Hovered) 
            {
                continueScript.LoadPreviousSave();
            }
            else if (settingsButton.Hovered)
            {
                settingsScript.StartTransition();
            }
        }
    }
    void HandleMusic()
    {
        if (loopMusicHasPlayed)
            return;

        if (introBookCoverScript.HasOpeningStarted())
        {
            openingMusicTimer += Time.deltaTime;

            if (openingMusicTimer < openingMusicDelay)
                return;

            loopMusicAudioSource.Play();
            loopMusicHasPlayed = true;
        }

        if (hasPlayedIntro)
        {
            loopMusicAudioSource.Play();
            loopMusicHasPlayed = true;
        }
    }

    public void HandleConfirmAnimation()
    {
        if (exitButton.Hovered)
        {
            closeBookEntity.SetActive(true);
            closeBookAnimator.Play();
        }
        else
        {
            passPageEntity.SetActive(true);
            passPageAnimator.Play();
        }

        canCallScripts = true;
    }

    public void LoadDatabases()
    {
        GlobalDatabase.GlobalData.LoadGlobalDatabase();

        // Load Settings
        if (GlobalDatabase.GlobalData.settingsDB.Settings.AreSettingsDefault == false)
        {
            if (!settingsLoaded)
            {
                loadSettingsScript.ImportSettings();
                settingsLoaded = true;
            }
        }

        globalDatabaseLoaded = true;
    }
    public void PrepareAnimations()
    {
        passPageAnimator.Play();
        passPageAnimator.Stop();
        passPageAnimator.CurrentFrame = passPageAnimator.StartFrame;
        passPageEntity.SetActive(false);
        invertedPassPageAnimator.Play();
        invertedPassPageAnimator.Stop();
        invertedPassPageAnimator.CurrentFrame = invertedPassPageAnimator.StartFrame;
        invertedPassPageEntity.SetActive(false);
        closeBookAnimator.Play();
        closeBookAnimator.Stop();
        closeBookAnimator.CurrentFrame = closeBookAnimator.StartFrame;
        closeBookEntity.SetActive(false);
        quickStartAnimations = false;
    }
    public void Open()
    {
        uiManagerScript.SelectedElement = newGameEntity;
        entity.SetActive(true);
        uiManagerScript.BlockNavigation = true;
    }
};