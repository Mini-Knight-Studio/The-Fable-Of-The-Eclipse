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
        }

        if (Input.IsKeyDown(KeyCode.NUM_0))
        {
            Debug.Log("Saving to Global Database");
            GlobalDatabase.Data.Save();
        }
    }
}