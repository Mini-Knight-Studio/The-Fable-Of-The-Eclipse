using System;
using Loopie;

class PuzzleDebugController : Component
{
    void OnCreate()
    {
        PuzzleProgressionManager.Initialize();
    }

    void OnUpdate()
    {
        if (Input.IsKeyDown(KeyCode.NUM_1))
        {
            PuzzleProgressionManager.runtimePuzzleData.Puzzle1Completed = !PuzzleProgressionManager.runtimePuzzleData.Puzzle1Completed;
            Debug.LogWarning("Completed Puzzle 1 = " + PuzzleProgressionManager.runtimePuzzleData.Puzzle1Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_2))
        {
            PuzzleProgressionManager.runtimePuzzleData.Puzzle2Completed = !PuzzleProgressionManager.runtimePuzzleData.Puzzle2Completed;
            Debug.LogWarning("Completed Puzzle 2 = " + PuzzleProgressionManager.runtimePuzzleData.Puzzle2Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_3))
        {
            PuzzleProgressionManager.runtimePuzzleData.Puzzle3Completed = !PuzzleProgressionManager.runtimePuzzleData.Puzzle3Completed;
            Debug.LogWarning("Completed Puzzle 3 = " + PuzzleProgressionManager.runtimePuzzleData.Puzzle3Completed);
        }
        if (Input.IsKeyDown(KeyCode.NUM_4))
        {
            PuzzleProgressionManager.runtimePuzzleData.AllPuzzlesCompleted = !PuzzleProgressionManager.runtimePuzzleData.AllPuzzlesCompleted;
            Debug.LogWarning("Completed All Puzzles = " + PuzzleProgressionManager.runtimePuzzleData.AllPuzzlesCompleted);
        }

        if (Input.IsKeyDown(KeyCode.NUM_0))
        {
            Debug.Log("Saving to Manager");
            PuzzleProgressionManager.SaveChanges();
        }
    }
}