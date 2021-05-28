using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public abstract class PlayerDashState : PlayerUnlockableSkill
{
    protected Vector2 direction;
    protected bool coyoteTime;
    protected float coyoteStartTime;
    protected float currentSpeed;
    protected bool dashJumpCoyoteTime;
    private bool _velocityUpdated;
    private Collider[] _hedgeCollisionsChecks;

    private PlayerHedgeState playerHedgeState;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        coolDown = stats.dashCooldown;
        playerHedgeState = GetComponent<PlayerHedgeState>();
        playerHedgeState.onTransitionIn.AddListener(OnHedge_Handler);
        PlayerEventSystem.GetInstance().OnGroundLand += OnGround_Handler;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        DashJumpCoyoteTimeCheck();
        
        _hedgeCollisionsChecks = Physics.OverlapBox(transform.position, controller.myCollider.bounds.size, Quaternion.identity,stats.hedge); 

        if (_hedgeCollisionsChecks.Length != 0 && !FitsInHedge()) {
            Physics.IgnoreLayerCollision(9,10,false);
            _velocityUpdated = true;
        }

        if(StartedDash()){
            controller.SetTotalVelocity(currentSpeed,direction);
            _velocityUpdated = true;
        } 

        if(!_velocityUpdated && _hedgeCollisionsChecks.Length != 0 && FitsInHedge()) {
            controller.SetTotalVelocity(currentSpeed,direction);
            _velocityUpdated = true;
        }
        
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedDash();
        controller.SetTotalVelocity(0,Vector2.zero);
        controller.SetAcceleration(1);
        controller.SetGravity(false);
        controller.SetDrag(stats.drag);
        coyoteTime = false;
        dashJumpCoyoteTime = controller.Grounded();
        Physics.IgnoreLayerCollision(9,10,true);
        _velocityUpdated = false;
        _hedgeCollisionsChecks = new Collider[0];
    }

    protected bool StartedDash() => counter > + stats.dashStartUp;

    protected override void DoTransitionOut()
    {
        if (!controller.Grounded()) { isLocked = true; }

        Physics.IgnoreLayerCollision(9,10,false);

        base.DoTransitionOut();
        if(direction.x !=0)
        {
            if(inputs.FixedAxis.x == direction.x)
            {
                controller.SetAcceleration(1);
            }
            else
            {
                controller.SetAcceleration(.5f);
            }
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();   
        if (_hedgeCollisionsChecks.Length != 0 && FitsInHedge())
        {                  
            _target.ChangeState<PlayerHedgeState>();  
            controller.SetDrag(0);
        }
        else if(inputs.JumpInput && dashJumpCoyoteTime)
        {
            _target.ChangeState<PlayerDashJumpState>();
            inputs.UsedJump();            
        }    
    }

    public void DashJumpCoyoteTimeStart() => dashJumpCoyoteTime = true;

    private void DashJumpCoyoteTimeCheck()
    {
        if (dashJumpCoyoteTime && counter > stats.jumpHandicapTime)
        {
            dashJumpCoyoteTime = false;
        }
    }

    private bool FitsInHedge() 
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
        Vector2 zeroedDirection = new Vector2(Math.Abs(direction.x), Math.Abs(direction.y)).normalized;
        float rotationAngle = zeroedDirection.x == 1 ? 0 : zeroedDirection.y == 1 ? 90 :  (float) (Math.Atan(zeroedDirection.y / zeroedDirection.x) * (180/Math.PI));
        float colliderSideWallAngle = (float) (Math.Acos(Math.Pow(extentsSize.magnitude, 2) * 2 - Math.Pow(colliderSize.y, 2) / (2 * extentsSize.magnitude * 2)) * (180/Math.PI));
        float colliderVerticalWallAngle = (float) (Math.Acos(Math.Pow(extentsSize.magnitude, 2) * 2 - Math.Pow(colliderSize.x, 2) / (2 * extentsSize.magnitude * 2)) * (180/Math.PI));
        float baseAngle = (rotationAngle - 45) < 0 ? colliderSideWallAngle : colliderVerticalWallAngle;
        // Reduci un poco el angulo para no ser tan estrictos
        baseAngle *= 0.75f;
        bool topHit = Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.Euler(0, 0, (baseAngle * Math.Abs(rotationAngle - 45) / 45)) * new Vector3(direction.x, direction.y, 0), 
            out RaycastHit topHitInfo, 
            stats.collisionDetection + Math.Max(colliderSize.x, colliderSize.y) * 2, 
            stats.hedge);
        bool bottomHit = Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.Euler(0, 0, (-baseAngle * Math.Abs(rotationAngle - 45) / 45)) * new Vector3(direction.x, direction.y, 0), 
            out RaycastHit bottomHitInfo, 
            stats.collisionDetection + Math.Max(colliderSize.x, colliderSize.y) * 2, 
            stats.hedge);   
        
        return topHit && bottomHit && topHitInfo.collider.gameObject == bottomHitInfo.collider.gameObject;
    }
   

    #region Event Handlers
    private void OnGround_Handler(object sender, PlayerEventSystem.OnLandEventArgs args) {
        ToggleLock(false);
    }
    private void OnHedge_Handler() {
        ToggleLock(false);
    }
    protected override void OnDestroyHandler() {
        PlayerEventSystem.GetInstance().OnGroundLand -= OnGround_Handler;
        playerHedgeState.onTransitionIn.RemoveListener(OnHedge_Handler);
        base.OnDestroyHandler();
    }

    #endregion
}
