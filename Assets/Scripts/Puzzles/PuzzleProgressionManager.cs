using System;
using Loopie;

public static class PuzzleProgressionManager
{
    public static PuzzlesData runtimePuzzleData;

    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized) return;

        runtimePuzzleData = new PuzzlesData();

        GlobalDatabase.Data.Load();

        runtimePuzzleData.Puzzle1Completed = GlobalDatabase.Data.Puzzles.Puzzle1Completed;
        runtimePuzzleData.Puzzle2Completed = GlobalDatabase.Data.Puzzles.Puzzle2Completed;
        runtimePuzzleData.Puzzle3Completed = GlobalDatabase.Data.Puzzles.Puzzle3Completed;
        runtimePuzzleData.AllPuzzlesCompleted = GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted;

        initialized = true;

        Debug.Log("- Initialising GlobalDatabase to PuzzleProgressionManager -");
    }

    public static void SaveChanges()
    {
        GlobalDatabase.Data.Puzzles.Puzzle1Completed = runtimePuzzleData.Puzzle1Completed;
        GlobalDatabase.Data.Puzzles.Puzzle2Completed = runtimePuzzleData.Puzzle2Completed;
        GlobalDatabase.Data.Puzzles.Puzzle3Completed = runtimePuzzleData.Puzzle3Completed;
        GlobalDatabase.Data.Puzzles.AllPuzzlesCompleted = runtimePuzzleData.AllPuzzlesCompleted;

        GlobalDatabase.Data.Save();

        Debug.Log("- Saving PuzzleProgressionManager to GlobalDatabase -");
    }
}