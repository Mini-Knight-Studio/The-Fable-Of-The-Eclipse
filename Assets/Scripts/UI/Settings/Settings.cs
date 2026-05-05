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
    //private Entity masterVolumeContainer1Filled;
    //private Entity masterVolumeContainer1FilledHovered;
    //private Entity masterVolumeContainer1EmptyHovered;

    //private Entity masterVolumeContainer2Filled;
    //private Entity masterVolumeContainer2FilledHovered;
    //private Entity masterVolumeContainer2EmptyHovered;

    //private Entity masterVolumeContainer3Filled;
    //private Entity masterVolumeContainer3FilledHovered;
    //private Entity masterVolumeContainer3EmptyHovered;

    //private Entity masterVolumeContainer4Filled;
    //private Entity masterVolumeContainer4FilledHovered;
    //private Entity masterVolumeContainer4EmptyHovered;

    //private Entity masterVolumeContainer5Filled;
    //private Entity masterVolumeContainer5FilledHovered;
    //private Entity masterVolumeContainer5EmptyHovered;

    //private Entity masterVolumeContainer6Filled;
    //private Entity masterVolumeContainer6FilledHovered;
    //private Entity masterVolumeContainer6EmptyHovered;

    //private Entity masterVolumeContainer7Filled;
    //private Entity masterVolumeContainer7FilledHovered;
    //private Entity masterVolumeContainer7EmptyHovered;

    //private Entity masterVolumeContainer8Filled;
    //private Entity masterVolumeContainer8FilledHovered;
    //private Entity masterVolumeContainer8EmptyHovered;

    //private Entity masterVolumeContainer9Filled;
    //private Entity masterVolumeContainer9FilledHovered;
    //private Entity masterVolumeContainer9EmptyHovered;

    //private Entity masterVolumeContainer10Filled;
    //private Entity masterVolumeContainer10FilledHovered;
    //private Entity masterVolumeContainer10EmptyHovered;

    // Music Volume 
    public Entity musicVolumeEntity;
    private MusicVolume musicVolumeScript;
    private Text musicVolumeDisplayText;
    private Button musicVolumeButton;
    private Button musicVolumeLeftArrowButton;
    private Button musicVolumeRightArrowButton;

    // Music Volume Containers
    //private Entity musicVolumeContainer1Filled;
    //private Entity musicVolumeContainer1FilledHovered;
    //private Entity musicVolumeContainer1EmptyHovered;

    //private Entity musicVolumeContainer2Filled;
    //private Entity musicVolumeContainer2FilledHovered;
    //private Entity musicVolumeContainer2EmptyHovered;

    //private Entity musicVolumeContainer3Filled;
    //private Entity musicVolumeContainer3FilledHovered;
    //private Entity musicVolumeContainer3EmptyHovered;

    //private Entity musicVolumeContainer4Filled;
    //private Entity musicVolumeContainer4FilledHovered;
    //private Entity musicVolumeContainer4EmptyHovered;

    //private Entity musicVolumeContainer5Filled;
    //private Entity musicVolumeContainer5FilledHovered;
    //private Entity musicVolumeContainer5EmptyHovered;

    //private Entity musicVolumeContainer6Filled;
    //private Entity musicVolumeContainer6FilledHovered;
    //private Entity musicVolumeContainer6EmptyHovered;

    //private Entity musicVolumeContainer7Filled;
    //private Entity musicVolumeContainer7FilledHovered;
    //private Entity musicVolumeContainer7EmptyHovered;

    //private Entity musicVolumeContainer8Filled;
    //private Entity musicVolumeContainer8FilledHovered;
    //private Entity musicVolumeContainer8EmptyHovered;

    //private Entity musicVolumeContainer9Filled;
    //private Entity musicVolumeContainer9FilledHovered;
    //private Entity musicVolumeContainer9EmptyHovered;

    //private Entity musicVolumeContainer10Filled;
    //private Entity musicVolumeContainer10FilledHovered;
    //private Entity musicVolumeContainer10EmptyHovered;

    // Sfx Volume
    public Entity sfxVolumeEntity;
    private SfxVolume sfxVolumeScript;
    private Text sfxVolumeDisplayText;
    private Button sfxVolumeButton;
    private Button sfxVolumeLeftArrowButton;
    private Button sfxVolumeRightArrowButton;

    // Sfx Volume Containers
    //private Entity sfxVolumeContainer1Filled;
    //private Entity sfxVolumeContainer1FilledHovered;
    //private Entity sfxVolumeContainer1EmptyHovered;

    //private Entity sfxVolumeContainer2Filled;
    //private Entity sfxVolumeContainer2FilledHovered;
    //private Entity sfxVolumeContainer2EmptyHovered;

    //private Entity sfxVolumeContainer3Filled;
    //private Entity sfxVolumeContainer3FilledHovered;
    //private Entity sfxVolumeContainer3EmptyHovered;

    //private Entity sfxVolumeContainer4Filled;
    //private Entity sfxVolumeContainer4FilledHovered;
    //private Entity sfxVolumeContainer4EmptyHovered;

    //private Entity sfxVolumeContainer5Filled;
    //private Entity sfxVolumeContainer5FilledHovered;
    //private Entity sfxVolumeContainer5EmptyHovered;

    //private Entity sfxVolumeContainer6Filled;
    //private Entity sfxVolumeContainer6FilledHovered;
    //private Entity sfxVolumeContainer6EmptyHovered;

    //private Entity sfxVolumeContainer7Filled;
    //private Entity sfxVolumeContainer7FilledHovered;
    //private Entity sfxVolumeContainer7EmptyHovered;

    //private Entity sfxVolumeContainer8Filled;
    //private Entity sfxVolumeContainer8FilledHovered;
    //private Entity sfxVolumeContainer8EmptyHovered;

    //private Entity sfxVolumeContainer9Filled;
    //private Entity sfxVolumeContainer9FilledHovered;
    //private Entity sfxVolumeContainer9EmptyHovered;

    //private Entity sfxVolumeContainer10Filled;
    //private Entity sfxVolumeContainer10FilledHovered;
    //private Entity sfxVolumeContainer10EmptyHovered;

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
            //masterVolumeContainer1Filled = Entity.FindEntityByName("MasterVolume_Container1_Filled");
            //masterVolumeContainer1FilledHovered = Entity.FindEntityByName("MasterVolume_Container1_Filled_Hovered");
            //masterVolumeContainer1EmptyHovered = Entity.FindEntityByName("MasterVolume_Container1_Empty_Hovered");

            //masterVolumeContainer2Filled = Entity.FindEntityByName("MasterVolume_Container2_Filled");
            //masterVolumeContainer2FilledHovered = Entity.FindEntityByName("MasterVolume_Container2_Filled_Hovered");
            //masterVolumeContainer2EmptyHovered = Entity.FindEntityByName("MasterVolume_Container2_Empty_Hovered");

            //masterVolumeContainer3Filled = Entity.FindEntityByName("MasterVolume_Container3_Filled");
            //masterVolumeContainer3FilledHovered = Entity.FindEntityByName("MasterVolume_Container3_Filled_Hovered");
            //masterVolumeContainer3EmptyHovered = Entity.FindEntityByName("MasterVolume_Container3_Empty_Hovered");

            //masterVolumeContainer4Filled = Entity.FindEntityByName("MasterVolume_Container4_Filled");
            //masterVolumeContainer4FilledHovered = Entity.FindEntityByName("MasterVolume_Container4_Filled_Hovered");
            //masterVolumeContainer4EmptyHovered = Entity.FindEntityByName("MasterVolume_Container4_Empty_Hovered");

            //masterVolumeContainer5Filled = Entity.FindEntityByName("MasterVolume_Container5_Filled");
            //masterVolumeContainer5FilledHovered = Entity.FindEntityByName("MasterVolume_Container5_Filled_Hovered");
            //masterVolumeContainer5EmptyHovered = Entity.FindEntityByName("MasterVolume_Container5_Empty_Hovered");

            //masterVolumeContainer6Filled = Entity.FindEntityByName("MasterVolume_Container6_Filled");
            //masterVolumeContainer6FilledHovered = Entity.FindEntityByName("MasterVolume_Container6_Filled_Hovered");
            //masterVolumeContainer6EmptyHovered = Entity.FindEntityByName("MasterVolume_Container6_Empty_Hovered");

            //masterVolumeContainer7Filled = Entity.FindEntityByName("MasterVolume_Container7_Filled");
            //masterVolumeContainer7FilledHovered = Entity.FindEntityByName("MasterVolume_Container7_Filled_Hovered");
            //masterVolumeContainer7EmptyHovered = Entity.FindEntityByName("MasterVolume_Container7_Empty_Hovered");

            //masterVolumeContainer8Filled = Entity.FindEntityByName("MasterVolume_Container8_Filled");
            //masterVolumeContainer8FilledHovered = Entity.FindEntityByName("MasterVolume_Container8_Filled_Hovered");
            //masterVolumeContainer8EmptyHovered = Entity.FindEntityByName("MasterVolume_Container8_Empty_Hovered");

            //masterVolumeContainer9Filled = Entity.FindEntityByName("MasterVolume_Container9_Filled");
            //masterVolumeContainer9FilledHovered = Entity.FindEntityByName("MasterVolume_Container9_Filled_Hovered");
            //masterVolumeContainer9EmptyHovered = Entity.FindEntityByName("MasterVolume_Container9_Empty_Hovered");

            //masterVolumeContainer10Filled = Entity.FindEntityByName("MasterVolume_Container10_Filled");
            //masterVolumeContainer10FilledHovered = Entity.FindEntityByName("MasterVolume_Container10_Filled_Hovered");
            //masterVolumeContainer10EmptyHovered = Entity.FindEntityByName("MasterVolume_Container10_Empty_Hovered");
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
            //musicVolumeContainer1Filled = Entity.FindEntityByName("MusicVolume_Container1_Filled");
            //musicVolumeContainer1FilledHovered = Entity.FindEntityByName("MusicVolume_Container1_Filled_Hovered");
            //musicVolumeContainer1EmptyHovered = Entity.FindEntityByName("MusicVolume_Container1_Empty_Hovered");
             
            //musicVolumeContainer2Filled = Entity.FindEntityByName("MusicVolume_Container2_Filled");
            //musicVolumeContainer2FilledHovered = Entity.FindEntityByName("MusicVolume_Container2_Filled_Hovered");
            //musicVolumeContainer2EmptyHovered = Entity.FindEntityByName("MusicVolume_Container2_Empty_Hovered");
             
            //musicVolumeContainer3Filled = Entity.FindEntityByName("MusicVolume_Container3_Filled");
            //musicVolumeContainer3FilledHovered = Entity.FindEntityByName("MusicVolume_Container3_Filled_Hovered");
            //musicVolumeContainer3EmptyHovered = Entity.FindEntityByName("MusicVolume_Container3_Empty_Hovered");
             
            //musicVolumeContainer4Filled = Entity.FindEntityByName("MusicVolume_Container4_Filled");
            //musicVolumeContainer4FilledHovered = Entity.FindEntityByName("MusicVolume_Container4_Filled_Hovered");
            //musicVolumeContainer4EmptyHovered = Entity.FindEntityByName("MusicVolume_Container4_Empty_Hovered");
             
            //musicVolumeContainer5Filled = Entity.FindEntityByName("MusicVolume_Container5_Filled");
            //musicVolumeContainer5FilledHovered = Entity.FindEntityByName("MusicVolume_Container5_Filled_Hovered");
            //musicVolumeContainer5EmptyHovered = Entity.FindEntityByName("MusicVolume_Container5_Empty_Hovered");
             
            //musicVolumeContainer6Filled = Entity.FindEntityByName("MusicVolume_Container6_Filled");
            //musicVolumeContainer6FilledHovered = Entity.FindEntityByName("MusicVolume_Container6_Filled_Hovered");
            //musicVolumeContainer6EmptyHovered = Entity.FindEntityByName("MusicVolume_Container6_Empty_Hovered");
             
            //musicVolumeContainer7Filled = Entity.FindEntityByName("MusicVolume_Container7_Filled");
            //musicVolumeContainer7FilledHovered = Entity.FindEntityByName("MusicVolume_Container7_Filled_Hovered");
            //musicVolumeContainer7EmptyHovered = Entity.FindEntityByName("MusicVolume_Container7_Empty_Hovered");
             
            //musicVolumeContainer8Filled = Entity.FindEntityByName("MusicVolume_Container8_Filled");
            //musicVolumeContainer8FilledHovered = Entity.FindEntityByName("MusicVolume_Container8_Filled_Hovered");
            //musicVolumeContainer8EmptyHovered = Entity.FindEntityByName("MusicVolume_Container8_Empty_Hovered");
             
            //musicVolumeContainer9Filled = Entity.FindEntityByName("MusicVolume_Container9_Filled");
            //musicVolumeContainer9FilledHovered = Entity.FindEntityByName("MusicVolume_Container9_Filled_Hovered");
            //musicVolumeContainer9EmptyHovered = Entity.FindEntityByName("MusicVolume_Container9_Empty_Hovered");
             
            //musicVolumeContainer10Filled = Entity.FindEntityByName("MusicVolume_Container10_Filled");
            //musicVolumeContainer10FilledHovered = Entity.FindEntityByName("MusicVolume_Container10_Filled_Hovered");
            //musicVolumeContainer10EmptyHovered = Entity.FindEntityByName("MusicVolume_Container10_Empty_Hovered");
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
            //sfxVolumeContainer1Filled = Entity.FindEntityByName("SfxVolume_Container1_Filled");
            //sfxVolumeContainer1FilledHovered = Entity.FindEntityByName("SfxVolume_Container1_Filled_Hovered");
            //sfxVolumeContainer1EmptyHovered = Entity.FindEntityByName("SfxVolume_Container1_Empty_Hovered");
            
            //sfxVolumeContainer2Filled = Entity.FindEntityByName("SfxVolume_Container2_Filled");
            //sfxVolumeContainer2FilledHovered = Entity.FindEntityByName("SfxVolume_Container2_Filled_Hovered");
            //sfxVolumeContainer2EmptyHovered = Entity.FindEntityByName("SfxVolume_Container2_Empty_Hovered");
            
            //sfxVolumeContainer3Filled = Entity.FindEntityByName("SfxVolume_Container3_Filled");
            //sfxVolumeContainer3FilledHovered = Entity.FindEntityByName("SfxVolume_Container3_Filled_Hovered");
            //sfxVolumeContainer3EmptyHovered = Entity.FindEntityByName("SfxVolume_Container3_Empty_Hovered");
            
            //sfxVolumeContainer4Filled = Entity.FindEntityByName("SfxVolume_Container4_Filled");
            //sfxVolumeContainer4FilledHovered = Entity.FindEntityByName("SfxVolume_Container4_Filled_Hovered");
            //sfxVolumeContainer4EmptyHovered = Entity.FindEntityByName("SfxVolume_Container4_Empty_Hovered");
            
            //sfxVolumeContainer5Filled = Entity.FindEntityByName("SfxVolume_Container5_Filled");
            //sfxVolumeContainer5FilledHovered = Entity.FindEntityByName("SfxVolume_Container5_Filled_Hovered");
            //sfxVolumeContainer5EmptyHovered = Entity.FindEntityByName("SfxVolume_Container5_Empty_Hovered");
            
            //sfxVolumeContainer6Filled = Entity.FindEntityByName("SfxVolume_Container6_Filled");
            //sfxVolumeContainer6FilledHovered = Entity.FindEntityByName("SfxVolume_Container6_Filled_Hovered");
            //sfxVolumeContainer6EmptyHovered = Entity.FindEntityByName("SfxVolume_Container6_Empty_Hovered");
            
            //sfxVolumeContainer7Filled = Entity.FindEntityByName("SfxVolume_Container7_Filled");
            //sfxVolumeContainer7FilledHovered = Entity.FindEntityByName("SfxVolume_Container7_Filled_Hovered");
            //sfxVolumeContainer7EmptyHovered = Entity.FindEntityByName("SfxVolume_Container7_Empty_Hovered");
            
            //sfxVolumeContainer8Filled = Entity.FindEntityByName("SfxVolume_Container8_Filled");
            //sfxVolumeContainer8FilledHovered = Entity.FindEntityByName("SfxVolume_Container8_Filled_Hovered");
            //sfxVolumeContainer8EmptyHovered = Entity.FindEntityByName("SfxVolume_Container8_Empty_Hovered");
            
            //sfxVolumeContainer9Filled = Entity.FindEntityByName("SfxVolume_Container9_Filled");
            //sfxVolumeContainer9FilledHovered = Entity.FindEntityByName("SfxVolume_Container9_Filled_Hovered");
            //sfxVolumeContainer9EmptyHovered = Entity.FindEntityByName("SfxVolume_Container9_Empty_Hovered");
            
            //sfxVolumeContainer10Filled = Entity.FindEntityByName("SfxVolume_Container10_Filled");
            //sfxVolumeContainer10FilledHovered = Entity.FindEntityByName("SfxVolume_Container10_Filled_Hovered");
            //sfxVolumeContainer10EmptyHovered = Entity.FindEntityByName("SfxVolume_Container10_Empty_Hovered");
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
    }
    public void Open()
    {
        uiManagerScript.SelectedElement = displayModeEntity;
        entity.SetActive(true);
        uiManagerScript.BlockNavigation = true;
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
            masterVolumeScript.ApplyMasterVolume();
        }
        else if (musicVolumeButton.Hovered)
        {
            musicVolumeScript.IncreaseVolume();
            float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
            musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
            musicVolumeScript.ApplyMusicVolume();
        }
        else if (sfxVolumeButton.Hovered)
        {
            sfxVolumeScript.IncreaseVolume();
            float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
            sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
            sfxVolumeScript.ApplySfxVolume();
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
            masterVolumeScript.ApplyMasterVolume();
        }
        else if (musicVolumeButton.Hovered)
        {
            musicVolumeScript.DecreaseVolume();
            float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
            musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
            musicVolumeScript.ApplyMusicVolume();
        }
        else if (sfxVolumeButton.Hovered)
        {
            sfxVolumeScript.DecreaseVolume();
            float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
            sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
            sfxVolumeScript.ApplySfxVolume();
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
            masterVolumeScript.ApplyMasterVolume();
            musicVolumeScript.ApplyMusicVolume();
            sfxVolumeScript.ApplySfxVolume();

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
            SceneManager.LoadSceneByID("db1dd4f7-fb12-b501-b8a7-ac788f03b8ae");
            MainMenu.quickStartAnimations = true;
            MainMenu.invertedPassPagePlayed = false;
        }
    }
    void HandleVisualFeedback()
    {
        // Visual Feedback
        Vector4 inactiveColor = new Vector4(0.196f, 0.196f, 0.510f, 1.0f);
        Vector4 activeColor = new Vector4(0.706f, 0.000f, 0.216f, 1.0f);

        if (displayModeButton.Hovered)
        {
            //displayModeLeftArrowButton.Hovered = true;
            //displayModeRightArrowButton.Hovered = true;
            displayModeDisplayText.SetColor(activeColor);
        }
        else if (framerateButton.Hovered)
        {
            //framerateLeftArrowButton.Hovered = true;
            //framerateRightArrowButton.Hovered = true;
            framerateDisplayText.SetColor(activeColor);
        }
        else if (vSyncButton.Hovered)
        {
            //vSyncLeftArrowButton.Hovered = true;
            //vSyncRightArrowButton.Hovered = true;
            vSyncDisplayText.SetColor(activeColor);
        }
        else if (masterVolumeButton.Hovered)
        {
            //masterVolumeLeftArrowButton.Hovered = true;
            //masterVolumeRightArrowButton.Hovered = true;
            masterVolumeDisplayText.SetColor(activeColor);
        }
        else if (musicVolumeButton.Hovered)
        {
            //musicVolumeLeftArrowButton.Hovered = true;
            //musicVolumeRightArrowButton.Hovered = true;
            musicVolumeDisplayText.SetColor(activeColor);
        }
        else if (sfxVolumeButton.Hovered)
        {
            //sfxVolumeLeftArrowButton.Hovered = true;
            //sfxVolumeRightArrowButton.Hovered = true;
            sfxVolumeDisplayText.SetColor(activeColor);
        }
    }

    //void HandleSfxContainers()
    //{
    //    if (currentButton == Buttons.SFX_VOLUME)
    //    {
    //        sfxVolumeContainer1EmptyHovered.SetActive(true);
    //        sfxVolumeContainer2EmptyHovered.SetActive(true);
    //        sfxVolumeContainer3EmptyHovered.SetActive(true);
    //        sfxVolumeContainer4EmptyHovered.SetActive(true);
    //        sfxVolumeContainer5EmptyHovered.SetActive(true);
    //        sfxVolumeContainer6EmptyHovered.SetActive(true);
    //        sfxVolumeContainer7EmptyHovered.SetActive(true);
    //        sfxVolumeContainer8EmptyHovered.SetActive(true);
    //        sfxVolumeContainer9EmptyHovered.SetActive(true);
    //        sfxVolumeContainer10EmptyHovered.SetActive(true);
    //    }
    //    else
    //    {
    //        sfxVolumeContainer1EmptyHovered.SetActive(false);
    //        sfxVolumeContainer2EmptyHovered.SetActive(false);
    //        sfxVolumeContainer3EmptyHovered.SetActive(false);
    //        sfxVolumeContainer4EmptyHovered.SetActive(false);
    //        sfxVolumeContainer5EmptyHovered.SetActive(false);
    //        sfxVolumeContainer6EmptyHovered.SetActive(false);
    //        sfxVolumeContainer7EmptyHovered.SetActive(false);
    //        sfxVolumeContainer8EmptyHovered.SetActive(false);
    //        sfxVolumeContainer9EmptyHovered.SetActive(false);
    //        sfxVolumeContainer10EmptyHovered.SetActive(false);
    //    }

    //    int sfxVolume = (int)(sfxVolumeScript.GetVolume() * 100);

    //    switch (sfxVolume)
    //    {
    //        case 0:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(false);
    //                sfxVolumeContainer2Filled.SetActive(false);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(false);
    //                sfxVolumeContainer2Filled.SetActive(false);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 10:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(false);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(false);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 20:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(false);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 30:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(false);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 40:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(false);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 50:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(false);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 60:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(true);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(false);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 70:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(true);
    //                sfxVolumeContainer7FilledHovered.SetActive(true);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(false);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 80:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(true);
    //                sfxVolumeContainer7FilledHovered.SetActive(true);
    //                sfxVolumeContainer8FilledHovered.SetActive(true);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(false);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 90:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(true);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(true);
    //                sfxVolumeContainer7FilledHovered.SetActive(true);
    //                sfxVolumeContainer8FilledHovered.SetActive(true);
    //                sfxVolumeContainer9FilledHovered.SetActive(true);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(true);
    //                sfxVolumeContainer10Filled.SetActive(false);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 100:
    //            if (currentButton == Buttons.SFX_VOLUME)
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(true);
    //                sfxVolumeContainer10Filled.SetActive(true);

    //                sfxVolumeContainer1FilledHovered.SetActive(true);
    //                sfxVolumeContainer2FilledHovered.SetActive(true);
    //                sfxVolumeContainer3FilledHovered.SetActive(true);
    //                sfxVolumeContainer4FilledHovered.SetActive(true);
    //                sfxVolumeContainer5FilledHovered.SetActive(true);
    //                sfxVolumeContainer6FilledHovered.SetActive(true);
    //                sfxVolumeContainer7FilledHovered.SetActive(true);
    //                sfxVolumeContainer8FilledHovered.SetActive(true);
    //                sfxVolumeContainer9FilledHovered.SetActive(true);
    //                sfxVolumeContainer10FilledHovered.SetActive(true);
    //            }
    //            else
    //            {
    //                sfxVolumeContainer1Filled.SetActive(true);
    //                sfxVolumeContainer2Filled.SetActive(true);
    //                sfxVolumeContainer3Filled.SetActive(true);
    //                sfxVolumeContainer4Filled.SetActive(true);
    //                sfxVolumeContainer5Filled.SetActive(true);
    //                sfxVolumeContainer6Filled.SetActive(true);
    //                sfxVolumeContainer7Filled.SetActive(true);
    //                sfxVolumeContainer8Filled.SetActive(true);
    //                sfxVolumeContainer9Filled.SetActive(true);
    //                sfxVolumeContainer10Filled.SetActive(true);

    //                sfxVolumeContainer1FilledHovered.SetActive(false);
    //                sfxVolumeContainer2FilledHovered.SetActive(false);
    //                sfxVolumeContainer3FilledHovered.SetActive(false);
    //                sfxVolumeContainer4FilledHovered.SetActive(false);
    //                sfxVolumeContainer5FilledHovered.SetActive(false);
    //                sfxVolumeContainer6FilledHovered.SetActive(false);
    //                sfxVolumeContainer7FilledHovered.SetActive(false);
    //                sfxVolumeContainer8FilledHovered.SetActive(false);
    //                sfxVolumeContainer9FilledHovered.SetActive(false);
    //                sfxVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //void HandleMusicContainers()
    //{
    //    if (currentButton == Buttons.MUSIC_VOLUME)
    //    {
    //        musicVolumeContainer1EmptyHovered.SetActive(true);
    //        musicVolumeContainer2EmptyHovered.SetActive(true);
    //        musicVolumeContainer3EmptyHovered.SetActive(true);
    //        musicVolumeContainer4EmptyHovered.SetActive(true);
    //        musicVolumeContainer5EmptyHovered.SetActive(true);
    //        musicVolumeContainer6EmptyHovered.SetActive(true);
    //        musicVolumeContainer7EmptyHovered.SetActive(true);
    //        musicVolumeContainer8EmptyHovered.SetActive(true);
    //        musicVolumeContainer9EmptyHovered.SetActive(true);
    //        musicVolumeContainer10EmptyHovered.SetActive(true);
    //    }
    //    else
    //    {
    //        musicVolumeContainer1EmptyHovered.SetActive(false);
    //        musicVolumeContainer2EmptyHovered.SetActive(false);
    //        musicVolumeContainer3EmptyHovered.SetActive(false);
    //        musicVolumeContainer4EmptyHovered.SetActive(false);
    //        musicVolumeContainer5EmptyHovered.SetActive(false);
    //        musicVolumeContainer6EmptyHovered.SetActive(false);
    //        musicVolumeContainer7EmptyHovered.SetActive(false);
    //        musicVolumeContainer8EmptyHovered.SetActive(false);
    //        musicVolumeContainer9EmptyHovered.SetActive(false);
    //        musicVolumeContainer10EmptyHovered.SetActive(false);
    //    }

    //    int musicVolume = (int)(musicVolumeScript.GetVolume() * 100);

    //    switch (musicVolume)
    //    {
    //        case 0:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(false);
    //                musicVolumeContainer2Filled.SetActive(false);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(false);
    //                musicVolumeContainer2Filled.SetActive(false);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 10:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(false);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(false);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 20:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(false);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 30:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(false);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 40:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(false);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 50:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(false);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 60:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(true);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(false);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 70:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(true);
    //                musicVolumeContainer7FilledHovered.SetActive(true);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(false);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 80:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(true);
    //                musicVolumeContainer7FilledHovered.SetActive(true);
    //                musicVolumeContainer8FilledHovered.SetActive(true);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(false);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 90:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(true);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(true);
    //                musicVolumeContainer7FilledHovered.SetActive(true);
    //                musicVolumeContainer8FilledHovered.SetActive(true);
    //                musicVolumeContainer9FilledHovered.SetActive(true);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(true);
    //                musicVolumeContainer10Filled.SetActive(false);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 100:
    //            if (currentButton == Buttons.MUSIC_VOLUME)
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(true);
    //                musicVolumeContainer10Filled.SetActive(true);

    //                musicVolumeContainer1FilledHovered.SetActive(true);
    //                musicVolumeContainer2FilledHovered.SetActive(true);
    //                musicVolumeContainer3FilledHovered.SetActive(true);
    //                musicVolumeContainer4FilledHovered.SetActive(true);
    //                musicVolumeContainer5FilledHovered.SetActive(true);
    //                musicVolumeContainer6FilledHovered.SetActive(true);
    //                musicVolumeContainer7FilledHovered.SetActive(true);
    //                musicVolumeContainer8FilledHovered.SetActive(true);
    //                musicVolumeContainer9FilledHovered.SetActive(true);
    //                musicVolumeContainer10FilledHovered.SetActive(true);
    //            }
    //            else
    //            {
    //                musicVolumeContainer1Filled.SetActive(true);
    //                musicVolumeContainer2Filled.SetActive(true);
    //                musicVolumeContainer3Filled.SetActive(true);
    //                musicVolumeContainer4Filled.SetActive(true);
    //                musicVolumeContainer5Filled.SetActive(true);
    //                musicVolumeContainer6Filled.SetActive(true);
    //                musicVolumeContainer7Filled.SetActive(true);
    //                musicVolumeContainer8Filled.SetActive(true);
    //                musicVolumeContainer9Filled.SetActive(true);
    //                musicVolumeContainer10Filled.SetActive(true);

    //                musicVolumeContainer1FilledHovered.SetActive(false);
    //                musicVolumeContainer2FilledHovered.SetActive(false);
    //                musicVolumeContainer3FilledHovered.SetActive(false);
    //                musicVolumeContainer4FilledHovered.SetActive(false);
    //                musicVolumeContainer5FilledHovered.SetActive(false);
    //                musicVolumeContainer6FilledHovered.SetActive(false);
    //                musicVolumeContainer7FilledHovered.SetActive(false);
    //                musicVolumeContainer8FilledHovered.SetActive(false);
    //                musicVolumeContainer9FilledHovered.SetActive(false);
    //                musicVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //void HandleMasterContainers()
    //{
    //    if (currentButton == Buttons.MASTER_VOLUME)
    //    {
    //        masterVolumeContainer1EmptyHovered.SetActive(true);
    //        masterVolumeContainer2EmptyHovered.SetActive(true);
    //        masterVolumeContainer3EmptyHovered.SetActive(true);
    //        masterVolumeContainer4EmptyHovered.SetActive(true);
    //        masterVolumeContainer5EmptyHovered.SetActive(true);
    //        masterVolumeContainer6EmptyHovered.SetActive(true);
    //        masterVolumeContainer7EmptyHovered.SetActive(true);
    //        masterVolumeContainer8EmptyHovered.SetActive(true);
    //        masterVolumeContainer9EmptyHovered.SetActive(true);
    //        masterVolumeContainer10EmptyHovered.SetActive(true);
    //    }
    //    else
    //    {
    //        masterVolumeContainer1EmptyHovered.SetActive(false);
    //        masterVolumeContainer2EmptyHovered.SetActive(false);
    //        masterVolumeContainer3EmptyHovered.SetActive(false);
    //        masterVolumeContainer4EmptyHovered.SetActive(false);
    //        masterVolumeContainer5EmptyHovered.SetActive(false);
    //        masterVolumeContainer6EmptyHovered.SetActive(false);
    //        masterVolumeContainer7EmptyHovered.SetActive(false);
    //        masterVolumeContainer8EmptyHovered.SetActive(false);
    //        masterVolumeContainer9EmptyHovered.SetActive(false);
    //        masterVolumeContainer10EmptyHovered.SetActive(false);
    //    }

    //    int masterVolume = (int)(masterVolumeScript.GetVolume() * 100);

    //    switch (masterVolume)
    //    {
    //        case 0:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(false);
    //                masterVolumeContainer2Filled.SetActive(false);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(false);
    //                masterVolumeContainer2Filled.SetActive(false);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 10:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(false);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(false);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 20:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(false);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 30:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(false);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 40:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(false);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 50:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(false);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 60:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(true);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(false);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 70:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(true);
    //                masterVolumeContainer7FilledHovered.SetActive(true);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(false);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 80:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(true);
    //                masterVolumeContainer7FilledHovered.SetActive(true);
    //                masterVolumeContainer8FilledHovered.SetActive(true);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(false);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 90:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(true);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(true);
    //                masterVolumeContainer7FilledHovered.SetActive(true);
    //                masterVolumeContainer8FilledHovered.SetActive(true);
    //                masterVolumeContainer9FilledHovered.SetActive(true);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(true);
    //                masterVolumeContainer10Filled.SetActive(false);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        case 100:
    //            if (currentButton == Buttons.MASTER_VOLUME)
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(true);
    //                masterVolumeContainer10Filled.SetActive(true);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(true);
    //                masterVolumeContainer2FilledHovered.SetActive(true);
    //                masterVolumeContainer3FilledHovered.SetActive(true);
    //                masterVolumeContainer4FilledHovered.SetActive(true);
    //                masterVolumeContainer5FilledHovered.SetActive(true);
    //                masterVolumeContainer6FilledHovered.SetActive(true);
    //                masterVolumeContainer7FilledHovered.SetActive(true);
    //                masterVolumeContainer8FilledHovered.SetActive(true);
    //                masterVolumeContainer9FilledHovered.SetActive(true);
    //                masterVolumeContainer10FilledHovered.SetActive(true);
    //            }
    //            else
    //            {
    //                masterVolumeContainer1Filled.SetActive(true);
    //                masterVolumeContainer2Filled.SetActive(true);
    //                masterVolumeContainer3Filled.SetActive(true);
    //                masterVolumeContainer4Filled.SetActive(true);
    //                masterVolumeContainer5Filled.SetActive(true);
    //                masterVolumeContainer6Filled.SetActive(true);
    //                masterVolumeContainer7Filled.SetActive(true);
    //                masterVolumeContainer8Filled.SetActive(true);
    //                masterVolumeContainer9Filled.SetActive(true);
    //                masterVolumeContainer10Filled.SetActive(true);
                     
    //                masterVolumeContainer1FilledHovered.SetActive(false);
    //                masterVolumeContainer2FilledHovered.SetActive(false);
    //                masterVolumeContainer3FilledHovered.SetActive(false);
    //                masterVolumeContainer4FilledHovered.SetActive(false);
    //                masterVolumeContainer5FilledHovered.SetActive(false);
    //                masterVolumeContainer6FilledHovered.SetActive(false);
    //                masterVolumeContainer7FilledHovered.SetActive(false);
    //                masterVolumeContainer8FilledHovered.SetActive(false);
    //                masterVolumeContainer9FilledHovered.SetActive(false);
    //                masterVolumeContainer10FilledHovered.SetActive(false);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}
};