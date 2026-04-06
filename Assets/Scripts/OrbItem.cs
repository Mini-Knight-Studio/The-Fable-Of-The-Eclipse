using System;
using Loopie;

public class OrbItem : Component
{
    void OnCreate()
    {
    }

    void OnUpdate()
    {
        BoxCollider col = entity.GetComponent<BoxCollider>();

        if (col != null && col.HasCollided)
        {
            Entity playerEntity = Entity.FindEntityByName("Player");
            if (playerEntity != null)
            {
                OrbInventory inventory = playerEntity.GetComponent<OrbInventory>();
                if (inventory != null)
                {
                    if (inventory.orbsCollected < inventory.maxOrbs)
                    {
                        inventory.AddOrb();

                        entity.SetActive(false);
                    }
                }
            }
        }
    }
};