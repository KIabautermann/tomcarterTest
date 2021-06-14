using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    
    #region Components
    private Rigidbody _rb;
    public Collider myCollider;
    public float colliderX {get; private set;}
    public float colliderY {get; private set;}
    #endregion

    #region Checks
    [SerializeField]
    private float _detectionLenght;
    [SerializeField]
    private LayerMask _walkwable;
    [SerializeField]
    private LayerMask _platform;
    [SerializeField]
    private LayerMask _rootable;
    [SerializeField]
    private Transform[] groundCheck;
    [SerializeField]
    private Transform[] wallCheck;
    [SerializeField]
    private Transform[] ceilingCheck;
    private bool canFlip;
    private bool flipRequest;

    #endregion
    
    public Vector2 CurrentVelocity;
    public float AcelerationIndex { get; private set; }
    public int facingDirection { get; private set; } 
    public int lastDirection { get; private set; } 
    private Vector2 workspace;

    #region RayCast Hits
    private RaycastHit rootableHit;
    private RaycastHit groundHit;
    #endregion


    private void Awake() 
    {
        facingDirection = 1;      
        lastDirection = 1;      
        canFlip = true;       
    }
    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        SetChecks();      
    }
    #region Check Functions

    private void Update() 
    {
        CurrentVelocity = _rb.velocity;
        if(flipRequest && canFlip){
            Flip();
        }
    }
    public bool Grounded()
    {
       int n = 0;
       RaycastHit tmpHit = new RaycastHit();
       for(int i = 0; i < groundCheck.Length; i++){
           if(Physics.Raycast(groundCheck[i].position, -Vector2.up, out tmpHit, _detectionLenght, _walkwable )){
               n++;
               groundHit = tmpHit;
           }
       }
       if (n == 0) { groundHit = tmpHit; }
       return n!=0 && CurrentVelocity.y < .01f;
    }

    public bool OnPlatform()
    {
       int n = 0;
       RaycastHit tmpHit = new RaycastHit();
       for(int i = 0; i < groundCheck.Length; i++){
           if(Physics.Raycast(groundCheck[i].position, -Vector2.up, out tmpHit, _detectionLenght, _platform)){
               n++;
           }
       }
       return n!=0;
    }
    
    public bool OnRootable()
    {
       int n = 0;
       RaycastHit tmpHit = new RaycastHit();
       for(int i = 0; i < groundCheck.Length; i++){
           if(Physics.Raycast(groundCheck[i].position, -Vector2.up,out tmpHit, _detectionLenght, _rootable )){
               n++;
               rootableHit = tmpHit;
           }
       }
       if (n == 0) { rootableHit = tmpHit; }
       return n!=0 && CurrentVelocity.y < .01f;
    }
    public bool OnWall()
    {
       int n = 0;
       for(int i = 0; i < wallCheck.Length; i++){
           if(Physics.Raycast(wallCheck[i].position, Vector2.right * facingDirection,_detectionLenght, _walkwable )){
               n++;
               Debug.Log("Wall");
           }
       }
       return n!=0;
    }

    public bool OnCeiling()
    {
       int n = 0;
       for(int i = 0; i < ceilingCheck.Length; i++){
           if(Physics.Raycast(ceilingCheck[i].position, Vector2.up,_detectionLenght, _walkwable)){
               n++;
           }
       }
       return n!=0;
    }
    public void FlipCheck(int xInput)
    {
        if (xInput != 0 && xInput != lastDirection)
        {
            ChangeDirection();
        }
    }

    public Vector3 DirectionalDetection(){
        Vector3 direction = CurrentVelocity.normalized;
        return new Vector3(direction.x * colliderX, direction.y * colliderY,0);
    }
    #endregion
   
    #region Set Functions
    public void SetVelocityX(float x)
    {
        CurrentVelocity = _rb.velocity;
        workspace.Set(x * AcelerationIndex, CurrentVelocity.y);
        _rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float y)
    {
        CurrentVelocity = _rb.velocity;
        workspace.Set(CurrentVelocity.x, y);
        _rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void ClampYVelocity(float value)
    {
        if (CurrentVelocity.y <= value)
        {
            SetVelocityY(value);
        }
    }
    public void SetTotalVelocity(float speed, Vector2 direction)
    {
        CurrentVelocity = _rb.velocity;
        workspace = direction * speed * AcelerationIndex;
        _rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetDrag(float drag)
    {
        _rb.drag = drag;
    }

    public void SetGravity(bool useGravity)
    {
        _rb.useGravity = useGravity;
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

    private void SetChecks(){
        float Xmax = myCollider.bounds.max.x - .01f;
        float Ymax = myCollider.bounds.max.y - .01f;
        float Xmin = myCollider.bounds.min.x + .01f;
        float Ymin = myCollider.bounds.min.y + .01f;
        groundCheck[0].position = new Vector2(Xmin,Ymin);
        groundCheck[1].position = new Vector2(Xmax,Ymin);
        wallCheck[0].position = new Vector2(Xmax,Ymax);
        wallCheck[1].position = new Vector2(Xmax,Ymin);
        ceilingCheck[0].position = new Vector2(Xmin,Ymax);
        ceilingCheck[1].position = new Vector2(Xmax,Ymax);
        colliderX = myCollider.bounds.size.x;
        colliderY = myCollider.bounds.size.y;
    }
    #endregion
    
    #region DoFunctions
    private void ChangeDirection()
    {
        lastDirection *= -1;
        SetAcceleration(.25f);
        flipRequest = true;          
    }

    private void Flip(){
        if(facingDirection != lastDirection){
            transform.Rotate(0.0f, 180.0f, 0.0f);
            facingDirection = lastDirection;
            flipRequest = false;
        }
    }

    public void LockFlip(bool locked){
        canFlip = !locked;
    }
    public void Force(Vector2 direction, float force, ForceMode mode)
    {
        _rb.AddForce(direction * force, mode);
    }

    #endregion

    private void OnDrawGizmos() 
    {
       for(int i = 0; i<groundCheck.Length; i++){
           Gizmos.DrawLine(groundCheck[i].position, groundCheck[i].position + (-Vector3.up * _detectionLenght));
       }
       for(int i = 0; i<wallCheck.Length; i++){
           Gizmos.DrawLine(wallCheck[i].position, wallCheck[i].position + (Vector3.right * facingDirection * _detectionLenght));
       }
       for(int i = 0; i<ceilingCheck.Length; i++){
           Gizmos.DrawLine(ceilingCheck[i].position, ceilingCheck[i].position + (Vector3.up * _detectionLenght));
       }
    }
    
    #region RaycastHit Getters
    public RaycastHit GetRootableHit()
    {
        return rootableHit;
    }

    public RaycastHit GetGroundHit() 
    {
        return groundHit;
    }
    #endregion
}



