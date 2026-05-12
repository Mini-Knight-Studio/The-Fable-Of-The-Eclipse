using System;
using Loopie;

public static class GameManager
{
    public enum GameState
    {
        DEFAULT,
        PAUSE,
        FREEZE
    }

    public static GameState state { get; private set; } = GameState.DEFAULT;
    
    public static void SetState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.DEFAULT:
                Time.timeScale = 1;
                break;
            case GameState.PAUSE:
                Time.timeScale = 1;
                break;
            case GameState.FREEZE:
                Time.timeScale = 0;
                break;
            default:
                break;  
        }
        state = gameState;
    }
};