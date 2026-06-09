using System;
using Loopie;

class InitialPlayerPositionManager : Component
{
    public string level1SceneUUID;
    public string puzzle1SceneUUID;
    public string level2SceneUUID;

    public Entity fromPuzzleReference;
    public Entity fromLvl2Reference;

    private bool canSave = false;

    void OnPostCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(level1SceneUUID);

        float previousY = Player.Instance.transform.position.y;

        if (DatabaseRegistry.playerDB.Player.previousSceneUUID == puzzle1SceneUUID)
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

        canSave = true;
    }

    void OnUpdate()
    {
        if (canSave)
        {
            canSave = false;
            Save();
        }
    }

    public void Save()
    {
        Player player = Player.Instance;
        if (DatabaseRegistry.playerDB != null)
        {
            DatabaseRegistry.playerDB.Player.SetPosition(player.entity.transform.position);
            DatabaseRegistry.playerDB.Player.maxHealth = Player.Instance.PlayerHealth.GetMaxHealth();
            DatabaseRegistry.playerDB.Player.currentHealth = Player.Instance.PlayerHealth.GetActualHealth();
            DatabaseRegistry.playerDB.Save();
            Debug.Log("Player Data Saved");
        }
    }
}