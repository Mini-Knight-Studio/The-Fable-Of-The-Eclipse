using System;
using Loopie;

class Settings : Component
{
    [HideInInspector]
    public float enterCooldown = 0.1f;
    private float enterTimer = 0f;

    [HideInInspector]
    public static bool quickStartAnimations = true;
    [HideInInspector]
    public static bool invertedPassPagePlayed = false;

    public Entity uiManagerEntity;
    private UIManager uiManagerScript;

    [Header("Scene Transitions")]
    public Entity passPageEntity;
    private SpriteAnimator passPageAnimator;
    public Entity invertedPassPageEntity;
    private SpriteAnimator invertedPassPageAnimator;

    // Display Mode
    [Header("Buttons")]
    public Entity displayModeEntity;
    private Fullscreen displayModeScript;
    private Text displayModeDisplayText;
    private Button displayModeButton;
    private Button displayModeLeftArrowButton;
    private Button displayModeRightArrowButton;

    // Framerate
    public Entity framerateEntity;
    private Framerate framerateScript;
    private Text framerateDisplayText;
    private Button framerateButton;
    private Button framerateLeftArrowButton;
    private Button framerateRightArrowButton;

    // V-Sync 
    public Entity vSyncEntity;
    private VSync vSyncScript;
    private Text vSyncDisplayText;
    private Button vSyncButton;
    private Button vSyncLeftArrowButton;
    private Button vSyncRightArrowButton;

    // Master Volume  
    public Entity masterVolumeEntity;
    private MasterVolume masterVolumeScript;
    private Text masterVolumeDisplayText;
    private Button masterVolumeButton;
    private Button masterVolumeLeftArrowButton;
    private Button masterVolumeRightArrowButton;

    // Master Volume Containers
    private Entity masterVolumeContainer1Filled;
    private Entity masterVolumeContainer2Filled;
    private Entity masterVolumeContainer3Filled;
    private Entity masterVolumeContainer4Filled;
    private Entity masterVolumeContainer5Filled;
    private Entity masterVolumeContainer6Filled;
    private Entity masterVolumeContainer7Filled;
    private Entity masterVolumeContainer8Filled;
    private Entity masterVolumeContainer9Filled;
    private Entity masterVolumeContainer10Filled;

    // Music Volume 
    public Entity musicVolumeEntity;
    private MusicVolume musicVolumeScript;
    private Text musicVolumeDisplayText;
    private Button musicVolumeButton;
    private Button musicVolumeLeftArrowButton;
    private Button musicVolumeRightArrowButton;

    // Music Volume Containers
    private Entity musicVolumeContainer1Filled;
    private Entity musicVolumeContainer2Filled;
    private Entity musicVolumeContainer3Filled;
    private Entity musicVolumeContainer4Filled;
    private Entity musicVolumeContainer5Filled;
    private Entity musicVolumeContainer6Filled;
    private Entity musicVolumeContainer7Filled;
    private Entity musicVolumeContainer8Filled;
    private Entity musicVolumeContainer9Filled;
    private Entity musicVolumeContainer10Filled;

    // Sfx Volume
    public Entity sfxVolumeEntity;
    private SfxVolume sfxVolumeScript;
    private Text sfxVolumeDisplayText;
    private Button sfxVolumeButton;
    private Button sfxVolumeLeftArrowButton;
    private Button sfxVolumeRightArrowButton;

    // Sfx Volume Containers
    private Entity sfxVolumeContainer1Filled;
    private Entity sfxVolumeContainer2Filled;
    private Entity sfxVolumeContainer3Filled;
    private Entity sfxVolumeContainer4Filled;
    private Entity sfxVolumeContainer5Filled;
    private Entity sfxVolumeContainer6Filled;
    private Entity sfxVolumeContainer7Filled;
    private Entity sfxVolumeContainer8Filled;
    private Entity sfxVolumeContainer9Filled;
    private Entity sfxVolumeContainer10Filled;

    // Input
    [Header("Input")]
    public float inputCooldown = 0.2f;

    private float inputTimer = 0f;
    private float confirmTimer = 0f;
    private float applyTimer = 0f;

    private bool isEditingValue = false;

    // Audio
    [Header("Audio")]
    public Entity loopMusicEntity;
    private AudioSource loopMusicAudioSource;

    private bool loopMusicHasPlayed = false;

    void OnCreate()
    {
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

        // Display Mode
        if (displayModeEntity != null)
        {
            displayModeScript = displayModeEntity.GetComponent<Fullscreen>();
            displayModeButton = displayModeEntity.GetComponent<Button>();

            // Arrows
            Entity displayModeLeftArrowEntity = Entity.FindEntityByName("DisplayMode_LeftArrow");
            if (displayModeLeftArrowEntity != null)
            {
                displayModeLeftArrowButton = displayModeLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no DisplayMode_LeftArrow Button.");
            }
            Entity displayModeRightArrowEntity = Entity.FindEntityByName("DisplayMode_RightArrow");
            if (displayModeRightArrowEntity != null)
            {
                displayModeRightArrowButton = displayModeRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no DisplayMode_RightArrow Button.");
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
        if (framerateEntity != null)
        {
            framerateScript = framerateEntity.GetComponent<Framerate>();
            framerateButton = framerateEntity.GetComponent<Button>();

            // Arrows
            Entity framerateLeftArrowEntity = Entity.FindEntityByName("Framerate_LeftArrow");
            if (framerateLeftArrowEntity != null)
            {
                framerateLeftArrowButton = framerateLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no Framerate_LeftArrow Button.");
            }
            Entity framerateRightArrowEntity = Entity.FindEntityByName("Framerate_RightArrow");
            if (framerateRightArrowEntity != null)
            {
                framerateRightArrowButton = framerateRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no Framerate_RightArrow Button.");
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
        if (vSyncEntity != null)
        {
            vSyncScript = vSyncEntity.GetComponent<VSync>();
            vSyncButton = vSyncEntity.GetComponent<Button>();

            // Arrows
            Entity vSyncLeftArrowEntity = Entity.FindEntityByName("VSync_LeftArrow");
            if (vSyncLeftArrowEntity != null)
            {
                vSyncLeftArrowButton = vSyncLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no VSync_LeftArrow Button.");
            }
            Entity vSyncRightArrowEntity = Entity.FindEntityByName("VSync_RightArrow");
            if (vSyncRightArrowEntity != null)
            {
                vSyncRightArrowButton = vSyncRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no VSync_RightArrow Button.");
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

        // Master Volume
        if (masterVolumeEntity != null)
        {
            masterVolumeScript = masterVolumeEntity.GetComponent<MasterVolume>();
            masterVolumeButton = masterVolumeEntity.GetComponent<Button>();

            // Arrows
            Entity masterVolumeLeftArrowEntity = Entity.FindEntityByName("MasterVolume_LeftArrow");
            if (masterVolumeLeftArrowEntity != null)
            {
                masterVolumeLeftArrowButton = masterVolumeLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no MasterVolume_LeftArrow Button.");
            }
            Entity masterVolumeRightArrowEntity = Entity.FindEntityByName("MasterVolume_RightArrow");
            if (masterVolumeRightArrowEntity != null)
            {
                masterVolumeRightArrowButton = masterVolumeRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no MasterVolume_RightArrow Button.");
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

            // Containers
            masterVolumeContainer1Filled = Entity.FindEntityByName("MasterVolume_Container1_Filled");
            masterVolumeContainer2Filled = Entity.FindEntityByName("MasterVolume_Container2_Filled");
            masterVolumeContainer3Filled = Entity.FindEntityByName("MasterVolume_Container3_Filled");
            masterVolumeContainer4Filled = Entity.FindEntityByName("MasterVolume_Container4_Filled");
            masterVolumeContainer5Filled = Entity.FindEntityByName("MasterVolume_Container5_Filled");
            masterVolumeContainer6Filled = Entity.FindEntityByName("MasterVolume_Container6_Filled");
            masterVolumeContainer7Filled = Entity.FindEntityByName("MasterVolume_Container7_Filled");
            masterVolumeContainer8Filled = Entity.FindEntityByName("MasterVolume_Container8_Filled");
            masterVolumeContainer9Filled = Entity.FindEntityByName("MasterVolume_Container9_Filled");
            masterVolumeContainer10Filled = Entity.FindEntityByName("MasterVolume_Container10_Filled");
        }
        else
        {
            Debug.Log("Error: There is no Master Volume Hovered Entity assigned.");
        }

        // Music Volume
        if (musicVolumeEntity != null)
        {
            musicVolumeScript = musicVolumeEntity.GetComponent<MusicVolume>();
            musicVolumeButton = musicVolumeEntity.GetComponent<Button>();

            // Arrows
            Entity musicVolumeLeftArrowEntity = Entity.FindEntityByName("MusicVolume_LeftArrow");
            if (musicVolumeLeftArrowEntity != null)
            {
                musicVolumeLeftArrowButton = musicVolumeLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no MusicVolume_LeftArrow Button.");
            }
            Entity musicVolumeRightArrowEntity = Entity.FindEntityByName("MusicVolume_RightArrow");
            if (musicVolumeRightArrowEntity != null)
            {
                musicVolumeRightArrowButton = musicVolumeRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no MusicVolume_RightArrow Button.");
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

            // Containers
            musicVolumeContainer1Filled = Entity.FindEntityByName("MusicVolume_Container1_Filled");
            musicVolumeContainer2Filled = Entity.FindEntityByName("MusicVolume_Container2_Filled");
            musicVolumeContainer3Filled = Entity.FindEntityByName("MusicVolume_Container3_Filled");
            musicVolumeContainer4Filled = Entity.FindEntityByName("MusicVolume_Container4_Filled");
            musicVolumeContainer5Filled = Entity.FindEntityByName("MusicVolume_Container5_Filled");
            musicVolumeContainer6Filled = Entity.FindEntityByName("MusicVolume_Container6_Filled");
            musicVolumeContainer7Filled = Entity.FindEntityByName("MusicVolume_Container7_Filled"); 
            musicVolumeContainer8Filled = Entity.FindEntityByName("MusicVolume_Container8_Filled");
            musicVolumeContainer9Filled = Entity.FindEntityByName("MusicVolume_Container9_Filled");
            musicVolumeContainer10Filled = Entity.FindEntityByName("MusicVolume_Container10_Filled");
        }
        else
        {
            Debug.Log("Error: There is no Music Volume Hovered Entity assigned.");
        }

        // Sfx Volume
        if (sfxVolumeEntity != null)
        {
            sfxVolumeScript = sfxVolumeEntity.GetComponent<SfxVolume>();
            sfxVolumeButton = sfxVolumeEntity.GetComponent<Button>();

            // Arrows
            Entity sfxVolumeLeftArrowEntity = Entity.FindEntityByName("SfxVolume_LeftArrow");
            if (sfxVolumeLeftArrowEntity != null)
            {
                sfxVolumeLeftArrowButton = sfxVolumeLeftArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no SfxVolume_LeftArrow Button.");
            }
            Entity sfxVolumeRightArrowEntity = Entity.FindEntityByName("SfxVolume_RightArrow");
            if (sfxVolumeRightArrowEntity != null)
            {
                sfxVolumeRightArrowButton = sfxVolumeRightArrowEntity.GetComponent<Button>();
            }
            else
            {
                Debug.Log("Error: There is no SfxVolume_RightArrow Button.");
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

            // Containers
            sfxVolumeContainer1Filled = Entity.FindEntityByName("SfxVolume_Container1_Filled");
            sfxVolumeContainer2Filled = Entity.FindEntityByName("SfxVolume_Container2_Filled");
            sfxVolumeContainer3Filled = Entity.FindEntityByName("SfxVolume_Container3_Filled");
            sfxVolumeContainer4Filled = Entity.FindEntityByName("SfxVolume_Container4_Filled");
            sfxVolumeContainer5Filled = Entity.FindEntityByName("SfxVolume_Container5_Filled");
            sfxVolumeContainer6Filled = Entity.FindEntityByName("SfxVolume_Container6_Filled");
            sfxVolumeContainer7Filled = Entity.FindEntityByName("SfxVolume_Container7_Filled");
            sfxVolumeContainer8Filled = Entity.FindEntityByName("SfxVolume_Container8_Filled");
            sfxVolumeContainer9Filled = Entity.FindEntityByName("SfxVolume_Container9_Filled");
            sfxVolumeContainer10Filled = Entity.FindEntityByName("SfxVolume_Container10_Filled");
        }
        else
        {
            Debug.Log("Error: There is no SfxVolume_Hovered Entity assigned.");
        }

        // Loop Music
        if (loopMusicEntity != null)
        {
            loopMusicAudioSource = loopMusicEntity.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Error: There is no Loop Music Entity assigned.");
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
        enterTimer += Time.unscaledDeltaTime;
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

            enterTimer = 0f;
            quickStartAnimations = false;
        }

        if (!invertedPassPagePlayed)
        {
            invertedPassPageEntity.SetActive(true);
            
            if (enterTimer > enterCooldown)
            {
                invertedPassPageAnimator.Play();
                invertedPassPagePlayed = true;
            }
        }
        else
        {
            if (invertedPassPageAnimator.CurrentFrame == invertedPassPageAnimator.FrameCount - 1)
            {
                invertedPassPageEntity.SetActive(false);
                uiManagerScript.BlockNavigation = false;
            }
        }
        
        HandleMusic();
        HandleChangeValue();
        HandleApplyChanges();
        HandleBack();
        HandleVisualFeedback();
    }
    public void Open()
    {
        uiManagerScript.SelectedElement = displayModeEntity;
        entity.SetActive(true);
        uiManagerScript.BlockNavigation = true;
    }
    void HandleChangeValue()
    {
        confirmTimer += Time.unscaledDeltaTime;

        float deadzone = 0.3f;
        float strongDeadzone = 0.7f;

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
            if (Mathf.Abs(Input.LeftAxis.x) < deadzone)
            {
                isEditingValue = false;
            }
        }
    }

    void ChangeValueRight()
    {
        if (displayModeButton.Hovered)
        {
            displayModeScript.ToggleFullscreen();
            if (displayModeScript.IsFullscreen()) { displayModeDisplayText.SetText("Fullscreen"); }
            else { displayModeDisplayText.SetText("Windowed"); }
        }
        else if (framerateButton.Hovered)
        {
            framerateScript.IncreaseFramerate();
            framerateDisplayText.SetText(framerateScript.GetFramerate().ToString());
        }
        else if (vSyncButton.Hovered)
        {
            vSyncScript.ToggleVSync();
            if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
            else { vSyncDisplayText.SetText("Off"); }
        }
        else if (masterVolumeButton.Hovered)
        {
            masterVolumeScript.IncreaseVolume();
            float masterVolume = Mathf.Abs(masterVolumeScript.GetVolume() * 100);
            masterVolumeDisplayText.SetText(Mathf.Round(masterVolume).ToString());
            MasterVolume.ApplyMasterVolume();
        }
        else if (musicVolumeButton.Hovered)
        {
            musicVolumeScript.IncreaseVolume();
            float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
            musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
            MusicVolume.ApplyMusicVolume();
        }
        else if (sfxVolumeButton.Hovered)
        {
            sfxVolumeScript.IncreaseVolume();
            float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
            sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
            SfxVolume.ApplySfxVolume();
        }
    }

    void ChangeValueLeft()
    {
        if (displayModeButton.Hovered)
        {
            displayModeScript.ToggleFullscreen();
            if (displayModeScript.IsFullscreen()) { displayModeDisplayText.SetText("Fullscreen"); }
            else { displayModeDisplayText.SetText("Windowed"); }
        }
        else if (framerateButton.Hovered)
        {
            framerateScript.DecreaseFramerate();
            framerateDisplayText.SetText(framerateScript.GetFramerate().ToString());
        }
        else if (vSyncButton.Hovered)
        {
            vSyncScript.ToggleVSync();
            if (vSyncScript.IsVSync()) { vSyncDisplayText.SetText("On"); }
            else { vSyncDisplayText.SetText("Off"); }
        }
        else if (masterVolumeButton.Hovered)
        {
            masterVolumeScript.DecreaseVolume();
            float masterVolume = masterVolumeScript.GetVolume() * 100;
            masterVolumeDisplayText.SetText(Mathf.Round(masterVolume).ToString());
            MasterVolume.ApplyMasterVolume();
        }
        else if (musicVolumeButton.Hovered)
        {
            musicVolumeScript.DecreaseVolume();
            float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
            musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
            MusicVolume.ApplyMusicVolume();
        }
        else if (sfxVolumeButton.Hovered)
        {
            sfxVolumeScript.DecreaseVolume();
            float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
            sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
            SfxVolume.ApplySfxVolume();
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
        applyTimer += Time.unscaledDeltaTime;

        // Cooldown
        if (applyTimer < inputCooldown)
            return;

        if (Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
        {
            displayModeScript.ApplyFullscreen();
            framerateScript.ApplyFramerate();
            vSyncScript.ApplyVSync();
            MasterVolume.ApplyMasterVolume();
            MusicVolume.ApplyMusicVolume();
            SfxVolume.ApplySfxVolume();

            GlobalDatabase.GlobalData.settingsDB.Settings.AreSettingsDefault = false;
            GlobalDatabase.GlobalData.SaveGlobalDatabase();

            applyTimer = 0f;
        }
    }
    void HandleBack()
    {
        if (Input.IsKeyPressed(KeyCode.RETURN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_B))
        {
            passPageEntity.SetActive(true);
            passPageAnimator.Play();
        }
        if (passPageAnimator.CurrentFrame == passPageAnimator.FrameCount - 1)
        {
            if (GlobalDatabase.GlobalData.mainMenuDB.MainMenu.IsInMainMenu)
            {
                SceneManager.LoadSceneByID("db1dd4f7-fb12-b501-b8a7-ac788f03b8ae");
                MainMenu.quickStartAnimations = true;
                MainMenu.invertedPassPagePlayed = false;
            }
            else 
            {
                DatabaseRegistry.LoadAll();
                SceneManager.LoadSceneByID(DatabaseRegistry.playerDB.Player.currentSceneUUID);
            }
        }
    }
    void HandleVisualFeedback()
    {
        masterVolumeContainer1Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 10);
        masterVolumeContainer2Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 20);
        masterVolumeContainer3Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 30);
        masterVolumeContainer4Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 40);
        masterVolumeContainer5Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 50);
        masterVolumeContainer6Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 60);
        masterVolumeContainer7Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 70);
        masterVolumeContainer8Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 80);
        masterVolumeContainer9Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 90);
        masterVolumeContainer10Filled.SetActive(Mathf.Round(Mathf.Abs(masterVolumeScript.GetVolume() * 100)) >= 100);

        musicVolumeContainer1Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 10);
        musicVolumeContainer2Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 20);
        musicVolumeContainer3Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 30);
        musicVolumeContainer4Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 40);
        musicVolumeContainer5Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 50);
        musicVolumeContainer6Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 60);
        musicVolumeContainer7Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 70);
        musicVolumeContainer8Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 80);
        musicVolumeContainer9Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 90);
        musicVolumeContainer10Filled.SetActive(Mathf.Round(Mathf.Abs(musicVolumeScript.GetVolume() * 100)) >= 100);

        sfxVolumeContainer1Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 10);
        sfxVolumeContainer2Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 20);
        sfxVolumeContainer3Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 30);
        sfxVolumeContainer4Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 40);
        sfxVolumeContainer5Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 50);
        sfxVolumeContainer6Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 60);
        sfxVolumeContainer7Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 70);
        sfxVolumeContainer8Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 80);
        sfxVolumeContainer9Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 90);
        sfxVolumeContainer10Filled.SetActive(Mathf.Round(Mathf.Abs(sfxVolumeScript.GetVolume() * 100)) >= 100);
    }
};