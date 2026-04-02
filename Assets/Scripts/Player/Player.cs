using System;
using Loopie;

public class Player : Component
{
    public PlayerMovement Movement;
    public PlayerAnimation Animation;
    public PlayerCombat Combat;
    public PlayerItems Items;
    public Health PlayerHealth;
    public PlayerCamera Camera;
    public Entity GrappleLine;
    public Entity HookAnchor;

    void OnCreate()
    {
        Movement = entity.GetComponent<PlayerMovement>();
        Animation = entity.GetComponent<PlayerAnimation>();
        Combat = entity.GetComponent<PlayerCombat>();
        Items = entity.GetComponent<PlayerItems>();
        PlayerHealth = entity.GetComponent<Health>();
        GrappleLine = Entity.FindEntityByName("GrappleLine");
        if (GrappleLine != null) GrappleLine.SetActive(false);
        HookAnchor = Entity.FindEntityByName("HookAnchor");
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