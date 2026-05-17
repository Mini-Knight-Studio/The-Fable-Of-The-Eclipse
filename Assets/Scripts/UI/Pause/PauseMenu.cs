using System;
using Loopie;

public class PauseMenu : Component
{
    // Buttons
    [Header("Buttons")]
    public Entity continueEntity;
    private Button continueButton;
    private Load continueScript;

    public Entity mainMenuEntity;
    private Button mainMenuButton;
    private SceneTransition mainMenuScript;

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

    [Header("Ilustration")]
    public Entity ilustrationLevel1Entity;
    [HideInInspector]
    public Image ilustrationLevel1Image;

    public Entity ilustrationWaterPathEntity;
    [HideInInspector]
    public Image ilustrationWaterPathImage;

    public string UUIDLevel1 = "";
    public string UUIDWaterPath = "";

    [HideInInspector]
    public Image backgroundImage;

    [Header("Others")]
    public Entity uiManagerEntity;
    private UIManager uiManagerScript;

    private bool canCallScripts = false;

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
    public static bool invertedPassPagePlayed = false;

    void OnCreate()
    {
        backgroundImage = entity.GetComponent<Image>();

        // Buttons
        if (mainMenuEntity != null)
        {
            mainMenuButton = mainMenuEntity.GetComponent<Button>();
            mainMenuScript = mainMenuEntity.GetComponent<SceneTransition>();
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

        if (ilustrationLevel1Entity != null)
        {
            ilustrationLevel1Image = ilustrationLevel1Entity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no ilustrationLevel1 Entity assigned.");
        }
        if (ilustrationWaterPathEntity != null)
        {
            ilustrationWaterPathImage = ilustrationWaterPathEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no ilustrationWaterPath Entity assigned.");
        }
    }
    void OnPostCreate()
    {
        if (DatabaseRegistry.playerDB.Player.currentSceneUUID == UUIDLevel1)
        {
            ilustrationLevel1Entity.SetActive(true);
            ilustrationWaterPathEntity.SetActive(false);
        }
        else if (DatabaseRegistry.playerDB.Player.currentSceneUUID == UUIDWaterPath)
        {
            ilustrationLevel1Entity.SetActive(false);
            ilustrationWaterPathEntity.SetActive(true);
        }
        else
        {
            ilustrationLevel1Entity.SetActive(true);
            ilustrationWaterPathEntity.SetActive(false);
        }
    }
    void OnUpdate()
    {
        if (quickStartAnimations)
        {
            PrepareAnimations();
        }

        //// Audio
        //if (!loopMusicHasPlayed)
        //{
        //    //loopMusicAudioSource.Play();
        //    loopMusicHasPlayed = true;
        //}

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
        
        if (canCallScripts)
        {
            HandleConfirm();
        }
    }
    public void Open()
    {
        uiManagerScript.SelectedElement = continueEntity;
        entity.SetActive(true);
    }
    void HandleConfirm()
    {
        if (closeBookAnimator.CurrentFrame == closeBookAnimator.FrameCount - 1)
        {
            exitScript.ExitGame();
        }

        if (passPageAnimator.CurrentFrame == passPageAnimator.FrameCount - 1)
        {
            //loopMusicAudioSource.Stop();
            quickStartAnimations = false;
            invertedPassPagePlayed = false;
            canCallScripts = false;
            MainMenu.quickStartAnimations = true;
            MainMenu.invertedPassPagePlayed = false;
            Settings.quickStartAnimations = true;
            Settings.invertedPassPagePlayed = false;

            Pause.Instance.TogglePauseMenu();

            if (continueButton.Hovered)
            {

            }
            else if (mainMenuButton.Hovered)
            {
                mainMenuScript.StartTransition();
            }
            else if (settingsButton.Hovered)
            {
                settingsScript.StartTransition();
            }
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
};