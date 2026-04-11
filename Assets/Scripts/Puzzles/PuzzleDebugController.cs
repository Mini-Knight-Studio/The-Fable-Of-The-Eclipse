using System;
using Loopie;

class PuzzleDebugController : Component
{

    void OnUpdate()
    {
        if (Input.IsKeyDown(KeyCode.NUM_1))
        {
            GlobalDatabase.Data.Puzzles.Puzzle1Completed = !GlobalDatabase.Data.Puzzles.Puzzle1Completed;
            Debug.LogWarning("Completed Puzzle 1 = " + GlobalDatabase.Data.Puzzles.Puzzle1Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_2))
        {
            GlobalDatabase.Data.Puzzles.Puzzle2Completed = !GlobalDatabase.Data.Puzzles.Puzzle2Completed;
            Debug.LogWarning("Completed Puzzle 2 = " + GlobalDatabase.Data.Puzzles.Puzzle2Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_3))
        {
            GlobalDatabase.Data.Puzzles.Puzzle3Completed = !GlobalDatabase.Data.Puzzles.Puzzle3Completed;
            Debug.LogWarning("Completed Puzzle 3 = " + GlobalDatabase.Data.Puzzles.Puzzle3Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_4))
        {
            GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted = !GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted;
            Debug.LogWarning("Completed All Puzzles = " + GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted);
            GlobalDatabase.Data.Puzzles.Puzzle1Completed = !GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted;
            GlobalDatabase.Data.Puzzles.Puzzle2Completed = !GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted;
            GlobalDatabase.Data.Puzzles.Puzzle3Completed = !GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted;
        }
        if (Input.IsKeyDown(KeyCode.NUM_5))
        {
            GlobalDatabase.Data.Player.hasBurner = !GlobalDatabase.Data.Player.hasBurner;
            Debug.LogWarning("Player has Burner = " + GlobalDatabase.Data.Player.hasBurner);
        }
        if (Input.IsKeyDown(KeyCode.NUM_6))
        {
            GlobalDatabase.Data.Player.hasGrappling = !GlobalDatabase.Data.Player.hasGrappling;
            Debug.LogWarning("Player has Grappling = " + GlobalDatabase.Data.Player.hasGrappling);
        }
        if (Input.IsKeyDown(KeyCode.NUM_7))
        {
            GlobalDatabase.Data.Player.gemEarthCollected = !GlobalDatabase.Data.Player.gemEarthCollected;
            Debug.LogWarning("Player has Earth Gem = " + GlobalDatabase.Data.Player.gemEarthCollected);
        }
        if (Input.IsKeyDown(KeyCode.NUM_8))
        {
            GlobalDatabase.Data.Player.gemWaterCollected = !GlobalDatabase.Data.Player.gemWaterCollected;
            Debug.LogWarning("Player has Water Gem = " + GlobalDatabase.Data.Player.gemWaterCollected);
        }
        if (Input.IsKeyDown(KeyCode.NUM_9))
        {
            GlobalDatabase.Data.Player.gemFireCollected = !GlobalDatabase.Data.Player.gemFireCollected;
            Debug.LogWarning("Player has Fire Gem = " + GlobalDatabase.Data.Player.gemFireCollected);
        }

        if (Input.IsKeyDown(KeyCode.NUM_0))
        {
            Debug.Log("Saving to Global Database");
            GlobalDatabase.Data.Save();
        }
    }
}