using System;
using Loopie;

class Firepath_InitialPlayerPositionManager : Component
{
    public string firepathUUID;
    public string puzzleSceneUUID;
    public string level2SceneUUID;

    public Entity fromPuzzleReference;
    public Entity fromLvl2Reference;

    void OnCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(firepathUUID);

        float previousY = Player.Instance.transform.position.y;

        if (DatabaseRegistry.playerDB.Player.previousSceneUUID == puzzleSceneUUID)
        {
            var pos = fromPuzzleReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromPuzzleReference.transform.rotation;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == level2SceneUUID)
        {
            var pos = fromLvl2Reference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromLvl2Reference.transform.rotation;
        }
    }

    //void OnDrawGizmo()
    //{
    //    if (fromPuzzleReference != null)
    //    {
    //        Gizmo.DrawLine(fromPuzzleReference.transform.position, fromPuzzleReference.transform.Forward, Color.Green);
    //    }

    //    if (fromLvl2Reference != null)
    //    {
    //        Gizmo.DrawLine(fromLvl2Reference.transform.position, fromLvl2Reference.transform.Forward, Color.Green);
    //    }
    //}
}