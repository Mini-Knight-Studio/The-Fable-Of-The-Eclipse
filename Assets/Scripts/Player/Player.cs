using System;
using Loopie;

public class Player : Component
{
    public PlayerMovement Movement;
    public Movement Movement2;
    public PlayerAnimation Animation;
    public PlayerCombat Combat;
    public PlayerItems Items;
    public Health PlayerHealth;
    public PlayerCamera Camera;

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

        if (Movement == null) Debug.Log("Missing PlayerMovement");
        if (Animation == null) Debug.Log("Missing PlayerAnimation");
        if (Combat == null) Debug.Log("Missing PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Missing Health");
    }

    void OnUpdate()
    {
    }
}