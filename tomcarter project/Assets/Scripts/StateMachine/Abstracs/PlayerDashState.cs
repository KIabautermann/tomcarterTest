using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public abstract class PlayerDashState : PlayerUnlockableSkill
{
    protected Vector3 direction;
    protected bool coyoteTime;
    protected float currentSpeed;
    protected float timeInAir;
    public bool _velocityUpdated { get; private set;}
    private Collider[] _hedgeCollisionsChecks;
    private PlayerHedgeState playerHedgeState;
    protected bool _hedgeUnlocked;
    private float _speedLerpCounter;
    public float _relativeSpawnTime;

    // TODO: empezar a ver como la subscripcion de eventos deberia ser solo para el dash activo y no ambos. Podria traer muchos bardos
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        coolDown = stats.dashCooldown;
        playerHedgeState = GetComponent<PlayerHedgeState>();
        playerHedgeState.onTransitionIn.AddListener(OnHedge_Handler);
        PlayerEventSystem.GetInstance().OnGroundLand += OnGround_Handler;
        PlayerEventSystem.GetInstance().OnHazardHit += OnHazard_Handler;
        _hedgeUnlocked = abilitySystem.IsPermanentlyUnlocked(typeof(PlayerHedgeState));
        abilitySystem.OnAbilityUnlocked += HedgeUnlockHandler;
        stateIndex = stats.dashNumberID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
      

        if (_hedgeCollisionsChecks.Length != 0 /*&& FitsInHedge(direction)*/)
        {
            Physics.IgnoreLayerCollision(9,10,false);
        }

        // Arrancar dash si paso suficiente tiempo desde la transicion a este estado
        if(StartedDash()){
            StartDash();
        } 

        // Arrancar el dash solo UNA vez en el caso de estar pegado un hedge en frente
        if(!_velocityUpdated && _hedgeCollisionsChecks.Length != 0 /*&& FitsInHedge(direction)*/) {
            StartDash();
        }  
      
        if(!controller.Grounded())
        {
            timeInAir += Time.deltaTime;
        }
        else
        {
            timeInAir = 0;
        }

        if(counter > stats.dashLenght)
        {
            _speedLerpCounter += 1/stats.dashendLenght * Time.deltaTime;
            currentSpeed = Mathf.Lerp(stats.dashSpeed, stats.movementVelocity, _speedLerpCounter);
            if(counter > stats.dashLenght + stats.dashendLenght)
            {
                stateDone = true;
                controller.SetDrag(0);
                controller.SetGravity(true);
            }
        }

    }


    private void StartDash()
    {
        controller.SetTotalVelocity(currentSpeed,direction);
        _velocityUpdated = true;
        platformManager.LogicUpdated();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        _hedgeCollisionsChecks = Physics.OverlapBox(transform.position + direction * .5f, controller.myCollider.bounds.size, Quaternion.identity, stats.hedge);
        _relativeSpawnTime = stats.dashAfterimageCounter / (controller.CurrentVelocity.magnitude / stats.dashSpeed);
        _relativeSpawnTime = Mathf.Clamp(_relativeSpawnTime, stats.dashAfterimageCounter, stats.dashAfterimageCounter * 2f);
        if (extraCounter >= _relativeSpawnTime)
        {
            _target.vfxSpawn.InstanceEffect(null, transform.position, Quaternion.identity, _target.vfxSpawn.EffectRepository.afterimage);
            extraCounter = 0;
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        currentSpeed = stats.dashSpeed;
        inputs.UsedDash();
        controller.SetTotalVelocity(0,Vector2.zero);
        controller.SetAcceleration(1);
        controller.SetGravity(false);
        controller.SetDrag(stats.dashDrag);
        coyoteTime = false;
        timeInAir = controller.Grounded() ? 0 : stats.dashCoyoteTime + 1;
        Physics.IgnoreLayerCollision(9,10, _hedgeUnlocked);
        _velocityUpdated = false;
        _hedgeCollisionsChecks = new Collider[0];
        _speedLerpCounter = 0;
        ToggleLock(true);
    }

    private void HedgeUnlockHandler(object sender, PlayerAbilitySystem.AbiltyUnlockedEventArgs args) 
    {
        _hedgeUnlocked = args.added.Contains(typeof(PlayerHedgeState)) || !args.removed.Contains(typeof(PlayerHedgeState));
    }
    protected bool StartedDash() => counter > stats.dashStartUp;

    protected override void DoTransitionOut()
    {
        Physics.IgnoreLayerCollision(9,10,false);

        platformManager.LogicExit();
        
        controller.SetGravity(true);
        controller.SetDrag(0);

        // No pasar la negacion de Grounded al toggle ya que podria estar en falso por impactar con un hazard
        if (controller.Grounded())
        {
            ToggleLock(false);
        }
        
        base.DoTransitionOut();
        if(direction.x !=0)
        {
            controller.SetAcceleration(1);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        if (direction.y > 0) controller.SetVelocityY(controller.CurrentVelocity.y / 2);
        StartCoroutine(_target.GravityExceptionTime());
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if (_hedgeCollisionsChecks.Length != 0 /*&& FitsInHedge(direction)*/)
        {
            controller.SetDrag(0);
            _target.ChangeState<PlayerHedgeState>();
        }
        else if (inputs.JumpInput && CanDashJump())
        {
            inputs.UsedJump();       
            _target.ChangeState<PlayerDashJumpState>();
        }    
    }

    private bool CanDashJump()
    {
        if (controller.Grounded()) return true;
        if (timeInAir <= stats.dashCoyoteTime) return true;
        return false;
    }




    private bool FitsInHedge(Vector2 dir) 
    {   
        // If there's a hedge in proximity, check via raycast whether the player fits into the oncominghedge
        //
        // Estimado Pedro del futuro: lamento que tengas que ver este codigo. No quiero poner a prueba tu sanidad, pero no quedo otra. Lo que
        // hace esta bella funcion, es armar un cono de raycast para predecir si el Player deberia o no entrar en un hedge. Esta bueno
        // particularmente para que evitar dejar al jugador entrar a hedges en situacions medio border, como que solo el punto medio del personaje
        // este alineado con un hedge, pero el 50% del collider esta afuera
        //
        // Entonces, disparamos 2 raycasts para ver si el hedge es lo suficientemente amplio. El problemita problemon, es que si queremos entrar en diagonal
        // uno de los rayos tendria que ser mas largo que el otro para poder compensar que ambos van para el mismo valor del eje X, pero uno tiene que recorrer
        // mucha mas distancia. Para solucionar esto, hacemos un calculo donde la amplitud del cono se reduce cuanto mas se acerca la direccion a los 45 grados.
        // Esta hecho con trigonometria porque sos un drogadicto de mierda, y porque si se llega a usar analogico, esta bueno que se adapte en funcion a 
        // FixedAxis mas expresivos que [0,1]
        //
        // La razon por la cual a 45 grados el cono se aplana es meramente porque los hedges tienen angulos de 90 grados alineados con la grid. Si esto llega a
        // cambiar, tranquilamente podemos volver a tener casos donde se ve inverosimil la entrada al hedge
        //
        // Agregue tambien un cambio para que el angulo del cono se achique en funcion del tamaño del collider para que sea dinamico, sin importar si se vuelve mas
        // angosto o mas ancho. Este codigo podriamos moverlo a algun util probablemente
        Vector3 colliderSize = controller.myCollider.bounds.size;
        Vector3 extentsSize = controller.myCollider.bounds.extents;
        Vector2 zeroedDirection = new Vector2(Math.Abs(dir.x), Math.Abs(dir.y)).normalized;
        float rotationAngle = zeroedDirection.x == 1 ? 0 : zeroedDirection.y == 1 ? 90 :  (float) (Math.Atan(zeroedDirection.y / zeroedDirection.x) * (180/Math.PI));
        float colliderSideWallAngle = (float) (Math.Acos(Math.Pow(extentsSize.magnitude, 2) * 2 - Math.Pow(colliderSize.y, 2) / (2 * extentsSize.magnitude * 2)) * (180/Math.PI));
        float colliderVerticalWallAngle = (float) (Math.Acos(Math.Pow(extentsSize.magnitude, 2) * 2 - Math.Pow(colliderSize.x, 2) / (2 * extentsSize.magnitude * 2)) * (180/Math.PI));
        float baseAngle = (rotationAngle - 45) < 0 ? colliderSideWallAngle : colliderVerticalWallAngle;
        // Reduci un poco el angulo para no ser tan estrictos. Modificar a discrecion. No se si da hacerlo serializable/data, porque es absolutamente inreutilizable
        baseAngle *= 0.85f;
        bool topHit = Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.Euler(0, 0, (baseAngle * Math.Abs(rotationAngle - 45) / 45)) * new Vector3(dir.x, dir.y, 0), 
            out RaycastHit topHitInfo, 
            stats.collisionDetection + Math.Max(extentsSize.x, extentsSize.y), 
            stats.hedge);
        bool bottomHit = Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.Euler(0, 0, (-baseAngle * Math.Abs(rotationAngle - 45) / 45)) * new Vector3(dir.x, dir.y, 0), 
            out RaycastHit bottomHitInfo, 
            stats.collisionDetection + Math.Max(extentsSize.x, extentsSize.y), 
            stats.hedge);   
        
        return topHit && bottomHit && topHitInfo.collider.gameObject == bottomHitInfo.collider.gameObject;
    }


    #region Event Handlers
    private void OnGround_Handler(object sender, PlayerEventSystem.OnLandEventArgs args) {
        ToggleLock(false);
    }
    private void OnHazard_Handler(object sender, EventArgs args) {
        ToggleLock(false);
    }
    private void OnHedge_Handler() {
        ToggleLock(false);
    }
    protected override void OnDestroyHandler() {
        PlayerEventSystem.GetInstance().OnGroundLand -= OnGround_Handler;
        playerHedgeState.onTransitionIn.RemoveListener(OnHedge_Handler);
        PlayerEventSystem.GetInstance().OnHazardHit -= OnHazard_Handler;
        abilitySystem.OnAbilityUnlocked -= HedgeUnlockHandler;
        base.OnDestroyHandler();
    }

    #endregion
}
