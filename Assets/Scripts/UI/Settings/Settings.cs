using System;
using Loopie;

class Settings : Component
{
    [HideInInspector]
    public float enterCooldown = 0.1f;
    private float enterTimer = 0f;

    public Entity passPageEntity;
    private SpriteAnimator passPageAnimator;
    public Entity invertedPassPageEntity;
    private SpriteAnimator invertedPassPageAnimator;
    [HideInInspector]
    public static bool quickStartAnimations = true;
    [HideInInspector]
    public static bool invertedPassPagePlayed = false;

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

    // Master Volume Containers
    private Entity masterVolumeContainer1Filled;
    private Entity masterVolumeContainer1FilledHovered;
    private Entity masterVolumeContainer1EmptyHovered;

    private Entity masterVolumeContainer2Filled;
    private Entity masterVolumeContainer2FilledHovered;
    private Entity masterVolumeContainer2EmptyHovered;

    private Entity masterVolumeContainer3Filled;
    private Entity masterVolumeContainer3FilledHovered;
    private Entity masterVolumeContainer3EmptyHovered;

    private Entity masterVolumeContainer4Filled;
    private Entity masterVolumeContainer4FilledHovered;
    private Entity masterVolumeContainer4EmptyHovered;

    private Entity masterVolumeContainer5Filled;
    private Entity masterVolumeContainer5FilledHovered;
    private Entity masterVolumeContainer5EmptyHovered;

    private Entity masterVolumeContainer6Filled;
    private Entity masterVolumeContainer6FilledHovered;
    private Entity masterVolumeContainer6EmptyHovered;

    private Entity masterVolumeContainer7Filled;
    private Entity masterVolumeContainer7FilledHovered;
    private Entity masterVolumeContainer7EmptyHovered;

    private Entity masterVolumeContainer8Filled;
    private Entity masterVolumeContainer8FilledHovered;
    private Entity masterVolumeContainer8EmptyHovered;

    private Entity masterVolumeContainer9Filled;
    private Entity masterVolumeContainer9FilledHovered;
    private Entity masterVolumeContainer9EmptyHovered;

    private Entity masterVolumeContainer10Filled;
    private Entity masterVolumeContainer10FilledHovered;
    private Entity masterVolumeContainer10EmptyHovered;

    // Music Volume
    public Entity musicVolumeHoveredEntity;
    private Image musicVolumeHoveredImage;
    private Image musicVolumeLeftArrowHoveredImage;
    private Image musicVolumeRightArrowHoveredImage;
    private MusicVolume musicVolumeScript;
    private Text musicVolumeDisplayText;

    // Music Volume Containers
    private Entity musicVolumeContainer1Filled;
    private Entity musicVolumeContainer1FilledHovered;
    private Entity musicVolumeContainer1EmptyHovered;

    private Entity musicVolumeContainer2Filled;
    private Entity musicVolumeContainer2FilledHovered;
    private Entity musicVolumeContainer2EmptyHovered;

    private Entity musicVolumeContainer3Filled;
    private Entity musicVolumeContainer3FilledHovered;
    private Entity musicVolumeContainer3EmptyHovered;

    private Entity musicVolumeContainer4Filled;
    private Entity musicVolumeContainer4FilledHovered;
    private Entity musicVolumeContainer4EmptyHovered;

    private Entity musicVolumeContainer5Filled;
    private Entity musicVolumeContainer5FilledHovered;
    private Entity musicVolumeContainer5EmptyHovered;

    private Entity musicVolumeContainer6Filled;
    private Entity musicVolumeContainer6FilledHovered;
    private Entity musicVolumeContainer6EmptyHovered;

    private Entity musicVolumeContainer7Filled;
    private Entity musicVolumeContainer7FilledHovered;
    private Entity musicVolumeContainer7EmptyHovered;

    private Entity musicVolumeContainer8Filled;
    private Entity musicVolumeContainer8FilledHovered;
    private Entity musicVolumeContainer8EmptyHovered;

    private Entity musicVolumeContainer9Filled;
    private Entity musicVolumeContainer9FilledHovered;
    private Entity musicVolumeContainer9EmptyHovered;

    private Entity musicVolumeContainer10Filled;
    private Entity musicVolumeContainer10FilledHovered;
    private Entity musicVolumeContainer10EmptyHovered;

    // Sfx Volume
    public Entity sfxVolumeHoveredEntity;
    private Image sfxVolumeHoveredImage;
    private Image sfxVolumeLeftArrowHoveredImage;
    private Image sfxVolumeRightArrowHoveredImage;
    private SfxVolume sfxVolumeScript;
    private Text sfxVolumeDisplayText;

    // Sfx Volume Containers
    private Entity sfxVolumeContainer1Filled;
    private Entity sfxVolumeContainer1FilledHovered;
    private Entity sfxVolumeContainer1EmptyHovered;

    private Entity sfxVolumeContainer2Filled;
    private Entity sfxVolumeContainer2FilledHovered;
    private Entity sfxVolumeContainer2EmptyHovered;

    private Entity sfxVolumeContainer3Filled;
    private Entity sfxVolumeContainer3FilledHovered;
    private Entity sfxVolumeContainer3EmptyHovered;

    private Entity sfxVolumeContainer4Filled;
    private Entity sfxVolumeContainer4FilledHovered;
    private Entity sfxVolumeContainer4EmptyHovered;

    private Entity sfxVolumeContainer5Filled;
    private Entity sfxVolumeContainer5FilledHovered;
    private Entity sfxVolumeContainer5EmptyHovered;

    private Entity sfxVolumeContainer6Filled;
    private Entity sfxVolumeContainer6FilledHovered;
    private Entity sfxVolumeContainer6EmptyHovered;

    private Entity sfxVolumeContainer7Filled;
    private Entity sfxVolumeContainer7FilledHovered;
    private Entity sfxVolumeContainer7EmptyHovered;

    private Entity sfxVolumeContainer8Filled;
    private Entity sfxVolumeContainer8FilledHovered;
    private Entity sfxVolumeContainer8EmptyHovered;

    private Entity sfxVolumeContainer9Filled;
    private Entity sfxVolumeContainer9FilledHovered;
    private Entity sfxVolumeContainer9EmptyHovered;

    private Entity sfxVolumeContainer10Filled;
    private Entity sfxVolumeContainer10FilledHovered;
    private Entity sfxVolumeContainer10EmptyHovered;

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

        // Master Volume
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

            // Containers
            masterVolumeContainer1Filled = Entity.FindEntityByName("MasterVolume_Container1_Filled");
            masterVolumeContainer1FilledHovered = Entity.FindEntityByName("MasterVolume_Container1_Filled_Hovered");
            masterVolumeContainer1EmptyHovered = Entity.FindEntityByName("MasterVolume_Container1_Empty_Hovered");

            masterVolumeContainer2Filled = Entity.FindEntityByName("MasterVolume_Container2_Filled");
            masterVolumeContainer2FilledHovered = Entity.FindEntityByName("MasterVolume_Container2_Filled_Hovered");
            masterVolumeContainer2EmptyHovered = Entity.FindEntityByName("MasterVolume_Container2_Empty_Hovered");

            masterVolumeContainer3Filled = Entity.FindEntityByName("MasterVolume_Container3_Filled");
            masterVolumeContainer3FilledHovered = Entity.FindEntityByName("MasterVolume_Container3_Filled_Hovered");
            masterVolumeContainer3EmptyHovered = Entity.FindEntityByName("MasterVolume_Container3_Empty_Hovered");

            masterVolumeContainer4Filled = Entity.FindEntityByName("MasterVolume_Container4_Filled");
            masterVolumeContainer4FilledHovered = Entity.FindEntityByName("MasterVolume_Container4_Filled_Hovered");
            masterVolumeContainer4EmptyHovered = Entity.FindEntityByName("MasterVolume_Container4_Empty_Hovered");

            masterVolumeContainer5Filled = Entity.FindEntityByName("MasterVolume_Container5_Filled");
            masterVolumeContainer5FilledHovered = Entity.FindEntityByName("MasterVolume_Container5_Filled_Hovered");
            masterVolumeContainer5EmptyHovered = Entity.FindEntityByName("MasterVolume_Container5_Empty_Hovered");

            masterVolumeContainer6Filled = Entity.FindEntityByName("MasterVolume_Container6_Filled");
            masterVolumeContainer6FilledHovered = Entity.FindEntityByName("MasterVolume_Container6_Filled_Hovered");
            masterVolumeContainer6EmptyHovered = Entity.FindEntityByName("MasterVolume_Container6_Empty_Hovered");

            masterVolumeContainer7Filled = Entity.FindEntityByName("MasterVolume_Container7_Filled");
            masterVolumeContainer7FilledHovered = Entity.FindEntityByName("MasterVolume_Container7_Filled_Hovered");
            masterVolumeContainer7EmptyHovered = Entity.FindEntityByName("MasterVolume_Container7_Empty_Hovered");

            masterVolumeContainer8Filled = Entity.FindEntityByName("MasterVolume_Container8_Filled");
            masterVolumeContainer8FilledHovered = Entity.FindEntityByName("MasterVolume_Container8_Filled_Hovered");
            masterVolumeContainer8EmptyHovered = Entity.FindEntityByName("MasterVolume_Container8_Empty_Hovered");

            masterVolumeContainer9Filled = Entity.FindEntityByName("MasterVolume_Container9_Filled");
            masterVolumeContainer9FilledHovered = Entity.FindEntityByName("MasterVolume_Container9_Filled_Hovered");
            masterVolumeContainer9EmptyHovered = Entity.FindEntityByName("MasterVolume_Container9_Empty_Hovered");

            masterVolumeContainer10Filled = Entity.FindEntityByName("MasterVolume_Container10_Filled");
            masterVolumeContainer10FilledHovered = Entity.FindEntityByName("MasterVolume_Container10_Filled_Hovered");
            masterVolumeContainer10EmptyHovered = Entity.FindEntityByName("MasterVolume_Container10_Empty_Hovered");
        }
        else
        {
            Debug.Log("Error: There is no Master Volume Hovered Entity assigned.");
        }

        // Music Volume
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

            // Containers
            musicVolumeContainer1Filled = Entity.FindEntityByName("MusicVolume_Container1_Filled");
            musicVolumeContainer1FilledHovered = Entity.FindEntityByName("MusicVolume_Container1_Filled_Hovered");
            musicVolumeContainer1EmptyHovered = Entity.FindEntityByName("MusicVolume_Container1_Empty_Hovered");
             
            musicVolumeContainer2Filled = Entity.FindEntityByName("MusicVolume_Container2_Filled");
            musicVolumeContainer2FilledHovered = Entity.FindEntityByName("MusicVolume_Container2_Filled_Hovered");
            musicVolumeContainer2EmptyHovered = Entity.FindEntityByName("MusicVolume_Container2_Empty_Hovered");
             
            musicVolumeContainer3Filled = Entity.FindEntityByName("MusicVolume_Container3_Filled");
            musicVolumeContainer3FilledHovered = Entity.FindEntityByName("MusicVolume_Container3_Filled_Hovered");
            musicVolumeContainer3EmptyHovered = Entity.FindEntityByName("MusicVolume_Container3_Empty_Hovered");
             
            musicVolumeContainer4Filled = Entity.FindEntityByName("MusicVolume_Container4_Filled");
            musicVolumeContainer4FilledHovered = Entity.FindEntityByName("MusicVolume_Container4_Filled_Hovered");
            musicVolumeContainer4EmptyHovered = Entity.FindEntityByName("MusicVolume_Container4_Empty_Hovered");
             
            musicVolumeContainer5Filled = Entity.FindEntityByName("MusicVolume_Container5_Filled");
            musicVolumeContainer5FilledHovered = Entity.FindEntityByName("MusicVolume_Container5_Filled_Hovered");
            musicVolumeContainer5EmptyHovered = Entity.FindEntityByName("MusicVolume_Container5_Empty_Hovered");
             
            musicVolumeContainer6Filled = Entity.FindEntityByName("MusicVolume_Container6_Filled");
            musicVolumeContainer6FilledHovered = Entity.FindEntityByName("MusicVolume_Container6_Filled_Hovered");
            musicVolumeContainer6EmptyHovered = Entity.FindEntityByName("MusicVolume_Container6_Empty_Hovered");
             
            musicVolumeContainer7Filled = Entity.FindEntityByName("MusicVolume_Container7_Filled");
            musicVolumeContainer7FilledHovered = Entity.FindEntityByName("MusicVolume_Container7_Filled_Hovered");
            musicVolumeContainer7EmptyHovered = Entity.FindEntityByName("MusicVolume_Container7_Empty_Hovered");
             
            musicVolumeContainer8Filled = Entity.FindEntityByName("MusicVolume_Container8_Filled");
            musicVolumeContainer8FilledHovered = Entity.FindEntityByName("MusicVolume_Container8_Filled_Hovered");
            musicVolumeContainer8EmptyHovered = Entity.FindEntityByName("MusicVolume_Container8_Empty_Hovered");
             
            musicVolumeContainer9Filled = Entity.FindEntityByName("MusicVolume_Container9_Filled");
            musicVolumeContainer9FilledHovered = Entity.FindEntityByName("MusicVolume_Container9_Filled_Hovered");
            musicVolumeContainer9EmptyHovered = Entity.FindEntityByName("MusicVolume_Container9_Empty_Hovered");
             
            musicVolumeContainer10Filled = Entity.FindEntityByName("MusicVolume_Container10_Filled");
            musicVolumeContainer10FilledHovered = Entity.FindEntityByName("MusicVolume_Container10_Filled_Hovered");
            musicVolumeContainer10EmptyHovered = Entity.FindEntityByName("MusicVolume_Container10_Empty_Hovered");
        }
        else
        {
            Debug.Log("Error: There is no Music Volume Hovered Entity assigned.");
        }

        // Sfx Volume
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

            // Containers
            sfxVolumeContainer1Filled = Entity.FindEntityByName("SfxVolume_Container1_Filled");
            sfxVolumeContainer1FilledHovered = Entity.FindEntityByName("SfxVolume_Container1_Filled_Hovered");
            sfxVolumeContainer1EmptyHovered = Entity.FindEntityByName("SfxVolume_Container1_Empty_Hovered");
            
            sfxVolumeContainer2Filled = Entity.FindEntityByName("SfxVolume_Container2_Filled");
            sfxVolumeContainer2FilledHovered = Entity.FindEntityByName("SfxVolume_Container2_Filled_Hovered");
            sfxVolumeContainer2EmptyHovered = Entity.FindEntityByName("SfxVolume_Container2_Empty_Hovered");
            
            sfxVolumeContainer3Filled = Entity.FindEntityByName("SfxVolume_Container3_Filled");
            sfxVolumeContainer3FilledHovered = Entity.FindEntityByName("SfxVolume_Container3_Filled_Hovered");
            sfxVolumeContainer3EmptyHovered = Entity.FindEntityByName("SfxVolume_Container3_Empty_Hovered");
            
            sfxVolumeContainer4Filled = Entity.FindEntityByName("SfxVolume_Container4_Filled");
            sfxVolumeContainer4FilledHovered = Entity.FindEntityByName("SfxVolume_Container4_Filled_Hovered");
            sfxVolumeContainer4EmptyHovered = Entity.FindEntityByName("SfxVolume_Container4_Empty_Hovered");
            
            sfxVolumeContainer5Filled = Entity.FindEntityByName("SfxVolume_Container5_Filled");
            sfxVolumeContainer5FilledHovered = Entity.FindEntityByName("SfxVolume_Container5_Filled_Hovered");
            sfxVolumeContainer5EmptyHovered = Entity.FindEntityByName("SfxVolume_Container5_Empty_Hovered");
            
            sfxVolumeContainer6Filled = Entity.FindEntityByName("SfxVolume_Container6_Filled");
            sfxVolumeContainer6FilledHovered = Entity.FindEntityByName("SfxVolume_Container6_Filled_Hovered");
            sfxVolumeContainer6EmptyHovered = Entity.FindEntityByName("SfxVolume_Container6_Empty_Hovered");
            
            sfxVolumeContainer7Filled = Entity.FindEntityByName("SfxVolume_Container7_Filled");
            sfxVolumeContainer7FilledHovered = Entity.FindEntityByName("SfxVolume_Container7_Filled_Hovered");
            sfxVolumeContainer7EmptyHovered = Entity.FindEntityByName("SfxVolume_Container7_Empty_Hovered");
            
            sfxVolumeContainer8Filled = Entity.FindEntityByName("SfxVolume_Container8_Filled");
            sfxVolumeContainer8FilledHovered = Entity.FindEntityByName("SfxVolume_Container8_Filled_Hovered");
            sfxVolumeContainer8EmptyHovered = Entity.FindEntityByName("SfxVolume_Container8_Empty_Hovered");
            
            sfxVolumeContainer9Filled = Entity.FindEntityByName("SfxVolume_Container9_Filled");
            sfxVolumeContainer9FilledHovered = Entity.FindEntityByName("SfxVolume_Container9_Filled_Hovered");
            sfxVolumeContainer9EmptyHovered = Entity.FindEntityByName("SfxVolume_Container9_Empty_Hovered");
            
            sfxVolumeContainer10Filled = Entity.FindEntityByName("SfxVolume_Container10_Filled");
            sfxVolumeContainer10FilledHovered = Entity.FindEntityByName("SfxVolume_Container10_Filled_Hovered");
            sfxVolumeContainer10EmptyHovered = Entity.FindEntityByName("SfxVolume_Container10_Empty_Hovered");
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
            }
        }
        
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

        HandleVisualFeedback();

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
                masterVolumeScript.ApplyMasterVolume();
                break;
            case Buttons.MUSIC_VOLUME:
                musicVolumeScript.IncreaseVolume();
                float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
                musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
                musicVolumeScript.ApplyMusicVolume();
                break;
            case Buttons.SFX_VOLUME:
                sfxVolumeScript.IncreaseVolume();
                float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
                sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
                sfxVolumeScript.ApplySfxVolume();
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
                masterVolumeScript.ApplyMasterVolume();
                break;
            case Buttons.MUSIC_VOLUME:
                musicVolumeScript.DecreaseVolume();
                float musicVolume = Mathf.Abs(musicVolumeScript.GetVolume() * 100);
                musicVolumeDisplayText.SetText(Mathf.Round(musicVolume).ToString());
                musicVolumeScript.ApplyMusicVolume();
                break;
            case Buttons.SFX_VOLUME:
                sfxVolumeScript.DecreaseVolume();
                float sfxVolume = Mathf.Abs(sfxVolumeScript.GetVolume() * 100);
                sfxVolumeDisplayText.SetText(Mathf.Round(sfxVolume).ToString());
                sfxVolumeScript.ApplySfxVolume();
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
        HandleSfxContainers();
        HandleMusicContainers();
        HandleMasterContainers();
    }

    void HandleSfxContainers()
    {
        if (currentButton == Buttons.SFX_VOLUME)
        {
            sfxVolumeContainer1EmptyHovered.SetActive(true);
            sfxVolumeContainer2EmptyHovered.SetActive(true);
            sfxVolumeContainer3EmptyHovered.SetActive(true);
            sfxVolumeContainer4EmptyHovered.SetActive(true);
            sfxVolumeContainer5EmptyHovered.SetActive(true);
            sfxVolumeContainer6EmptyHovered.SetActive(true);
            sfxVolumeContainer7EmptyHovered.SetActive(true);
            sfxVolumeContainer8EmptyHovered.SetActive(true);
            sfxVolumeContainer9EmptyHovered.SetActive(true);
            sfxVolumeContainer10EmptyHovered.SetActive(true);
        }
        else
        {
            sfxVolumeContainer1EmptyHovered.SetActive(false);
            sfxVolumeContainer2EmptyHovered.SetActive(false);
            sfxVolumeContainer3EmptyHovered.SetActive(false);
            sfxVolumeContainer4EmptyHovered.SetActive(false);
            sfxVolumeContainer5EmptyHovered.SetActive(false);
            sfxVolumeContainer6EmptyHovered.SetActive(false);
            sfxVolumeContainer7EmptyHovered.SetActive(false);
            sfxVolumeContainer8EmptyHovered.SetActive(false);
            sfxVolumeContainer9EmptyHovered.SetActive(false);
            sfxVolumeContainer10EmptyHovered.SetActive(false);
        }

        int sfxVolume = (int)(sfxVolumeScript.GetVolume() * 100);

        switch (sfxVolume)
        {
            case 0:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(false);
                    sfxVolumeContainer2Filled.SetActive(false);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(false);
                    sfxVolumeContainer2Filled.SetActive(false);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 10:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(false);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(false);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 20:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(false);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 30:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(false);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 40:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(false);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 50:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(false);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 60:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(true);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(false);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 70:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(true);
                    sfxVolumeContainer7FilledHovered.SetActive(true);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(false);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 80:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(true);
                    sfxVolumeContainer7FilledHovered.SetActive(true);
                    sfxVolumeContainer8FilledHovered.SetActive(true);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(false);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 90:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(true);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(true);
                    sfxVolumeContainer7FilledHovered.SetActive(true);
                    sfxVolumeContainer8FilledHovered.SetActive(true);
                    sfxVolumeContainer9FilledHovered.SetActive(true);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(true);
                    sfxVolumeContainer10Filled.SetActive(false);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 100:
                if (currentButton == Buttons.SFX_VOLUME)
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(true);
                    sfxVolumeContainer10Filled.SetActive(true);

                    sfxVolumeContainer1FilledHovered.SetActive(true);
                    sfxVolumeContainer2FilledHovered.SetActive(true);
                    sfxVolumeContainer3FilledHovered.SetActive(true);
                    sfxVolumeContainer4FilledHovered.SetActive(true);
                    sfxVolumeContainer5FilledHovered.SetActive(true);
                    sfxVolumeContainer6FilledHovered.SetActive(true);
                    sfxVolumeContainer7FilledHovered.SetActive(true);
                    sfxVolumeContainer8FilledHovered.SetActive(true);
                    sfxVolumeContainer9FilledHovered.SetActive(true);
                    sfxVolumeContainer10FilledHovered.SetActive(true);
                }
                else
                {
                    sfxVolumeContainer1Filled.SetActive(true);
                    sfxVolumeContainer2Filled.SetActive(true);
                    sfxVolumeContainer3Filled.SetActive(true);
                    sfxVolumeContainer4Filled.SetActive(true);
                    sfxVolumeContainer5Filled.SetActive(true);
                    sfxVolumeContainer6Filled.SetActive(true);
                    sfxVolumeContainer7Filled.SetActive(true);
                    sfxVolumeContainer8Filled.SetActive(true);
                    sfxVolumeContainer9Filled.SetActive(true);
                    sfxVolumeContainer10Filled.SetActive(true);

                    sfxVolumeContainer1FilledHovered.SetActive(false);
                    sfxVolumeContainer2FilledHovered.SetActive(false);
                    sfxVolumeContainer3FilledHovered.SetActive(false);
                    sfxVolumeContainer4FilledHovered.SetActive(false);
                    sfxVolumeContainer5FilledHovered.SetActive(false);
                    sfxVolumeContainer6FilledHovered.SetActive(false);
                    sfxVolumeContainer7FilledHovered.SetActive(false);
                    sfxVolumeContainer8FilledHovered.SetActive(false);
                    sfxVolumeContainer9FilledHovered.SetActive(false);
                    sfxVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    void HandleMusicContainers()
    {
        if (currentButton == Buttons.MUSIC_VOLUME)
        {
            musicVolumeContainer1EmptyHovered.SetActive(true);
            musicVolumeContainer2EmptyHovered.SetActive(true);
            musicVolumeContainer3EmptyHovered.SetActive(true);
            musicVolumeContainer4EmptyHovered.SetActive(true);
            musicVolumeContainer5EmptyHovered.SetActive(true);
            musicVolumeContainer6EmptyHovered.SetActive(true);
            musicVolumeContainer7EmptyHovered.SetActive(true);
            musicVolumeContainer8EmptyHovered.SetActive(true);
            musicVolumeContainer9EmptyHovered.SetActive(true);
            musicVolumeContainer10EmptyHovered.SetActive(true);
        }
        else
        {
            musicVolumeContainer1EmptyHovered.SetActive(false);
            musicVolumeContainer2EmptyHovered.SetActive(false);
            musicVolumeContainer3EmptyHovered.SetActive(false);
            musicVolumeContainer4EmptyHovered.SetActive(false);
            musicVolumeContainer5EmptyHovered.SetActive(false);
            musicVolumeContainer6EmptyHovered.SetActive(false);
            musicVolumeContainer7EmptyHovered.SetActive(false);
            musicVolumeContainer8EmptyHovered.SetActive(false);
            musicVolumeContainer9EmptyHovered.SetActive(false);
            musicVolumeContainer10EmptyHovered.SetActive(false);
        }

        int musicVolume = (int)(musicVolumeScript.GetVolume() * 100);

        switch (musicVolume)
        {
            case 0:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(false);
                    musicVolumeContainer2Filled.SetActive(false);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(false);
                    musicVolumeContainer2Filled.SetActive(false);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 10:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(false);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(false);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 20:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(false);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 30:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(false);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 40:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(false);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 50:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(false);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 60:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(true);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(false);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 70:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(true);
                    musicVolumeContainer7FilledHovered.SetActive(true);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(false);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 80:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(true);
                    musicVolumeContainer7FilledHovered.SetActive(true);
                    musicVolumeContainer8FilledHovered.SetActive(true);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(false);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 90:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(true);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(true);
                    musicVolumeContainer7FilledHovered.SetActive(true);
                    musicVolumeContainer8FilledHovered.SetActive(true);
                    musicVolumeContainer9FilledHovered.SetActive(true);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(true);
                    musicVolumeContainer10Filled.SetActive(false);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 100:
                if (currentButton == Buttons.MUSIC_VOLUME)
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(true);
                    musicVolumeContainer10Filled.SetActive(true);

                    musicVolumeContainer1FilledHovered.SetActive(true);
                    musicVolumeContainer2FilledHovered.SetActive(true);
                    musicVolumeContainer3FilledHovered.SetActive(true);
                    musicVolumeContainer4FilledHovered.SetActive(true);
                    musicVolumeContainer5FilledHovered.SetActive(true);
                    musicVolumeContainer6FilledHovered.SetActive(true);
                    musicVolumeContainer7FilledHovered.SetActive(true);
                    musicVolumeContainer8FilledHovered.SetActive(true);
                    musicVolumeContainer9FilledHovered.SetActive(true);
                    musicVolumeContainer10FilledHovered.SetActive(true);
                }
                else
                {
                    musicVolumeContainer1Filled.SetActive(true);
                    musicVolumeContainer2Filled.SetActive(true);
                    musicVolumeContainer3Filled.SetActive(true);
                    musicVolumeContainer4Filled.SetActive(true);
                    musicVolumeContainer5Filled.SetActive(true);
                    musicVolumeContainer6Filled.SetActive(true);
                    musicVolumeContainer7Filled.SetActive(true);
                    musicVolumeContainer8Filled.SetActive(true);
                    musicVolumeContainer9Filled.SetActive(true);
                    musicVolumeContainer10Filled.SetActive(true);

                    musicVolumeContainer1FilledHovered.SetActive(false);
                    musicVolumeContainer2FilledHovered.SetActive(false);
                    musicVolumeContainer3FilledHovered.SetActive(false);
                    musicVolumeContainer4FilledHovered.SetActive(false);
                    musicVolumeContainer5FilledHovered.SetActive(false);
                    musicVolumeContainer6FilledHovered.SetActive(false);
                    musicVolumeContainer7FilledHovered.SetActive(false);
                    musicVolumeContainer8FilledHovered.SetActive(false);
                    musicVolumeContainer9FilledHovered.SetActive(false);
                    musicVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    void HandleMasterContainers()
    {
        if (currentButton == Buttons.MASTER_VOLUME)
        {
            masterVolumeContainer1EmptyHovered.SetActive(true);
            masterVolumeContainer2EmptyHovered.SetActive(true);
            masterVolumeContainer3EmptyHovered.SetActive(true);
            masterVolumeContainer4EmptyHovered.SetActive(true);
            masterVolumeContainer5EmptyHovered.SetActive(true);
            masterVolumeContainer6EmptyHovered.SetActive(true);
            masterVolumeContainer7EmptyHovered.SetActive(true);
            masterVolumeContainer8EmptyHovered.SetActive(true);
            masterVolumeContainer9EmptyHovered.SetActive(true);
            masterVolumeContainer10EmptyHovered.SetActive(true);
        }
        else
        {
            masterVolumeContainer1EmptyHovered.SetActive(false);
            masterVolumeContainer2EmptyHovered.SetActive(false);
            masterVolumeContainer3EmptyHovered.SetActive(false);
            masterVolumeContainer4EmptyHovered.SetActive(false);
            masterVolumeContainer5EmptyHovered.SetActive(false);
            masterVolumeContainer6EmptyHovered.SetActive(false);
            masterVolumeContainer7EmptyHovered.SetActive(false);
            masterVolumeContainer8EmptyHovered.SetActive(false);
            masterVolumeContainer9EmptyHovered.SetActive(false);
            masterVolumeContainer10EmptyHovered.SetActive(false);
        }

        int masterVolume = (int)(masterVolumeScript.GetVolume() * 100);

        switch (masterVolume)
        {
            case 0:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(false);
                    masterVolumeContainer2Filled.SetActive(false);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(false);
                    masterVolumeContainer2Filled.SetActive(false);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 10:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(false);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(false);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 20:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(false);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 30:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(false);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 40:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(false);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 50:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(false);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 60:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(true);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(false);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 70:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(true);
                    masterVolumeContainer7FilledHovered.SetActive(true);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(false);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 80:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(true);
                    masterVolumeContainer7FilledHovered.SetActive(true);
                    masterVolumeContainer8FilledHovered.SetActive(true);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(false);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 90:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(true);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(true);
                    masterVolumeContainer7FilledHovered.SetActive(true);
                    masterVolumeContainer8FilledHovered.SetActive(true);
                    masterVolumeContainer9FilledHovered.SetActive(true);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(true);
                    masterVolumeContainer10Filled.SetActive(false);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            case 100:
                if (currentButton == Buttons.MASTER_VOLUME)
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(true);
                    masterVolumeContainer10Filled.SetActive(true);
                     
                    masterVolumeContainer1FilledHovered.SetActive(true);
                    masterVolumeContainer2FilledHovered.SetActive(true);
                    masterVolumeContainer3FilledHovered.SetActive(true);
                    masterVolumeContainer4FilledHovered.SetActive(true);
                    masterVolumeContainer5FilledHovered.SetActive(true);
                    masterVolumeContainer6FilledHovered.SetActive(true);
                    masterVolumeContainer7FilledHovered.SetActive(true);
                    masterVolumeContainer8FilledHovered.SetActive(true);
                    masterVolumeContainer9FilledHovered.SetActive(true);
                    masterVolumeContainer10FilledHovered.SetActive(true);
                }
                else
                {
                    masterVolumeContainer1Filled.SetActive(true);
                    masterVolumeContainer2Filled.SetActive(true);
                    masterVolumeContainer3Filled.SetActive(true);
                    masterVolumeContainer4Filled.SetActive(true);
                    masterVolumeContainer5Filled.SetActive(true);
                    masterVolumeContainer6Filled.SetActive(true);
                    masterVolumeContainer7Filled.SetActive(true);
                    masterVolumeContainer8Filled.SetActive(true);
                    masterVolumeContainer9Filled.SetActive(true);
                    masterVolumeContainer10Filled.SetActive(true);
                     
                    masterVolumeContainer1FilledHovered.SetActive(false);
                    masterVolumeContainer2FilledHovered.SetActive(false);
                    masterVolumeContainer3FilledHovered.SetActive(false);
                    masterVolumeContainer4FilledHovered.SetActive(false);
                    masterVolumeContainer5FilledHovered.SetActive(false);
                    masterVolumeContainer6FilledHovered.SetActive(false);
                    masterVolumeContainer7FilledHovered.SetActive(false);
                    masterVolumeContainer8FilledHovered.SetActive(false);
                    masterVolumeContainer9FilledHovered.SetActive(false);
                    masterVolumeContainer10FilledHovered.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
};