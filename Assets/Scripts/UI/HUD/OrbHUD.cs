using System;
using Loopie;

public class OrbHUD : Component
{
    public string orbPrefix = "HUD_Orb_";
    public int maxOrbs = 3;

    private Entity[] orbIcons;

    private bool airWasCollected = false;
    private bool waterWasCollected = false;
    private bool fireWasCollected = false;

    void OnCreate()
    {
        orbIcons = new Entity[maxOrbs];

        for (int i = 0; i < maxOrbs; i++)
        {
            string searchName = orbPrefix + (i + 1).ToString();
            orbIcons[i] = Entity.FindEntityByName(searchName);
        }

        ForceUpdateIcons();
    }

    void OnUpdate()
    {
        if (DatabaseRegistry.playerDB == null) return;

        bool airNow = DatabaseRegistry.playerDB.Player.gemAirCollected;
        bool waterNow = DatabaseRegistry.playerDB.Player.gemWaterCollected;
        bool fireNow = DatabaseRegistry.playerDB.Player.gemFireCollected;

        if (airNow != airWasCollected || waterNow != waterWasCollected || fireNow != fireWasCollected)
        {
            ForceUpdateIcons();

            airWasCollected = airNow;
            waterWasCollected = waterNow;
            fireWasCollected = fireNow;
        }
    }

    private void ForceUpdateIcons()
    {
        if (DatabaseRegistry.playerDB == null) return;

        if (orbIcons.Length > 0 && orbIcons[0] != null)
            orbIcons[0].SetActive(DatabaseRegistry.playerDB.Player.gemAirCollected);

        if (orbIcons.Length > 1 && orbIcons[1] != null)
            orbIcons[1].SetActive(DatabaseRegistry.playerDB.Player.gemWaterCollected);

        if (orbIcons.Length > 2 && orbIcons[2] != null)
            orbIcons[2].SetActive(DatabaseRegistry.playerDB.Player.gemFireCollected);
    }
};