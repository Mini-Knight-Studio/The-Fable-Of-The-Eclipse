using System;
using Loopie;

class Settings : Component
{
    // Display Mode
    public Entity displayModeHoveredEntity;
    private Image displayModeHoveredImage;
    private Image displayModeLeftArrowHoveredImage;
    private Image displayModeRightArrowHoveredImage;
    private Fullscreen displayModeScript;
    private Text displayModeDisplayText;

    // Framerate
    public Entity framerateHoveredEntity;
    private Image framerateHoveredImage;
    private Image framerateLeftArrowHoveredImage;
    private Image framerateRightArrowHoveredImage;
    private Framerate framerateScript;
    private Text framerateDisplayText;

    // V-Sync
    public Entity vSyncHoveredEntity;
    private Image vSyncHoveredImage;
    private Image vSyncLeftArrowHoveredImage;
    private Image vSyncRightArrowHoveredImage;
    private VSync vSyncScript;
    private Text vSyncDisplayText;

    // Master Volume
    public Entity masterVolumeHoveredEntity;
    private Image masterVolumeHoveredImage;
    private Image masterVolumeLeftArrowHoveredImage;
    private Image masterVolumeRightArrowHoveredImage;
    private MasterVolume masterVolumeScript;
    private Text masterVolumeDisplayText;

    // Music Volume
    public Entity musicVolumeHoveredEntity;
    private Image musicVolumeHoveredImage;
    private Image musicVolumeLeftArrowHoveredImage;
    private Image musicVolumeRightArrowHoveredImage;
    private MusicVolume musicVolumeScript;
    private Text musicVolumeDisplayText;

    public Entity sfxVolumeHoveredEntity;
    private Image sfxVolumeHoveredImage;
    private Image sfxVolumeLeftArrowHoveredImage;
    private Image sfxVolumeRightArrowHoveredImage;
    private SfxVolume sfxVolumeScript;
    private Text sfxVolumeDisplayText;

    private enum Buttons
    {
        DISPLAY_MODE,
        FRAMERATE,
        V_SYNC,
        MASTER_VOLUME,
        MUSIC_VOLUME,
        SFX_VOLUME
    }
    private Buttons currentButton = Buttons.DISPLAY_MODE;

    // Input
    public float inputCooldown = 0.2f;

    private float inputTimer = 0f;
    private float confirmTimer = 0f;
    private float applyTimer = 0f;

    private bool isEditingValue = false;

    // Audio
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    private bool loopMusicHasPlayed = false;

    void OnCreate()
    {
        // Display Mode
        if (displayModeHoveredEntity != null)
        {
            displayModeScript = displayModeHoveredEntity.GetComponent<Fullscreen>();
            displayModeHoveredImage = displayModeHoveredEntity.GetComponent<Image>();

            // Arrows
            Entity displayModeLeftArrowHoveredEntity = Entity.FindEntityByName("DisplayMode_LeftArrow_Hovered");
            if (displayModeLeftArrowHoveredEntity != null)
            {
                displayModeLeftArrowHoveredImage = displayModeLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no DisplayMode_LeftArrow_Hovered Image.");
            }
            Entity displayModeRightArrowHoveredEntity = Entity.FindEntityByName("DisplayMode_RightArrow_Hovered");
            if (displayModeRightArrowHoveredEntity != null)
            {
                displayModeRightArrowHoveredImage = displayModeRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no DisplayMode_RightArrow_Hovered Image.");
            }

            // Display
            Entity displayModeDisplayEntity = Entity.FindEntityByName("DisplayMode_Display");
            if (displayModeDisplayEntity != null)
            {
                displayModeDisplayText = displayModeDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no DisplayMode Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no DisplayMode Hovered Entity assigned.");
        }

        // Framerate
        if (framerateHoveredEntity != null)
        {
            framerateScript = framerateHoveredEntity.GetComponent<Framerate>();
            framerateHoveredImage = framerateHoveredEntity.GetComponent<Image>();

            // Arrows
            Entity framerateLeftArrowHoveredEntity = Entity.FindEntityByName("Framerate_LeftArrow_Hovered");
            if (framerateLeftArrowHoveredEntity != null)
            {
                framerateLeftArrowHoveredImage = framerateLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no Framerate_LeftArrow_Hovered Image.");
            }
            Entity framerateRightArrowHoveredEntity = Entity.FindEntityByName("Framerate_RightArrow_Hovered");
            if (framerateRightArrowHoveredEntity != null)
            {
                framerateRightArrowHoveredImage = framerateRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no Framerate_RightArrow_Hovered Image.");
            }

            // Display
            Entity framerateDisplayEntity = Entity.FindEntityByName("Framerate_Display");
            if (framerateDisplayEntity != null)
            {
                framerateDisplayText = framerateDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no Framerate Display Text.");
            }
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

            // Arrows
            Entity vSyncLeftArrowHoveredEntity = Entity.FindEntityByName("VSync_LeftArrow_Hovered");
            if (vSyncLeftArrowHoveredEntity != null)
            {
                vSyncLeftArrowHoveredImage = vSyncLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no VSync_LeftArrow_Hovered Image.");
            }
            Entity vSyncRightArrowHoveredEntity = Entity.FindEntityByName("VSync_RightArrow_Hovered");
            if (vSyncRightArrowHoveredEntity != null)
            {
                vSyncRightArrowHoveredImage = vSyncRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no VSync_RightArrow_Hovered Image.");
            }

            // Display
            Entity vSyncDisplayEntity = Entity.FindEntityByName("VSync_Display");
            if (vSyncDisplayEntity != null)
            {
                vSyncDisplayText = vSyncDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no VSync Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no VSync Hovered Entity assigned.");
        }

        if (masterVolumeHoveredEntity != null)
        {
            masterVolumeScript = masterVolumeHoveredEntity.GetComponent<MasterVolume>();
            masterVolumeHoveredImage = masterVolumeHoveredEntity.GetComponent<Image>();

            // Arrows
            Entity masterVolumeLeftArrowHoveredEntity = Entity.FindEntityByName("MasterVolume_LeftArrow_Hovered");
            if (masterVolumeLeftArrowHoveredEntity != null)
            {
                masterVolumeLeftArrowHoveredImage = masterVolumeLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no MasterVolume_LeftArrow_Hovered Image.");
            }
            Entity masterVolumeRightArrowHoveredEntity = Entity.FindEntityByName("MasterVolume_RightArrow_Hovered");
            if (masterVolumeRightArrowHoveredEntity != null)
            {
                masterVolumeRightArrowHoveredImage = masterVolumeRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no MasterVolume_RightArrow_Hovered Image.");
            }

            // Display
            Entity masterVolumeDisplayEntity = Entity.FindEntityByName("MasterVolume_Display");
            if (masterVolumeDisplayEntity != null)
            {
                masterVolumeDisplayText = masterVolumeDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no MasterVolume Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Master Volume Hovered Entity assigned.");
        }

        if (musicVolumeHoveredEntity != null)
        {
            musicVolumeScript = musicVolumeHoveredEntity.GetComponent<MusicVolume>();
            musicVolumeHoveredImage = musicVolumeHoveredEntity.GetComponent<Image>();

            // Arrows
            Entity musicVolumeLeftArrowHoveredEntity = Entity.FindEntityByName("MusicVolume_LeftArrow_Hovered");
            if (musicVolumeLeftArrowHoveredEntity != null)
            {
                musicVolumeLeftArrowHoveredImage = musicVolumeLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no MusicVolume_LeftArrow_Hovered Image.");
            }
            Entity musicVolumeRightArrowHoveredEntity = Entity.FindEntityByName("MusicVolume_RightArrow_Hovered");
            if (musicVolumeRightArrowHoveredEntity != null)
            {
                musicVolumeRightArrowHoveredImage = musicVolumeRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no MusicVolume_RightArrow_Hovered Image.");
            }

            // Display
            Entity musicVolumeDisplayEntity = Entity.FindEntityByName("MusicVolume_Display");
            if (musicVolumeDisplayEntity != null)
            {
                musicVolumeDisplayText = musicVolumeDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no MusicVolume Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Music Volume Hovered Entity assigned.");
        }

        if (sfxVolumeHoveredEntity != null)
        {
            sfxVolumeScript = sfxVolumeHoveredEntity.GetComponent<SfxVolume>();
            sfxVolumeHoveredImage = sfxVolumeHoveredEntity.GetComponent<Image>();

            // Arrows
            Entity sfxVolumeLeftArrowHoveredEntity = Entity.FindEntityByName("SfxVolume_LeftArrow_Hovered");
            if (sfxVolumeLeftArrowHoveredEntity != null)
            {
                sfxVolumeLeftArrowHoveredImage = sfxVolumeLeftArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no SfxVolume_LeftArrow_Hovered Image.");
            }
            Entity sfxVolumeRightArrowHoveredEntity = Entity.FindEntityByName("SfxVolume_RightArrow_Hovered");
            if (sfxVolumeRightArrowHoveredEntity != null)
            {
                sfxVolumeRightArrowHoveredImage = sfxVolumeRightArrowHoveredEntity.GetComponent<Image>();
            }
            else
            {
                Debug.Log("Error: There is no SfxVolume_RightArrow_Hovered Image.");
            }

            // Display
            Entity sfxVolumeDisplayEntity = Entity.FindEntityByName("SfxVolume_Display");
            if (sfxVolumeDisplayEntity != null)
            {
                sfxVolumeDisplayText = sfxVolumeDisplayEntity.GetComponent<Text>();
            }
            else
            {
                Debug.Log("Error: There is no SfxVolume Display Text.");
            }
        }
        else
        {
            Debug.Log("Error: There is no SfxVolume_Hovered Entity assigned.");
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
        HandleApplyChanges();
        HandleBack();
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
                case Buttons.DISPLAY_MODE: currentButton = Buttons.SFX_VOLUME; break;
                case Buttons.FRAMERATE: currentButton = Buttons.DISPLAY_MODE; break;
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
                case Buttons.DISPLAY_MODE: currentButton = Buttons.FRAMERATE; break;
                case Buttons.FRAMERATE: currentButton = Buttons.V_SYNC; break;
                case Buttons.V_SYNC: currentButton = Buttons.MASTER_VOLUME; break;
                case Buttons.MASTER_VOLUME: currentButton = Buttons.MUSIC_VOLUME; break;
                case Buttons.MUSIC_VOLUME: currentButton = Buttons.SFX_VOLUME; break;
                case Buttons.SFX_VOLUME: currentButton = Buttons.DISPLAY_MODE; break;
            }
            moved = true;
        }

        // Visual Feedback
        Vector4 inactiveColor = new Vector4(0.196f, 0.196f, 0.510f, 1.0f);
        Vector4 activeColor = new Vector4(0.706f, 0.000f, 0.216f, 1.0f);

        switch (currentButton)
        {
            case Buttons.DISPLAY_MODE:
                // Display
                displayModeHoveredEntity.SetActive(true);
                displayModeLeftArrowHoveredImage.SetActive(true);
                displayModeRightArrowHoveredImage.SetActive(true);
                displayModeDisplayText.SetColor(activeColor);
                // Framerate
                framerateHoveredEntity.SetActive(false);
                framerateLeftArrowHoveredImage.SetActive(false);
                framerateRightArrowHoveredImage.SetActive(false);
                framerateDisplayText.SetColor(inactiveColor);
                // VSync
                vSyncHoveredEntity.SetActive(false);
                vSyncLeftArrowHoveredImage.SetActive(false);
                vSyncRightArrowHoveredImage.SetActive(false);
                vSyncDisplayText.SetColor(inactiveColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(false);
                masterVolumeLeftArrowHoveredImage.SetActive(false);
                masterVolumeRightArrowHoveredImage.SetActive(false);
                masterVolumeDisplayText.SetColor(inactiveColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(false);
                musicVolumeLeftArrowHoveredImage.SetActive(false);
                musicVolumeRightArrowHoveredImage.SetActive(false);
                musicVolumeDisplayText.SetColor(inactiveColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(false);
                sfxVolumeLeftArrowHoveredImage.SetActive(false);
                sfxVolumeRightArrowHoveredImage.SetActive(false);
                sfxVolumeDisplayText.SetColor(inactiveColor);
                break;
            case Buttons.FRAMERATE:
                // Display
                displayModeHoveredEntity.SetActive(false);
                displayModeLeftArrowHoveredImage.SetActive(false);
                displayModeRightArrowHoveredImage.SetActive(false);
                displayModeDisplayText.SetColor(inactiveColor);
                // Framerate
                framerateHoveredEntity.SetActive(true);
                framerateLeftArrowHoveredImage.SetActive(true);
                framerateRightArrowHoveredImage.SetActive(true);
                framerateDisplayText.SetColor(activeColor);
                // VSync
                vSyncHoveredEntity.SetActive(false);
                vSyncLeftArrowHoveredImage.SetActive(false);
                vSyncRightArrowHoveredImage.SetActive(false);
                vSyncDisplayText.SetColor(inactiveColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(false);
                masterVolumeLeftArrowHoveredImage.SetActive(false);
                masterVolumeRightArrowHoveredImage.SetActive(false);
                masterVolumeDisplayText.SetColor(inactiveColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(false);
                musicVolumeLeftArrowHoveredImage.SetActive(false);
                musicVolumeRightArrowHoveredImage.SetActive(false);
                musicVolumeDisplayText.SetColor(inactiveColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(false);
                sfxVolumeLeftArrowHoveredImage.SetActive(false);
                sfxVolumeRightArrowHoveredImage.SetActive(false);
                sfxVolumeDisplayText.SetColor(inactiveColor);
                break;
            case Buttons.V_SYNC:
                // Display
                displayModeHoveredEntity.SetActive(false);
                displayModeLeftArrowHoveredImage.SetActive(false);
                displayModeRightArrowHoveredImage.SetActive(false);
                displayModeDisplayText.SetColor(inactiveColor);
                // Framerate
                framerateHoveredEntity.SetActive(false);
                framerateLeftArrowHoveredImage.SetActive(false);
                framerateRightArrowHoveredImage.SetActive(false);
                framerateDisplayText.SetColor(inactiveColor);
                // VSync
                vSyncHoveredEntity.SetActive(true);
                vSyncLeftArrowHoveredImage.SetActive(true);
                vSyncRightArrowHoveredImage.SetActive(true);
                vSyncDisplayText.SetColor(activeColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(false);
                masterVolumeLeftArrowHoveredImage.SetActive(false);
                masterVolumeRightArrowHoveredImage.SetActive(false);
                masterVolumeDisplayText.SetColor(inactiveColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(false);
                musicVolumeLeftArrowHoveredImage.SetActive(false);
                musicVolumeRightArrowHoveredImage.SetActive(false);
                musicVolumeDisplayText.SetColor(inactiveColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(false);
                sfxVolumeLeftArrowHoveredImage.SetActive(false);
                sfxVolumeRightArrowHoveredImage.SetActive(false);
                sfxVolumeDisplayText.SetColor(inactiveColor);
                break;
            case Buttons.MASTER_VOLUME:
                // Display
                displayModeHoveredEntity.SetActive(false);
                displayModeLeftArrowHoveredImage.SetActive(false);
                displayModeRightArrowHoveredImage.SetActive(false);
                displayModeDisplayText.SetColor(inactiveColor);
                // Framerate
                framerateHoveredEntity.SetActive(false);
                framerateLeftArrowHoveredImage.SetActive(false);
                framerateRightArrowHoveredImage.SetActive(false);
                framerateDisplayText.SetColor(inactiveColor);
                // VSync
                vSyncHoveredEntity.SetActive(false);
                vSyncLeftArrowHoveredImage.SetActive(false);
                vSyncRightArrowHoveredImage.SetActive(false);
                vSyncDisplayText.SetColor(inactiveColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(true);
                masterVolumeLeftArrowHoveredImage.SetActive(true);
                masterVolumeRightArrowHoveredImage.SetActive(true);
                masterVolumeDisplayText.SetColor(activeColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(false);
                musicVolumeLeftArrowHoveredImage.SetActive(false);
                musicVolumeRightArrowHoveredImage.SetActive(false);
                musicVolumeDisplayText.SetColor(inactiveColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(false);
                sfxVolumeLeftArrowHoveredImage.SetActive(false);
                sfxVolumeRightArrowHoveredImage.SetActive(false);
                sfxVolumeDisplayText.SetColor(inactiveColor);
                break;
            case Buttons.MUSIC_VOLUME:
                // Display
                displayModeHoveredEntity.SetActive(false);
                displayModeLeftArrowHoveredImage.SetActive(false);
                displayModeRightArrowHoveredImage.SetActive(false);
                displayModeDisplayText.SetColor(inactiveColor);
                // Framerate
                framerateHoveredEntity.SetActive(false);
                framerateLeftArrowHoveredImage.SetActive(false);
                framerateRightArrowHoveredImage.SetActive(false);
                framerateDisplayText.SetColor(inactiveColor);
                // VSync
                vSyncHoveredEntity.SetActive(false);
                vSyncLeftArrowHoveredImage.SetActive(false);
                vSyncRightArrowHoveredImage.SetActive(false);
                vSyncDisplayText.SetColor(inactiveColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(false);
                masterVolumeLeftArrowHoveredImage.SetActive(false);
                masterVolumeRightArrowHoveredImage.SetActive(false);
                masterVolumeDisplayText.SetColor(inactiveColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(true);
                musicVolumeLeftArrowHoveredImage.SetActive(true);
                musicVolumeRightArrowHoveredImage.SetActive(true);
                musicVolumeDisplayText.SetColor(activeColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(false);
                sfxVolumeLeftArrowHoveredImage.SetActive(false);
                sfxVolumeRightArrowHoveredImage.SetActive(false);
                sfxVolumeDisplayText.SetColor(inactiveColor);
                break;
            case Buttons.SFX_VOLUME:
                // Display
                displayModeHoveredEntity.SetActive(false);
                displayModeLeftArrowHoveredImage.SetActive(false);
                displayModeRightArrowHoveredImage.SetActive(false);
                displayModeDisplayText.SetColor(inactiveColor);
                // Framerate
                framerateHoveredEntity.SetActive(false);
                framerateLeftArrowHoveredImage.SetActive(false);
                framerateRightArrowHoveredImage.SetActive(false);
                framerateDisplayText.SetColor(inactiveColor);
                // VSync
                vSyncHoveredEntity.SetActive(false);
                vSyncLeftArrowHoveredImage.SetActive(false);
                vSyncRightArrowHoveredImage.SetActive(false);
                vSyncDisplayText.SetColor(inactiveColor);
                // Master Volume
                masterVolumeHoveredEntity.SetActive(false);
                masterVolumeLeftArrowHoveredImage.SetActive(false);
                masterVolumeRightArrowHoveredImage.SetActive(false);
                masterVolumeDisplayText.SetColor(inactiveColor);
                // Music Volume
                musicVolumeHoveredEntity.SetActive(false);
                musicVolumeLeftArrowHoveredImage.SetActive(false);
                musicVolumeRightArrowHoveredImage.SetActive(false);
                musicVolumeDisplayText.SetColor(inactiveColor);
                // Sfx Volume
                sfxVolumeHoveredEntity.SetActive(true);
                sfxVolumeLeftArrowHoveredImage.SetActive(true);
                sfxVolumeRightArrowHoveredImage.SetActive(true);
                sfxVolumeDisplayText.SetColor(activeColor);
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
            case Buttons.DISPLAY_MODE:
                displayModeScript.ToggleFullscreen();
                if (displayModeScript.IsFullscreen()) { displayModeDisplayText.SetText("Fullscreen"); }
                else { displayModeDisplayText.SetText("Windowed"); }
                break;
            case Buttons.FRAMERATE:
                framerateScript.IncreaseFramerate();
                framerateDisplayText.SetText(framerateScript.GetFramerate().ToString());
                break;
            case Buttons.V_SYNC:
                vSyncScript.ToggleVSync();
                if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
                else { vSyncDisplayText.SetText("Off"); }
                break;
            case Buttons.MASTER_VOLUME:
                masterVolumeScript.IncreaseVolume();
                float masterVolume = Mathf.Abs(masterVolumeScript.GetVolume() * 100);
                masterVolumeDisplayText.SetText(Mathf.Round(masterVolume).ToString());
                break;
            case Buttons.MUSIC_VOLUME:
                musicVolumeScript.IncreaseVolume();
                float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
                musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
                break;
            case Buttons.SFX_VOLUME:
                sfxVolumeScript.IncreaseVolume();
                float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
                sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
                break;
        }
    }

    void ChangeValueLeft()
    {
        switch (currentButton)
        {
            case Buttons.DISPLAY_MODE:
                displayModeScript.ToggleFullscreen();
                if (displayModeScript.IsFullscreen()) { displayModeDisplayText.SetText("Fullscreen"); }
                else { displayModeDisplayText.SetText("Windowed"); }
                break;
            case Buttons.FRAMERATE:
                framerateScript.DecreaseFramerate();
                framerateDisplayText.SetText(framerateScript.GetFramerate().ToString());
                break;
            case Buttons.V_SYNC:
                vSyncScript.ToggleVSync();
                if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
                else { vSyncDisplayText.SetText("Off"); }
                break;
            case Buttons.MASTER_VOLUME:
                masterVolumeScript.DecreaseVolume();
                float masterVolume = masterVolumeScript.GetVolume() * 100;
                masterVolumeDisplayText.SetText(Mathf.Round(masterVolume).ToString());
                break;
            case Buttons.MUSIC_VOLUME:
                musicVolumeScript.DecreaseVolume();
                float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
                musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
                break;
            case Buttons.SFX_VOLUME:
                sfxVolumeScript.DecreaseVolume();
                float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
                sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
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

    void HandleApplyChanges()
    {
        applyTimer += Time.deltaTime;

        // Cooldown
        if (applyTimer < inputCooldown)
            return;

        if (Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
        {
            displayModeScript.ApplyFullscreen();
            framerateScript.ApplyFramerate();
            vSyncScript.ApplyVSync();
            // Apply Master Volume
            // Apply Music Volume
            // Apply Sfx

            applyTimer = 0f;
        }
    }
    void HandleBack()
    {
        if (Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_B))
        {
            SceneManager.LoadSceneByID("db1dd4f7-fb12-b501-b8a7-ac788f03b8ae");
        }
    }
};