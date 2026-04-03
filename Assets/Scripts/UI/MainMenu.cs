using System;
using Loopie;

class MainMenu : Component
{
    // Buttons
    public Entity newGameEntity;
    public Entity newGameHoveredEntity;
    //private Button newGameButton;
    private SceneTransition newGameScript;
    private Image newGameHoveredImage;

    public Entity continueEntity;
    public Entity continueHoveredEntity;
    //private Button continueButton;
    private Load continueScript;
    private Image continueHoveredImage;

    public Entity settingsEntity;
    public Entity settingsHoveredEntity;
    //private Button settingsButton;
    private SceneTransition settingsScript;
    private Image settingsHoveredImage;

    public Entity exitEntity;
    public Entity exitHoveredEntity;
    //private Button exitButton;
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

    // Internal
    public float inputCooldown = 0.2f;
    private float inputTimer = 0f;
    private float confirmTimer = 0f;
    private bool canCallScripts = false;

    void OnCreate()
    {
        // Buttons
        if (newGameEntity != null )
        {
            // button

            if (newGameHoveredEntity != null)
            {
                newGameScript = newGameHoveredEntity.GetComponent<SceneTransition>();
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
    }

    void OnUpdate()
    {
        inputTimer += Time.deltaTime;
        confirmTimer += Time.deltaTime;

        HandleNavigation();
        HandleConfirm();
    }

    void HandleNavigation()
    {
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
                case Buttons.NEW_GAME: currentButton = Buttons.EXIT; break;
                case Buttons.CONTINUE: currentButton = Buttons.NEW_GAME; break;
                case Buttons.SETTINGS: currentButton = Buttons.CONTINUE; break;
                case Buttons.EXIT: currentButton = Buttons.SETTINGS; break;
            }
            moved = true;
        }
        else if (Input.IsKeyPressed(KeyCode.DOWN) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN) || Input.LeftAxis.y < 0) 
        {
            switch (currentButton)
            {
                case Buttons.NEW_GAME: currentButton = Buttons.CONTINUE; break;
                case Buttons.CONTINUE: currentButton = Buttons.SETTINGS; break;
                case Buttons.SETTINGS: currentButton = Buttons.EXIT; break;
                case Buttons.EXIT: currentButton = Buttons.NEW_GAME; break;
            }
            moved = true;
        }

        // Visual Feedback
        switch (currentButton)
        {
            case Buttons.NEW_GAME:
                newGameHoveredEntity.SetActive(true);
                continueHoveredEntity.SetActive(false);
                settingsHoveredEntity.SetActive(false);
                exitHoveredEntity.SetActive(false);
                break;
            case Buttons.CONTINUE:
                newGameHoveredEntity.SetActive(false);
                continueHoveredEntity.SetActive(true);
                settingsHoveredEntity.SetActive(false);
                exitHoveredEntity.SetActive(false);
                break;
            case Buttons.SETTINGS:
                newGameHoveredEntity.SetActive(false);
                continueHoveredEntity.SetActive(false);
                settingsHoveredEntity.SetActive(true);
                exitHoveredEntity.SetActive(false);
                break;
            case Buttons.EXIT:
                newGameHoveredEntity.SetActive(false);
                continueHoveredEntity.SetActive(false);
                settingsHoveredEntity.SetActive(false);
                exitHoveredEntity.SetActive(true);
                break;
        }
        
        if (moved)
        {
            inputTimer = 0f;
        }
    }

    void HandleConfirm()
    {
        // Read Input
        if (Input.IsKeyPressed(KeyCode.KP_ENTER) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
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
        }
    }
};