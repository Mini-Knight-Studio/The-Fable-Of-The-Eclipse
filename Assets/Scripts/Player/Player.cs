using System;
using Loopie;

public class Player : Component
{
    public Health PlayerHealth;

    protected Entity CameraEntity;
    public PlayerCamera Camera;

    public PlayerInput Input;
    public PlayerMovement Movement;
    public PlayerAnimation Animation;
    public PlayerCombat Combat;
    public PlayerGrapple Grapple;

    // --- ADD THIS LINE ---
    public PlayerTorch Torch;

    public Entity GrappleLine;
    public Entity HookAnchor;
    public TemporalEffectApplier Effects;
    private SceneTransition LoseTransition;

    public Entity RespawnTransitionEntity;
    public FadeInOutEvent RespawnTransition;

    public static Player Instance { get; private set; }

    void OnCreate()
    {
        Instance = this;

        PlayerHealth = entity.GetComponent<Health>();

        if (CameraEntity != null)
            Camera = CameraEntity.GetComponent<PlayerCamera>();

        Input = entity.GetComponent<PlayerInput>();
        Input.SetOwner(this);

        Movement = entity.GetComponent<PlayerMovement>();
        Movement.SetOwner(this);

        Animation = entity.GetComponent<PlayerAnimation>();
        Animation.SetOwner(this);

        Combat = entity.GetComponent<PlayerCombat>();
        Combat.SetOwner(this);

        Grapple = entity.GetComponent<PlayerGrapple>();
        if (Grapple != null) Grapple.SetOwner(this);

        // --- ADD THESE LINES TO INITIALIZE THE TORCH ---
        Torch = entity.GetComponent<PlayerTorch>();
        if (Torch != null) Torch.SetOwner(this);

        GrappleLine = Entity.FindEntityByName("GrappleLine");
        if (GrappleLine != null) GrappleLine.SetActive(false);

        HookAnchor = Entity.FindEntityByName("HookAnchor");

        Effects = entity.GetComponent<TemporalEffectApplier>();
        LoseTransition = entity.GetComponent<SceneTransition>();

        RespawnTransition = RespawnTransitionEntity.GetComponent<FadeInOutEvent>();
        RespawnTransition.OnFadeInComplete += EndRespawn;

        // Debug checks
        if (Movement == null) Debug.Log("Missing PlayerMovement");
        if (Animation == null) Debug.Log("Missing PlayerAnimation");
        if (Combat == null) Debug.Log("Missing PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Missing Health");
        if (Input == null) Debug.Log("Missing PlayerInput");
        if (Camera == null) Debug.Log("Missing PlayerCamera");
        if (Grapple == null) Debug.Log("Missing PlayerGrapple");

        // Added debug for Torch
        if (Torch == null) Debug.Log("Missing PlayerTorch");

        PlayerHealth.Init();
    }

    public void GoToLastCheckpoint()
    {
        entity.transform.position = new Vector3(DatabaseRegistry.playerDB.Player.playerPositionX, DatabaseRegistry.playerDB.Player.playerPositionY, DatabaseRegistry.playerDB.Player.playerPositionZ);
    }

    public void StartRespawn()
    {
        RespawnTransition.StartFade();
        PlayerHealth.canBeDamaged = false;
    }

    private void EndRespawn()
    {
        GoToLastCheckpoint();
        PlayerHealth.canBeDamaged = true;
    }

    void OnUpdate()
    {
        if (Pause.isPaused)
        {
            return;
        }

        Input.ProcessInputs();
        Movement.ProcessMovement();
        Combat.ProcessCombat();

        Animation.ProcessAnimations();
    }

    void OnDestroy()
    {
        RespawnTransition.OnFadeInComplete -= EndRespawn;
    }
}