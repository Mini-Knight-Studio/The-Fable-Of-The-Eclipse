using System;
using Loopie;

public class Player : Component
{
    [Header("Health")]
    public Health PlayerHealth;

    [Header("Camera")]
    protected Entity CameraEntity;
    public PlayerCamera Camera;

    [Header("PlayerComponents")]
    public PlayerInput Input;
    public PlayerMovement Movement;
    public PlayerAnimation Animation;
    public PlayerCombat Combat;
    public PlayerGrapple Grapple;
    public PlayerFeedback Feedback;

    public PlayerTorch Torch;

    [Header("Others")]
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

        Feedback = entity.GetComponent<PlayerFeedback>();
        Feedback.SetOwner(this);

        Grapple = entity.GetComponent<PlayerGrapple>();
        if (Grapple != null) Grapple.SetOwner(this);

        Torch = entity.GetComponent<PlayerTorch>();
        if (Torch != null) Torch.SetOwner(this);

        GrappleLine = Entity.FindEntityByName("GrappleLine");
        if (GrappleLine != null) GrappleLine.SetActive(false);

        HookAnchor = Entity.FindEntityByName("HookAnchor");

        Effects = entity.GetComponent<TemporalEffectApplier>();
        LoseTransition = entity.GetComponent<SceneTransition>();

        RespawnTransition = RespawnTransitionEntity.GetComponent<FadeInOutEvent>();
        RespawnTransition.OnFadeInComplete += EndRespawn;

        if (Movement == null) Debug.Log("Missing PlayerMovement");
        if (Animation == null) Debug.Log("Missing PlayerAnimation");
        if (Combat == null) Debug.Log("Missing PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Missing Health");
        if (Input == null) Debug.Log("Missing PlayerInput");
        if (Camera == null) Debug.Log("Missing PlayerCamera");
        if (Grapple == null) Debug.Log("Missing PlayerGrapple");
        if (Feedback == null) Debug.Log("Missing PlayerFeedback");
        if (Torch == null) Debug.Log("Missing PlayerTorch");

        PlayerHealth.Init();
        Feedback.Initialize();
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
        Movement.gravityActive = false;
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
        Torch.ProcessTorch();
        Grapple.ProcessGrappel();
        Animation.ProcessAnimations();
        Feedback.ProcessFeedback();

    }

    void OnDestroy()
    {
        RespawnTransition.OnFadeInComplete -= EndRespawn;
    }
}