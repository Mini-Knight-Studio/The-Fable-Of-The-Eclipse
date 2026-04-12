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
    public TemporalEffectApplier Effects;
    public Entity GrappleLine;
    public Entity HookAnchor;

    void OnCreate()
    {
        Movement = entity.GetComponent<PlayerMovement>();
        Movement2 = entity.GetComponent<Movement>();
        Animation = entity.GetComponent<PlayerAnimation>();
        Combat = entity.GetComponent<PlayerCombat>();
        Items = entity.GetComponent<PlayerItems>();
        PlayerHealth = entity.GetComponent<Health>();
        Effects = entity.GetComponent<TemporalEffectApplier>();
        GrappleLine = Entity.FindEntityByName("GrappleLine");
        if (GrappleLine != null) GrappleLine.SetActive(false);
        HookAnchor = Entity.FindEntityByName("HookAnchor");


        Entity cameraEntity = Entity.FindEntityByName("PlayerCamera");
        if (cameraEntity != null)
        {
            Camera = cameraEntity.GetComponent<PlayerCamera>();
        }

        if (Movement == null) Debug.Log("Missing PlayerMovement");
        if (Animation == null) Debug.Log("Missing PlayerAnimation");
        if (Combat == null) Debug.Log("Missing PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Missing Health");
        PlayerHealth.Init();
    }

    void OnUpdate()
    { }
}