using System;
using Loopie;

public class GameDebug : Component
{
    public Entity debugMenuEntity;
    public Entity pauseMenuEntity;
    private PauseMenu pauseMenuScript;

    public bool debugMenuActive = false;

    public bool pauseTransparencyActive = false;
    
    void OnCreate()
    {
        if (pauseMenuEntity != null)
        {
            pauseMenuScript = pauseMenuEntity.GetComponent<PauseMenu>();
        }
        else
        {
            Debug.Log("Error: There is no passPageEntity Entity assigned.");
        }
    }
    void OnUpdate()
    {
        if (Input.IsKeyDown(KeyCode.NUM_1))
        {
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed;
            Debug.LogWarning("Completed Puzzle 1 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_2))
        {
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed;
            Debug.LogWarning("Completed Puzzle 2 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_3))
        {
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed;
            Debug.LogWarning("Completed Puzzle 3 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_4))
        {
            DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
            Debug.LogWarning("Completed All Puzzles = " + DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted);
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
        }
        if (Input.IsKeyDown(KeyCode.NUM_5))
        {
            DatabaseRegistry.playerDB.Player.hasBurner = !DatabaseRegistry.playerDB.Player.hasBurner;
            Debug.LogWarning("Player has Burner = " + DatabaseRegistry.playerDB.Player.hasBurner);
        }
        if (Input.IsKeyDown(KeyCode.NUM_6))
        {
            DatabaseRegistry.playerDB.Player.hasGrappling = !DatabaseRegistry.playerDB.Player.hasGrappling;
            Debug.LogWarning("Player has Grappling = " + DatabaseRegistry.playerDB.Player.hasGrappling);
        }
        if (Input.IsKeyDown(KeyCode.NUM_7))
        {
            DatabaseRegistry.playerDB.Player.gemAirCollected = !DatabaseRegistry.playerDB.Player.gemAirCollected;
            Debug.LogWarning("Player has Earth Gem = " + DatabaseRegistry.playerDB.Player.gemAirCollected);
        }
        if (Input.IsKeyDown(KeyCode.NUM_8))
        {
            DatabaseRegistry.playerDB.Player.gemWaterCollected = !DatabaseRegistry.playerDB.Player.gemWaterCollected;
            Debug.LogWarning("Player has Water Gem = " + DatabaseRegistry.playerDB.Player.gemWaterCollected);
        }
        if (Input.IsKeyDown(KeyCode.NUM_9))
        {
            DatabaseRegistry.playerDB.Player.gemFireCollected = !DatabaseRegistry.playerDB.Player.gemFireCollected;
            Debug.LogWarning("Player has Fire Gem = " + DatabaseRegistry.playerDB.Player.gemFireCollected);
        }

        if (Input.IsKeyDown(KeyCode.NUM_0))
        {
            Debug.Log("Saving to Global Database");
            DatabaseRegistry.playerDB.Save();
        }
    }
    public void ToggleDebugMenu() 
    {
        debugMenuActive = !debugMenuActive;
        debugMenuEntity.SetActive(debugMenuActive);
    }
    public void TogglePauseTransparency() 
    {
        pauseTransparencyActive = !pauseTransparencyActive;
        if (pauseTransparencyActive)
        {
            Vector4 lowOpacityColor = new Vector4(1, 1, 1, 0.25f);
            pauseMenuScript.backgroundImage.SetTint(lowOpacityColor);
            pauseMenuScript.ilustrationImage.SetTint(lowOpacityColor);
        }
        else
        {
            Vector4 fullOpacityColor = new Vector4(1, 1, 1, 1);
            pauseMenuScript.backgroundImage.SetTint(fullOpacityColor);
            pauseMenuScript.ilustrationImage.SetTint(fullOpacityColor);
        }
    }
    public void CompletePuzzle1()
    {
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed;
        Debug.LogWarning("Completed Puzzle 1 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed);
    }
    public void CompletePuzzle2()
    {
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed;
        Debug.LogWarning("Completed Puzzle 2 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed);
    }
    public void CompletePuzzle3()
    {
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed = !DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed;
        Debug.LogWarning("Completed Puzzle 3 = " + DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed);
    }
    public void CompleteAllPuzzles()
    {
        DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
        Debug.LogWarning("Completed All Puzzles = " + DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted);
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed = !DatabaseRegistry.puzzlesDB.Puzzles.AllPuzzlesCompleted;
    }
    public void ToggleBurner()
    {
        DatabaseRegistry.playerDB.Player.hasBurner = !DatabaseRegistry.playerDB.Player.hasBurner;
        Debug.LogWarning("Player has Burner = " + DatabaseRegistry.playerDB.Player.hasBurner);
    }
    public void ToggleGrapple()
    {
        DatabaseRegistry.playerDB.Player.hasGrappling = !DatabaseRegistry.playerDB.Player.hasGrappling;
        Debug.LogWarning("Player has Grappling = " + DatabaseRegistry.playerDB.Player.hasGrappling);
    }
    public void CollectAirGem()
    {
        DatabaseRegistry.playerDB.Player.gemAirCollected = !DatabaseRegistry.playerDB.Player.gemAirCollected;
        Debug.LogWarning("Player has Earth Gem = " + DatabaseRegistry.playerDB.Player.gemAirCollected);
    }
    public void CollectWaterGem()
    {
        DatabaseRegistry.playerDB.Player.gemWaterCollected = !DatabaseRegistry.playerDB.Player.gemWaterCollected;
        Debug.LogWarning("Player has Water Gem = " + DatabaseRegistry.playerDB.Player.gemWaterCollected);
    }
    public void CollectFireGem()
    {
        DatabaseRegistry.playerDB.Player.gemFireCollected = !DatabaseRegistry.playerDB.Player.gemFireCollected;
        Debug.LogWarning("Player has Fire Gem = " + DatabaseRegistry.playerDB.Player.gemFireCollected);
    }
    public void SaveGame()
    {
        Debug.Log("Saving current game to Global Database");
        DatabaseRegistry.SaveAll();
    }
    public void ToggleGodmode()
    {
        Player.Instance.Movement.isGodMode = !Player.Instance.Movement.isGodMode;
        Debug.LogWarning("Player is on Godmode = " + Player.Instance.Movement.isGodMode);
    }
};