using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region States

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState Idle { get; private set; }

    public PlayerMoveState Move { get; private set; }

    public PlayerJumpState Jump { get; private set; }

    public PlayerInAirState InAir { get; private set; }

    public PlayerLandState Land { get; private set; }

    public PlayerDashState Dash { get; private set; }

    public PlayerDashJumpState DashJump { get; private set; }

    public PlayerHookState Hook { get; private set; }

    public PlayerHedgeState Hedge { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    #endregion

    #region Components

    public Animator Anim { get; private set; }

    public PlayerInputHandler MyInputs { get; private set; }

    public Rigidbody RB { get; private set; }

    public SpriteRenderer Sprite { get; private set; }

    public BoxCollider MyCollider { get; private set; }


    [SerializeField]
    private ParticleSystem _particles;

    #endregion

    #region CheckVariables
    [SerializeField]
    private Transform[] groundCheck;
    [SerializeField]
    private Transform[] wallCheck;
    public Transform hookPoint;
    #endregion

    #region Others

    public Vector2 CurrentVelocity { get; private set; }

    public float AcelerationIndex { get; private set; }

    public int facingDirection { get; private set; } 

    private Vector2 workspace;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        facingDirection = 1;
        StateMachine = new PlayerStateMachine();
        Idle = new PlayerIdleState(this, StateMachine,playerData, "idle");
        Move = new PlayerMoveState(this, StateMachine, playerData, "move");
        Jump = new PlayerJumpState(this, StateMachine, playerData, "jump");
        InAir = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        Land = new PlayerLandState(this, StateMachine, playerData, "land");
        Dash = new PlayerDashState(this, StateMachine, playerData, "dash");
        DashJump = new PlayerDashJumpState(this, StateMachine, playerData, "dash jump");
        Hook = new PlayerHookState(this, StateMachine, playerData, "hook");
        Hedge = new PlayerHedgeState(this, StateMachine, playerData, "hedge");
        _particles.Pause();
    }

    private void Start()
    {
        MyInputs = GetComponent<PlayerInputHandler>();
        MyCollider = GetComponent<BoxCollider>();
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        Sprite = GetComponent<SpriteRenderer>(); 
        StateMachine.Initialize(Idle);
        _particles.Pause();
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();    
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        StateMachine.CurrentState.FrameEndUpdate();
    }

    #endregion

    #region Set Functions
    public void SetVelocityX(float x)
    {
        workspace.Set(x * AcelerationIndex, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float y)
    {
        workspace.Set(CurrentVelocity.x, y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetTotalVelocity(float speed, Vector2 direction)
    {
        workspace = direction * speed;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetAcceleration(float acceleration)
    {
        AcelerationIndex = Mathf.Clamp(acceleration,0,1);
    }
    public void Accelerate(float acceleration)
    {
        AcelerationIndex += acceleration;
        AcelerationIndex = Mathf.Clamp(AcelerationIndex, 0, 1);
    }
    #endregion

    #region Check Functions

    public bool Grounded()
    {
        int checks = 0;
        for(int i = 0; i < groundCheck.Length; i++)
        {
            if(Physics.Raycast(groundCheck[i].position, -transform.up, playerData.groundCheckDistance, playerData.walkable))
            {
                checks++;
            }
        }
        return checks != 0;
    }
    public bool OnWall()
    {
        int checks = 0;
        for (int i = 0; i < wallCheck.Length; i++)
        {
            if (Physics.Raycast(wallCheck[i].position, transform.right, playerData.wallCheckDistance, playerData.walkable))
            {
                checks++;
            }
        }
        return checks != 0;
    }
    public void FlipCheck(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Do Functions

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        SetAcceleration(.25f);
    }
    public void Force(Vector2 direction, float force)
    {
        RB.AddForce(direction * force);
    }

    public void SetParticles(bool set)
    {
        if (set)
        {           
            _particles.Play();
        }
        else
        {
            _particles.Stop();
        }
    }

    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishedTrigger();

    private void OnDrawGizmos()
    {
        for(int i = 0; i < groundCheck.Length; i++)
        {
            Gizmos.DrawLine(groundCheck[i].position, groundCheck[i].position - (Vector3.up * playerData.groundCheckDistance));
        }
        for (int i = 0; i < wallCheck.Length; i++)
        {
            Gizmos.DrawLine(wallCheck[i].position, wallCheck[i].position + (Vector3.right * playerData.wallCheckDistance));
        }
        Gizmos.DrawWireSphere(hookPoint.position, playerData.hookDetectionRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(playerData.hookTarget.x * facingDirection, playerData.hookTarget.y,0 ), playerData.hookDetectionRadius);
    }

    #endregion    

}
