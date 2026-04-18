using System;
using Loopie;

class MainMenu : Component
{
    // Buttons
    public Entity newGameEntity;
    public Entity newGameHoveredEntity;
    private Button newGameButton;
    private NewGame newGameScript;
    private Image newGameHoveredImage;

    public Entity continueEntity;
    public Entity continueHoveredEntity;
    private Button continueButton;
    private Load continueScript;
    private Image continueHoveredImage;

    public Entity settingsEntity;
    public Entity settingsHoveredEntity;
    private Button settingsButton;
    private SceneTransition settingsScript;
    private Image settingsHoveredImage;

    public Entity exitEntity;
    public Entity exitHoveredEntity;
    private Button exitButton;
    private Exit exitScript;
    private Image exitHoveredImage;

    private enum Buttons
    {
        NEW_GAME,
        CONTINUE,
        SETTINGS,
        EXIT
    }
    private Buttons currentButton = Buttons.NEW_GAME;
    private Buttons? mouseResult;
    private Buttons keyboardResult = Buttons.NEW_GAME;

    private enum InputMode
    {
        MOUSE,
        KEYBOARD
    }
    private InputMode currentInputMode = InputMode.KEYBOARD;

    private Vector2 lastMousePosition;

    // Intro Book Cover
    public Entity introBookCoverEntity;
    private IntroBookCover introBookCoverScript;

    private float preMainMenuDelay = 0f;
    private float preMainMenuDelayTimer = 0f;

    // Input
    public float inputCooldown = 0.2f;

    private float inputTimer = 0f;
    private float confirmTimer = 0f;
    private float mouseTimer = 0f;

    private bool canCallScripts = false;

    // Audio
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    public Entity selectSfxEntity;
    private AudioSource selectSfxAudioSource;

    private bool loopMusicHasPlayed = false;
    private float openingMusicDelay = 21f;
    private float openingMusicTimer = 0f;

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

    public static bool quickStartAnimations = true;
    public static bool hasPlayedIntro = false;

    void OnCreate()
    {
        // Buttons
        if (newGameEntity != null)
        {
            newGameButton = newGameEntity.GetComponent<Button>();

            if (newGameHoveredEntity != null)
            {
                newGameScript = newGameHoveredEntity.GetComponent<NewGame>();
                newGameHoveredImage = newGameHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no NewGame Hovered Entity assigned.");
            }
        }
        else
        {
            Debug.Log("Error: There is no NewGame Entity assigned.");
        }

        if (continueEntity != null)
        {
            continueButton = continueEntity.GetComponent<Button>();

            if (continueHoveredEntity != null)
            {
                continueScript = continueHoveredEntity.GetComponent<Load>();
                continueHoveredImage = continueHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no Continue Hovered Entity assigned.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Continue Entity assigned.");
        }

        if (settingsEntity != null)
        {
            settingsButton = settingsEntity.GetComponent<Button>();

            if (settingsHoveredEntity != null)
            {
                settingsScript = settingsHoveredEntity.GetComponent<SceneTransition>();
                settingsHoveredImage = settingsHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no Settings Hovered Entity assigned.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Settings Entity assigned.");
        }

        if (exitEntity != null)
        {
            exitButton = exitEntity.GetComponent<Button>();

            if (exitHoveredEntity != null)
            {
                exitScript = exitHoveredEntity.GetComponent<Exit>();
                exitHoveredImage = exitHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no Exit Hovered Entity assigned.");
            }
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
    }
    void OnUpdate()
    {
        // On Start
        if (!globalDatabaseLoaded)
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

        if (quickStartAnimations)
        {
            passPageAnimator.Play();
            passPageAnimator.Stop();
            passPageEntity.SetActive(false);
            closeBookAnimator.Play();
            closeBookAnimator.Stop();
            closeBookEntity.SetActive(false);
            quickStartAnimations = false;
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
        }

        // Main Menu logic.
        Buttons previous = currentButton;

        HandleMouseNavigation();
        HandleNavigation();

        if (currentInputMode == InputMode.MOUSE)
        {
            if (mouseResult.HasValue)
                currentButton = mouseResult.Value;
        }
        else
        {
            currentButton = keyboardResult;
        }

        HandleVisualFeedback();
        HandleConfirm();
    }

    void HandleMouseNavigation()
    {
        Vector2 currentMouse = Input.MousePosition;
        Vector2 delta = currentMouse - lastMousePosition;

        float sqrDistance = delta.x * delta.x + delta.y * delta.y;

        lastMousePosition = currentMouse;

        Buttons? hovered = null;

        if (newGameButton.Hovered)
            hovered = Buttons.NEW_GAME;
        else if (continueButton.Hovered)
            hovered = Buttons.CONTINUE;
        else if (settingsButton.Hovered)
            hovered = Buttons.SETTINGS;
        else if (exitButton.Hovered)
            hovered = Buttons.EXIT;

        if (sqrDistance > 1.0f)
            currentInputMode = InputMode.MOUSE;

        if (currentInputMode != InputMode.MOUSE)
            return;

        mouseResult = hovered;
    }

    void HandleNavigation()
    {
        bool input = Input.IsKeyPressed(KeyCode.UP) || Input.IsKeyPressed(KeyCode.DOWN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN) || Input.LeftAxis.y != 0;

        if (input)
            currentInputMode = InputMode.KEYBOARD;

        if (currentInputMode != InputMode.KEYBOARD)
            return;

        inputTimer += Time.deltaTime;

        if (inputTimer < inputCooldown || canCallScripts)
            return;

        bool moved = false;

        if (Input.IsKeyPressed(KeyCode.UP) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP) || Input.LeftAxis.y > 0)
        {
            keyboardResult = (Buttons)(((int)keyboardResult + 3) % 4);
            moved = true;
        }
        else if (Input.IsKeyPressed(KeyCode.DOWN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN) || Input.LeftAxis.y < 0)
        {
            keyboardResult = (Buttons)(((int)keyboardResult + 1) % 4);
            moved = true;
        }

        if (moved)
        {
            selectSfxAudioSource.Play();
            inputTimer = 0f;
        }
    }

    void HandleConfirm()
    {
        confirmTimer += Time.deltaTime;

        // Read Input
        if ((Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A)) && !canCallScripts)
        {
            Vector4 color = new Vector4(255, 0, 0, 1);
            switch (currentButton)
            {
                case Buttons.NEW_GAME: 
                    newGameHoveredImage.SetTint(color);
                    passPageEntity.SetActive(true);
                    passPageAnimator.Play();
                    break;
                case Buttons.CONTINUE: 
                    continueHoveredImage.SetTint(color);
                    passPageEntity.SetActive(true);
                    passPageAnimator.Play();
                    break;
                case Buttons.SETTINGS: 
                    settingsHoveredImage.SetTint(color);
                    passPageEntity.SetActive(true);
                    passPageAnimator.Play();
                    break;
                case Buttons.EXIT: 
                    exitHoveredImage.SetTint(color);
                    closeBookEntity.SetActive(true);
                    closeBookAnimator.Play();
                    break;
            }

            canCallScripts = true;
            confirmTimer = 0f;
        }

        // Function Call
        if (confirmTimer > inputCooldown && canCallScripts)
        {
            if (closeBookAnimator.CurrentFrame == closeBookAnimator.FrameCount)
            {
                exitScript.ExitGame();
                loopMusicAudioSource.Stop();
                canCallScripts = false;
            }

            if (passPageAnimator.CurrentFrame == passPageAnimator.FrameCount)
            {
                switch (currentButton)
                {
                    case Buttons.NEW_GAME: newGameScript.StartNewGame(); break;
                    case Buttons.CONTINUE: continueScript.LoadPreviousSave(); break;
                    case Buttons.SETTINGS: settingsScript.StartTransition(); break;
                    case Buttons.EXIT: break;
                }
                loopMusicAudioSource.Stop();
                canCallScripts = false;
            }
        }
    }

    void HandleVisualFeedback()
    {
        newGameHoveredEntity.SetActive(currentButton == Buttons.NEW_GAME);
        continueHoveredEntity.SetActive(currentButton == Buttons.CONTINUE);
        settingsHoveredEntity.SetActive(currentButton == Buttons.SETTINGS);
        exitHoveredEntity.SetActive(currentButton == Buttons.EXIT);
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

    public void HandleClickConfirm()
    {
        Vector4 color = new Vector4(255, 0, 0, 1);
        switch (currentButton)
        {
            case Buttons.NEW_GAME:
                newGameHoveredImage.SetTint(color);
                passPageEntity.SetActive(true);
                passPageAnimator.Play();
                break;
            case Buttons.CONTINUE:
                continueHoveredImage.SetTint(color);
                passPageEntity.SetActive(true);
                passPageAnimator.Play();
                break;
            case Buttons.SETTINGS:
                settingsHoveredImage.SetTint(color);
                passPageEntity.SetActive(true);
                passPageAnimator.Play();
                break;
            case Buttons.EXIT:
                exitHoveredImage.SetTint(color);
                closeBookEntity.SetActive(true);
                closeBookAnimator.Play();
                break;
        }

        canCallScripts = true;
        confirmTimer = 0f;
    }
};