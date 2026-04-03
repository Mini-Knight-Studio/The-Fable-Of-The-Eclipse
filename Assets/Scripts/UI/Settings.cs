using System;
using Loopie;

class Settings : Component
{
    public Entity fullscreenHoveredEntity;
    private Image fullscreenHoveredImage;
    // Script

    public Entity framerateHoveredEntity;
    private Image framerateHoveredImage;
    // Script

    public Entity vSyncHoveredEntity;
    private Image vSyncHoveredImage;
    // Script

    public Entity masterVolumeHoveredEntity;
    private Image masterVolumeHoveredImage;
    // Script

    public Entity musicVolumeHoveredEntity;
    private Image musicVolumeHoveredImage;
    // Script

    public Entity sfxVolumeHoveredEntity;
    private Image sfxVolumeHoveredImage;
    // Script

    private enum Buttons
    {
        FULLSCREEN,
        FRAMERATE,
        V_SYNC,
        MASTER_VOLUME,
        MUSIC_VOLUME,
        SFX_VOLUME
    }
    private Buttons currentButton = Buttons.FULLSCREEN;

    // Input
    public float inputCooldown = 0.2f;

    private float inputTimer = 0f;
    private float confirmTimer = 0f;

    private bool canCallScripts = false;

    // Audio
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    private bool loopMusicHasPlayed = false;

    void OnCreate()
    {
        if (fullscreenHoveredEntity != null)
        {
            //fullscreenScript = fullscreenHoveredEntity.GetComponent<Fullscreen>();
            fullscreenHoveredImage = fullscreenHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Fullscreen Hovered Entity assigned.");
        }

        if (framerateHoveredEntity != null)
        {
            //framerateScript = framerateHoveredEntity.GetComponent<Framerate>();
            framerateHoveredImage = framerateHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Framerate Hovered Entity assigned.");
        }

        if (continueEntity != null)
        {
            // button

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
            // button

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
            // button

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
    }

    void OnUpdate()
    {
        HandleMusic();

        // Nullify Input while in intro.
        preMainMenuDelayTimer += Time.deltaTime;
        preMainMenuDelay = introBookCoverScript.GetTotalPreAnimationDelay() + introBookCoverScript.inAnimationDelay;

        if (preMainMenuDelayTimer < preMainMenuDelay)
            return;

        // Main Menu logic.
        HandleNavigation();
        HandleConfirm();
    }

    void HandleNavigation()
    {
        inputTimer += Time.deltaTime;

        // Cooldown
        if (inputTimer < inputCooldown)
            return;
        if (canCallScripts)
            return;

        bool moved = false;

        FULLSCREEN,
        FRAMERATE,
        V_SYNC,
        MASTER_VOLUME,
        MUSIC_VOLUME,
        SFX_VOLUME

        // Read Input
        if (Input.IsKeyPressed(KeyCode.UP) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP) || Input.LeftAxis.y > 0)
        {
            switch (currentButton)
            {
                case Buttons.FULLSCREEN: currentButton = Buttons.SFX_VOLUME; break;
                case Buttons.FRAMERATE: currentButton = Buttons.FULLSCREEN; break;
                case Buttons.V_SYNC: currentButton = Buttons.FRAMERATE; break;
                case Buttons.MASTER_VOLUME: currentButton = Buttons.V_SYNC; break;
                case Buttons.MUSIC_VOLUME: currentButton = Buttons.MASTER_VOLUME; break;
                case Buttons.SFX_VOLUME: currentButton = Buttons.MUSIC_VOLUME; break;
            }
            moved = true;
        }
        else if (Input.IsKeyPressed(KeyCode.DOWN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN) || Input.LeftAxis.y < 0)
        {
            switch (currentButton)
            {
                case Buttons.FULLSCREEN: currentButton = Buttons.FRAMERATE; break;
                case Buttons.FRAMERATE: currentButton = Buttons.V_SYNC; break;
                case Buttons.V_SYNC: currentButton = Buttons.MASTER_VOLUME; break;
                case Buttons.MASTER_VOLUME: currentButton = Buttons.MUSIC_VOLUME; break;
                case Buttons.MUSIC_VOLUME: currentButton = Buttons.SFX_VOLUME; break;
                case Buttons.SFX_VOLUME: currentButton = Buttons.FULLSCREEN; break;
            }
            moved = true;
        }

        // Visual Feedback
        //switch (currentButton)
        //{
        //    case Buttons.NEW_GAME:
        //        newGameHoveredEntity.SetActive(true);
        //        continueHoveredEntity.SetActive(false);
        //        settingsHoveredEntity.SetActive(false);
        //        exitHoveredEntity.SetActive(false);
        //        break;
        //    case Buttons.CONTINUE:
        //        newGameHoveredEntity.SetActive(false);
        //        continueHoveredEntity.SetActive(true);
        //        settingsHoveredEntity.SetActive(false);
        //        exitHoveredEntity.SetActive(false);
        //        break;
        //    case Buttons.SETTINGS:
        //        newGameHoveredEntity.SetActive(false);
        //        continueHoveredEntity.SetActive(false);
        //        settingsHoveredEntity.SetActive(true);
        //        exitHoveredEntity.SetActive(false);
        //        break;
        //    case Buttons.EXIT:
        //        newGameHoveredEntity.SetActive(false);
        //        continueHoveredEntity.SetActive(false);
        //        settingsHoveredEntity.SetActive(false);
        //        exitHoveredEntity.SetActive(true);
        //        break;
        //}

        if (moved)
        {
            inputTimer = 0f;
        }
    }

    void HandleConfirm()
    {
        confirmTimer += Time.deltaTime;

        // Read Input
        if (Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
        {
            // Visual Feedback
            Vector4 color = new Vector4(255, 0, 0, 1);
            switch (currentButton)
            {
                case Buttons.NEW_GAME: newGameHoveredImage.SetTint(color); break;
                case Buttons.CONTINUE: continueHoveredImage.SetTint(color); break;
                case Buttons.SETTINGS: settingsHoveredImage.SetTint(color); break;
                case Buttons.EXIT: exitHoveredImage.SetTint(color); break;
            }

            canCallScripts = true;
            confirmTimer = 0f;
        }

        if (confirmTimer > inputCooldown && canCallScripts)
        {
            // Function Call
            switch (currentButton)
            {
                case Buttons.NEW_GAME: newGameScript.StartTransition(); break;
                case Buttons.CONTINUE: continueScript.LoadPreviousSave(); break;
                case Buttons.SETTINGS: settingsScript.StartTransition(); break;
                case Buttons.EXIT: exitScript.ExitGame(); break;
            }

            loopMusicAudioSource.Stop();
        }
    }

    void HandleMusic()
    {
        if (loopMusicHasPlayed)
            return;

        loopMusicAudioSource.Play();
        loopMusicHasPlayed = true;
    }
};