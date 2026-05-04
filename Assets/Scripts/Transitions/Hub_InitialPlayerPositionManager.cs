using System;
using Loopie;

class Hub_InitialPlayerPositionManager : Component
{
    public string level1SceneUUID;
    public string level2SceneUUID;
    public string waterpathUUID;
    public string firepathUUID;

    public Entity fromLvl1Reference;
    public Entity fromWaterpathReference;
    public Entity fromFirepathReference;

    void OnCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(level2SceneUUID);

        float previousY = Player.Instance.transform.position.y;

        if (DatabaseRegistry.playerDB.Player.previousSceneUUID == level1SceneUUID)
        {
            var pos = fromLvl1Reference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromLvl1Reference.transform.rotation;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == waterpathUUID)
        {
            var pos = fromWaterpathReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromWaterpathReference.transform.rotation;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == firepathUUID)
        {
            var pos = fromFirepathReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromFirepathReference.transform.rotation;
        }
    }

    //void OnDrawGizmo()
    //{
    //    if (fromLvl1Reference != null)
    //    {
    //        Gizmo.DrawLine(fromLvl1Reference.transform.position, fromLvl1Reference.transform.Forward, Color.Green);
    //    }

    //    if (fromWaterpathReference != null)
    //    {
    //        Gizmo.DrawLine(fromWaterpathReference.transform.position, fromWaterpathReference.transform.Forward, Color.Green);
    //    }

    //    if (fromFirepathReference != null)
    //    {
    //        Gizmo.DrawLine(fromFirepathReference.transform.position, fromFirepathReference.transform.Forward, Color.Green);
    //    }
    //}
}