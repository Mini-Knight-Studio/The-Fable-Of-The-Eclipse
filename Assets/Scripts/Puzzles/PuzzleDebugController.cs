using System;
using Loopie;

class PuzzleDebugController : Component
{

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
}