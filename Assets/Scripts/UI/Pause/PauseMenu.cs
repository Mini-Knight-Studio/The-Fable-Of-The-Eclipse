using System;
using Loopie;

public class PauseMenu : Component
{
    // Buttons
    [Header("Buttons")]
    public Entity continueEntity;
    public Entity continueHoveredEntity;
    private Button continueButton;
    private Load continueScript;
    private Image continueHoveredImage;

    public Entity mainMenuEntity;
    public Entity mainMenuHoveredEntity;
    private Button mainMenuButton;
    private SceneTransition mainMenuScript;
    private Image mainMenuHoveredImage;

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
        CONTINUE,
        MAIN_MENU,
        SETTINGS,
        EXIT
    }
    private Buttons currentButton = Buttons.CONTINUE;
    private Buttons? mouseResult;
    private Buttons keyboardResult = Buttons.CONTINUE;

    private enum InputMode
    {
        MOUSE,
        KEYBOARD
    }
    private InputMode currentInputMode = InputMode.KEYBOARD;

    private Vector2 lastMousePosition;

    // Audio
    [Header("Audio")]
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    public Entity selectSfxEntity;
    private AudioSource selectSfxAudioSource;

    private bool loopMusicHasPlayed = false;
    private float openingMusicDelay = 2f;
    private float openingMusicTimer = 0f;

    [Header("Debug")]
    public Entity ilustrationEntity;
    [HideInInspector]
    public Image ilustrationImage;
    [HideInInspector]
    public Image backgroundImage;

    [Header("Others")]
    // Input
    public float inputCooldown = 0.2f;
    [HideInInspector]
    public float enterCooldown = 0.1f;

    private float inputTimer = 0f;
    private float confirmTimer = 0f;
    private float mouseTimer = 0f;
    private float enterTimer = 0f;

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
    public static bool invertedPassPagePlayed = true;

    void OnCreate()
    {
        backgroundImage = entity.GetComponent<Image>();

        // Buttons
        if (mainMenuEntity != null)
        {
            mainMenuButton = mainMenuEntity.GetComponent<Button>();

            if (mainMenuHoveredEntity != null)
            {
                mainMenuScript = mainMenuHoveredEntity.GetComponent<SceneTransition>();
                mainMenuHoveredImage = mainMenuHoveredEntity.GetComponent<Image>();
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

        if (ilustrationEntity != null)
        {
            ilustrationImage = ilustrationEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Ilustration Entity assigned.");
        }
    }
    void OnUpdate()
    {
        enterTimer += Time.deltaTime;
        if (quickStartAnimations)
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
            enterTimer = 0f;
        }

        HandleMusic();

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

        if (continueButton.Hovered)
            hovered = Buttons.CONTINUE;
        else if (mainMenuButton.Hovered)
            hovered = Buttons.MAIN_MENU;
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
                case Buttons.CONTINUE:
                    continueHoveredImage.SetTint(color);
                    passPageEntity.SetActive(true);
                    passPageAnimator.Play();
                    break;
                case Buttons.MAIN_MENU:
                    mainMenuHoveredImage.SetTint(color);
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
            if (closeBookAnimator.CurrentFrame == closeBookAnimator.FrameCount - 1)
            {
                loopMusicAudioSource.Stop();
                canCallScripts = false;
                exitScript.ExitGame();
            }

            if (passPageAnimator.CurrentFrame == passPageAnimator.FrameCount - 1)
            {
                loopMusicAudioSource.Stop();
                quickStartAnimations = false;
                invertedPassPagePlayed = false;
                canCallScripts = false;
                MainMenu.quickStartAnimations = true;
                MainMenu.invertedPassPagePlayed = false;
                Settings.quickStartAnimations = true;
                Settings.invertedPassPagePlayed = false;
                Pause.isPaused = false;
                switch (currentButton)
                {
                    case Buttons.CONTINUE: break;
                    case Buttons.MAIN_MENU: mainMenuScript.StartTransition(); break;
                    case Buttons.SETTINGS: settingsScript.StartTransition(); break;
                    case Buttons.EXIT: break;
                }
            }
        }
    }

    void HandleVisualFeedback()
    {
        continueHoveredEntity.SetActive(currentButton == Buttons.CONTINUE);
        mainMenuHoveredEntity.SetActive(currentButton == Buttons.MAIN_MENU);
        settingsHoveredEntity.SetActive(currentButton == Buttons.SETTINGS);
        exitHoveredEntity.SetActive(currentButton == Buttons.EXIT);
    }

    void HandleMusic()
    {
        if (loopMusicHasPlayed)
            return;

        loopMusicAudioSource.Play();
        loopMusicHasPlayed = true;
    }

    public void HandleClickConfirm()
    {
        Vector4 color = new Vector4(255, 0, 0, 1);
        switch (currentButton)
        {
            case Buttons.CONTINUE:
                continueHoveredImage.SetTint(color);
                passPageEntity.SetActive(true);
                passPageAnimator.Play();
                break;
            case Buttons.MAIN_MENU:
                mainMenuHoveredImage.SetTint(color);
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