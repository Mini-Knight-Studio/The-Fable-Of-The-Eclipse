using System;
using Loopie;

class WaterPuzzle_InitialPlayerPositionManager : Component
{
    public string waterpuzzleUUID;
    public string mechanicSceneUUID;
    public string waterpathUUID;

    public Entity fromMechanicReference;
    public Entity fromWaterpathReference;

    private bool canSave = false;

    void OnPostCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(waterpuzzleUUID);

        float previousY = Player.Instance.transform.position.y;

        if (DatabaseRegistry.playerDB.Player.previousSceneUUID == mechanicSceneUUID)
        {
            var pos = fromMechanicReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromMechanicReference.transform.rotation;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == waterpathUUID)
        {
            var pos = fromWaterpathReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromWaterpathReference.transform.rotation;
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

};