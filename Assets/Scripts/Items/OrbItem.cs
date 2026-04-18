using System;
using Loopie;

public class OrbItem : Component
{
    public int orbType = 0;
    void OnCreate()
    {
    }

    void OnUpdate()
    {
        BoxCollider col = entity.GetComponent<BoxCollider>();

        if (col != null && col.HasCollided)
        {

            switch (orbType)
            {
                case 0:
                    DatabaseRegistry.playerDB.Player.gemAirCollected = true;
                    break;
                case 1:
                    DatabaseRegistry.playerDB.Player.gemWaterCollected = true;
                    break;
                case 2:
                    DatabaseRegistry.playerDB.Player.gemFireCollected = true;
                    break;
                default:
                    break;
            }

            entity.SetActive(false);
        }
    }
};