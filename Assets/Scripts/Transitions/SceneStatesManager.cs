using System;
using Loopie;

public static class SceneStatesManager
{
    private static string currentSceneUUID = "";
    private static string previousSceneUUID = "";

    public static string GetPreviousSceneUUID()
    {
        return previousSceneUUID;
    }

    public static void SetCurrentScene(string newSceneUUID)
    {
        if(currentSceneUUID == newSceneUUID) { return; }
        previousSceneUUID = currentSceneUUID;
        currentSceneUUID = newSceneUUID;
    }
}