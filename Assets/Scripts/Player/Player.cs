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


    [Header("Canvas (OPTIONAL)")]
    public Entity RespawnTransitionEntity;
    public Entity LoseScreenEntity;
    public Entity CreditsScreenEntity;
    public FadeInOutEvent RespawnTransition;
    public LoseScreen LoseScreen;
    public CreditsScreen CreditsScreen;

    public bool IsInCutscene = false;

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


        RespawnTransition = RespawnTransitionEntity.GetComponent<FadeInOutEvent>();
        LoseScreen = LoseScreenEntity.GetComponent<LoseScreen>();
        CreditsScreen = CreditsScreenEntity.GetComponent<CreditsScreen>();

        if (RespawnTransition != null)
            RespawnTransition.OnFadeInComplete += EndRespawn;
        if (LoseScreen != null)
            PlayerHealth.OnDeath += LoseScreen.OpenLoseScreen;

        if (PlayerHealth != null)
        {
            PlayerHealth.OnHit += Animation.PlayHit;
        }
    }

    public void GoToLastCheckpoint()
    {
        Vector3 pos = entity.transform.position;
        DatabaseRegistry.playerDB.Player.GetPosition(ref pos);
        entity.transform.position = pos;
    }

    public void StartRespawn()
    {
        if (RespawnTransition == null)
            return;
        if (PlayerHealth.GetActualHealth() > 0)
        {
            RespawnTransition.StartFade();
            PlayerHealth.canBeDamaged = false;
        }
    }

    private void EndRespawn()
    {
        GoToLastCheckpoint();
        PlayerHealth.canBeDamaged = true;
        Movement.gravityActive = false;
    }

    void OnUpdate()
    {
        Input.ProcessInputs(GameManager.state);

        if (GameManager.state == GameManager.GameState.DEFAULT)
        {
            Movement.ProcessMovement();
            Combat.ProcessCombat();
            Torch.ProcessTorch();
            Grapple.ProcessGrappel();
            Animation.ProcessAnimations();
            Feedback.ProcessFeedback();
        }
    }

    void OnDestroy()
    {
        if (RespawnTransition == null)
            return;
        RespawnTransition.OnFadeInComplete -= EndRespawn;
    }
}