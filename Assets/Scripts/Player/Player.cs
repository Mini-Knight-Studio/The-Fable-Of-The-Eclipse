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

    public TemporalEffectApplier Effects;
    private SceneTransition LoseTransition;

    public static Player Instance { get; private set; }

    void OnCreate()
    {
        Instance = this;

        PlayerHealth = entity.GetComponent<Health>();
        Camera = CameraEntity.GetComponent<PlayerCamera>();



        Input = entity.GetComponent<PlayerInput>();
        Input.SetOwner(this);
        Movement = entity.GetComponent<PlayerMovement>();
        Movement.SetOwner(this);
        Animation = entity.GetComponent<PlayerAnimation>();
        Animation.SetOwner(this);
        Combat = entity.GetComponent<PlayerCombat>();
        Combat.SetOwner(this);


        Effects = entity.GetComponent<TemporalEffectApplier>();
        LoseTransition = entity.GetComponent<SceneTransition>();



        if (Movement == null) Debug.Log("Missing PlayerMovement");
        if (Animation == null) Debug.Log("Missing PlayerAnimation");
        if (Combat == null) Debug.Log("Missing PlayerCombat");
        if (PlayerHealth == null) Debug.Log("Missing Health");
        if(Input == null) Debug.Log("Missing PlayerInput");
        if(Camera == null) Debug.Log("Missing PlayerCamera");



        PlayerHealth.Init();
    }

    private void PlayerHealth_OnDeath()
    {
        throw new NotImplementedException();
    }

    void OnUpdate()
    {
 
        Input.ProcessInputs();
        Movement.ProcessMovement();
        Combat.ProcessCombat();


        Animation.ProcessAnimations();
    }
}


