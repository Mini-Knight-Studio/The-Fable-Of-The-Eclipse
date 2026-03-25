using System;
using Loopie;

public class PuzzlesDataBase : LocalDatabase
{
    public PuzzlesDataBase(string name) : base(name) { }

    public bool Puzzle1Completed;
    public bool Puzzle2Completed;
    public bool Puzzle3Completed;
    public bool AllPuzzlesCompleted;
}