using System;
using Loopie;

class Settings : Component
{
    public Entity fullscreenHoveredEntity;
    private Image fullscreenHoveredImage;
    private Fullscreen fullscreenScript;
    private Text fullscreenDisplayText;

    public Entity framerateHoveredEntity;
    private Image framerateHoveredImage;
    private Framerate framerateScript;

    public Entity vSyncHoveredEntity;
    private Image vSyncHoveredImage;
    private VSync vSyncScript;
    private Text vSyncDisplayText;

    public Entity masterVolumeHoveredEntity;
    private Image masterVolumeHoveredImage;
    private MasterVolume masterVolumeScript;

    public Entity musicVolumeHoveredEntity;
    private Image musicVolumeHoveredImage;
    private MusicVolume musicVolumeScript;

    public Entity sfxVolumeHoveredEntity;
    private Image sfxVolumeHoveredImage;
    private SfxVolume sfxVolumeScript;

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

    private bool isEditingValue = false;

    // Audio
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    private bool loopMusicHasPlayed = false;

    void OnCreate()
    {
        // Fullscreen
        if (fullscreenHoveredEntity != null)
        {
            fullscreenScript = fullscreenHoveredEntity.GetComponent<Fullscreen>();
            fullscreenHoveredImage = fullscreenHoveredEntity.GetComponent<Image>();

            Entity displayFullscreenEntity = Entity.FindEntityByName("Fullscreen_Display");
            if (displayFullscreenEntity != null)
            {
                fullscreenDisplayText = displayFullscreenEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no Fullscreen Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Fullscreen Hovered Entity assigned.");
        }

        if (framerateHoveredEntity != null)
        {
            framerateScript = framerateHoveredEntity.GetComponent<Framerate>();
            framerateHoveredImage = framerateHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Framerate Hovered Entity assigned.");
        }

        // V-Sync
        if (vSyncHoveredEntity != null)
        {
            vSyncScript = vSyncHoveredEntity.GetComponent<VSync>();
            vSyncHoveredImage = vSyncHoveredEntity.GetComponent<Image>();

            Entity displayVSyncEntity = Entity.FindEntityByName("VSync_Display");
            if (displayVSyncEntity != null)
            {
                vSyncDisplayText = displayVSyncEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no Fullscreen Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no V-Sync Hovered Entity assigned.");
        }

        if (masterVolumeHoveredEntity != null)
        {
            masterVolumeScript = masterVolumeHoveredEntity.GetComponent<MasterVolume>();
            masterVolumeHoveredImage = masterVolumeHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Master Volume Hovered Entity assigned.");
        }

        if (musicVolumeHoveredEntity != null)
        {
            musicVolumeScript = musicVolumeHoveredEntity.GetComponent<MusicVolume>();
            musicVolumeHoveredImage = musicVolumeHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Music Volume Hovered Entity assigned.");
        }

        if (sfxVolumeHoveredEntity != null)
        {
            sfxVolumeScript = sfxVolumeHoveredEntity.GetComponent<SfxVolume>();
            sfxVolumeHoveredImage = sfxVolumeHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no SFX Volume Hovered Entity assigned.");
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
        HandleNavigation();
        HandleChangeValue();
    }

    void HandleNavigation()
    {
        inputTimer += Time.deltaTime;

        // Cooldown
        if (inputTimer < inputCooldown)
            return;
        if (isEditingValue)
            return;

        bool moved = false;

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
        switch (currentButton)
        {
            case Buttons.FULLSCREEN:
                fullscreenHoveredEntity.SetActive(true);
                framerateHoveredEntity.SetActive(false);
                vSyncHoveredEntity.SetActive(false);
                masterVolumeHoveredEntity.SetActive(false);
                musicVolumeHoveredEntity.SetActive(false);
                sfxVolumeHoveredEntity.SetActive(false);
                break;
            case Buttons.FRAMERATE:
                fullscreenHoveredEntity.SetActive(false);
                framerateHoveredEntity.SetActive(true);
                vSyncHoveredEntity.SetActive(false);
                masterVolumeHoveredEntity.SetActive(false);
                musicVolumeHoveredEntity.SetActive(false);
                sfxVolumeHoveredEntity.SetActive(false);
                break;
            case Buttons.V_SYNC:
                fullscreenHoveredEntity.SetActive(false);
                framerateHoveredEntity.SetActive(false);
                vSyncHoveredEntity.SetActive(true);
                masterVolumeHoveredEntity.SetActive(false);
                musicVolumeHoveredEntity.SetActive(false);
                sfxVolumeHoveredEntity.SetActive(false);
                break;
            case Buttons.MASTER_VOLUME:
                fullscreenHoveredEntity.SetActive(false);
                framerateHoveredEntity.SetActive(false);
                vSyncHoveredEntity.SetActive(false);
                masterVolumeHoveredEntity.SetActive(true);
                musicVolumeHoveredEntity.SetActive(false);
                sfxVolumeHoveredEntity.SetActive(false);
                break;
            case Buttons.MUSIC_VOLUME:
                fullscreenHoveredEntity.SetActive(false);
                framerateHoveredEntity.SetActive(false);
                vSyncHoveredEntity.SetActive(false);
                masterVolumeHoveredEntity.SetActive(false);
                musicVolumeHoveredEntity.SetActive(true);
                sfxVolumeHoveredEntity.SetActive(false);
                break;
            case Buttons.SFX_VOLUME:
                fullscreenHoveredEntity.SetActive(false);
                framerateHoveredEntity.SetActive(false);
                vSyncHoveredEntity.SetActive(false);
                masterVolumeHoveredEntity.SetActive(false);
                musicVolumeHoveredEntity.SetActive(false);
                sfxVolumeHoveredEntity.SetActive(true);
                break;
        }

        if (moved)
        {
            inputTimer = 0f;
        }
    }

    void HandleChangeValue()
    {
        confirmTimer += Time.deltaTime;

        float deadzone = 0.3f;
        float strongDeadzone = 0.7f;

        // Enter editing mode only on strong push
        if (!isEditingValue)
        {
            if (Input.LeftAxis.x > strongDeadzone || Input.IsKeyPressed(KeyCode.RIGHT) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT))
            {
                if (confirmTimer > inputCooldown)
                {
                    isEditingValue = true;
                    ChangeValueRight();
                    confirmTimer = 0f;
                }
            }
            else if (Input.LeftAxis.x < -strongDeadzone || Input.IsKeyPressed(KeyCode.LEFT) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT))
            {
                if (confirmTimer > inputCooldown)
                {
                    isEditingValue = true;
                    ChangeValueLeft();
                    confirmTimer = 0f;
                }
            }
        }
        else
        {
            // Exit editing mode when stick returns to neutral
            if (Mathf.Abs(Input.LeftAxis.x) < deadzone)
            {
                isEditingValue = false;
            }
        }
    }

    void ChangeValueRight()
    {
        switch (currentButton)
        {
            case Buttons.FULLSCREEN:
                fullscreenScript.ToggleFullscreen();
                fullscreenScript.ApplyFullscreen();
                if (fullscreenScript.IsFullscreen()) { fullscreenDisplayText.SetText("On"); }
                else { fullscreenDisplayText.SetText("Off"); }
                break;
            case Buttons.FRAMERATE:
                // increase FPS
                break;
            case Buttons.V_SYNC:
                vSyncScript.ToggleVSync();
                vSyncScript.ApplyVSync();
                if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
                else { vSyncDisplayText.SetText("Off"); }
                break;
            case Buttons.MASTER_VOLUME:
                // increase volume
                break;
        }
    }

    void ChangeValueLeft()
    {
        switch (currentButton)
        {
            case Buttons.FULLSCREEN:
                fullscreenScript.ToggleFullscreen();
                fullscreenScript.ApplyFullscreen();
                if (fullscreenScript.IsFullscreen()) { fullscreenDisplayText.SetText("On"); }
                else { fullscreenDisplayText.SetText("Off"); }
                break;
            case Buttons.FRAMERATE:
                // decrease FPS
                break;
            case Buttons.V_SYNC:
                vSyncScript.ToggleVSync();
                vSyncScript.ApplyVSync();
                if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
                else { vSyncDisplayText.SetText("Off"); }
                break;
            case Buttons.MASTER_VOLUME:
                // decrease volume
                break;
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