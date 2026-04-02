using System;
using Loopie;

public class Player : Component
{
    private PlayerMovement Movement;
    private PlayerAnimation Animation;
    private PlayerCombat Combat;
    private PlayerItems Items;
    private Health PlayerHealth;
    private PlayerCamera Camera;

    void OnCreate()
    {
        Movement = entity.GetComponent<PlayerMovement>();
        Animation = entity.GetComponent<PlayerAnimation>();
        Combat = entity.GetComponent<PlayerCombat>();
        Items = entity.GetComponent<PlayerItems>();
        PlayerHealth = entity.GetComponent<Health>();

        Entity cameraEntity = Entity.FindEntityByName("PlayerCamera");
        if (cameraEntity != null)
        {
            Camera = cameraEntity.GetComponent<PlayerCamera>();
        }

        if (Movement == null) Debug.Log("Falta PlayerMovement");
        if (Animation == null) Debug.Log("Falta PlayerAnimation");
        if (Combat == null) Debug.Log("Falta PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Falta Health");
    }

    void OnUpdate()
    {
    }
}