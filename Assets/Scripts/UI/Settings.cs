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

    private bool canCallScriptsRight = false;
    private bool canCallScriptsLeft = false;

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

        if (vSyncHoveredEntity != null)
        {
            //vSyncScript = framerateHoveredEntity.GetComponent<Framerate>();
            vSyncHoveredImage = vSyncHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no V-Sync Hovered Entity assigned.");
        }

        if (masterVolumeHoveredEntity != null)
        {
            //masterVolumeScript = masterVolumeHoveredEntity.GetComponent<MasterVolume>();
            masterVolumeHoveredImage = masterVolumeHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Master Volume Hovered Entity assigned.");
        }

        if (musicVolumeHoveredEntity != null)
        {
            //musicVolumeScript = musicVolumeHoveredEntity.GetComponent<MusicVolume>();
            musicVolumeHoveredImage = musicVolumeHoveredEntity.GetComponent<Image>();
        }
        else
        {
            Debug.Log("Error: There is no Music Volume Hovered Entity assigned.");
        }

        if (sfxVolumeHoveredEntity != null)
        {
            //sfxVolumeScript = sfxVolumeHoveredEntity.GetComponent<SfxVolume>();
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
        if (canCallScripts)
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

        // Read Input
        if (Input.IsKeyPressed(KeyCode.RIGHT) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT) || Input.LeftAxis.x > 0)
        {
            canCallScriptsRight = true;
            confirmTimer = 0f;
        }
        else if (Input.IsKeyPressed(KeyCode.LEFT) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT) || Input.LeftAxis.x < 0)
        {
            canCallScriptsLeft = true;
            confirmTimer = 0f;
        }

        // Function Call
        if (confirmTimer > inputCooldown && canCallScriptsRight)
        {
            switch (currentButton)
            {
                case Buttons.FULLSCREEN:
                    //
                    break;
                case Buttons.FRAMERATE: currentButton = Buttons.V_SYNC; break;
                case Buttons.V_SYNC: currentButton = Buttons.MASTER_VOLUME; break;
                case Buttons.MASTER_VOLUME: currentButton = Buttons.MUSIC_VOLUME; break;
                case Buttons.MUSIC_VOLUME: currentButton = Buttons.SFX_VOLUME; break;
                case Buttons.SFX_VOLUME: currentButton = Buttons.FULLSCREEN; break;
            }

            canCallScriptsRight = false;
        }
        else if (confirmTimer > inputCooldown && canCallScriptsLeft)
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
            canCallScriptsLeft = false;
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