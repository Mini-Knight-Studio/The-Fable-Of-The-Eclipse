using System;
using Loopie;

class MainMenu : Component
{
    // Buttons
    public Entity newGameEntity;
    //private Button newGameButton;
    private SceneTransition newGameScript;

    public Entity continueEntity;
    //private Button continueButton;
    private Load continueScript;

    public Entity settingsEntity;
    //private Button settingsButton;
    private SceneTransition settingsScript;
    
    public Entity exitEntity;
    //private Button exitButton;
    private Exit exitScript;

    private enum Buttons
    {
        NEW_GAME,
        CONTINUE,
        SETTINGS,
        EXIT
    }
    private Buttons currentButton = Buttons.NEW_GAME;

    // Internal
    float inputCooldown = 0.2f;
    float inputTimer = 0f;

    void OnCreate()
    {
        // Buttons
        if (newGameEntity != null )
        {
            // button

            Entity textNewGameEntity = Entity.FindEntityByName("Text_NewGame");
            if (textNewGameEntity != null)
            {
                newGameScript = textNewGameEntity.GetComponent<SceneTransition>();
            }
            else
            {
                Debug.Log("Error: There is no Text_NewGame Entity.");
            }
        }
        else
        {
            Debug.Log("Error: There is no NewGame Entity assigned.");
        }

        if (continueEntity != null)
        {
            // button

            Entity textContinueEntity = Entity.FindEntityByName("Text_Continue");
            if (textContinueEntity != null)
            {
                continueScript = textContinueEntity.GetComponent<Load>();
            }
            else
            {
                Debug.Log("Error: There is no Text_Continue Entity.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Continue Entity assigned.");
        }

        if (settingsEntity != null)
        {
            // button

            Entity textSettingsEntity = Entity.FindEntityByName("Text_Settings");
            if (textSettingsEntity != null)
            {
                settingsScript = textSettingsEntity.GetComponent<SceneTransition>();
            }
            else
            {
                Debug.Log("Error: There is no Text_Settings Entity.");
            }
        }
        else
        {
            Debug.Log("Error: There is no Settings Entity assigned.");
        }

        if (exitEntity != null)
        {
            // button

            Entity textExitEntity = Entity.FindEntityByName("Text_Exit");
            if (textExitEntity != null)
            {
                exitScript = textExitEntity.GetComponent<Exit>();
            }
            else
            {
                Debug.Log("Error: There is no Text_Exit Entity.");
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

        HandleNavigation();
        HandleConfirm();
    }

    void HandleNavigation()
    {
        // Cooldown
        if (inputTimer < inputCooldown)
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

        if (moved)
        {
            inputTimer = 0f;
        }
    }

    void HandleConfirm()
    {
        if (Input.IsKeyPressed(KeyCode.KP_ENTER) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
        {
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