using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    #region Components
    private Rigidbody _rb;
    public BoxCollider myCollider { get; private set; }



    RaycastHit m_Hit;

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
    private LayerMask _hedge;
    [SerializeField]
    private bool canFlip;
    private bool flipRequest;

    #endregion

    public Vector2 CurrentVelocity;
    [Range(0.0f, 1.0f)]
    public float AcelerationIndex;
    public int facingDirection { get; private set; } 
    public int lastDirection { get; private set; } 
    public bool usingGravity { get; private set; }
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
        myCollider = GetComponent<BoxCollider>();
        usingGravity = _rb.useGravity;
    }
    #region Check Functions

    private void Update() 
    {
        CurrentVelocity = _rb.velocity;
        usingGravity = _rb.useGravity;
        if(flipRequest && canFlip){
            Flip();
        }
    }
    public bool Grounded()
    {
        Vector3 detection = new Vector3(myCollider.bounds.size.x - .25f, _detectionLenght) / 2;
        Vector3 center = new Vector3(myCollider.bounds.center.x, myCollider.bounds.min.y, myCollider.bounds.center.z);
        Collider[] temp = Physics.OverlapBox(center, detection, Quaternion.identity, _walkwable);
        if (temp.Length != 0){
            Physics.Raycast(center + Vector3.up * _detectionLenght / 2, -Vector3.up, out groundHit, _detectionLenght, _walkwable);
            return true;

        }
        else
        {
            return false;
        }  
    }

    public bool OnPlatform()
    {
        int n = 0;
        RaycastHit tmpHit = new RaycastHit();
        Vector3 detection = new Vector3(myCollider.bounds.size.x - .1f, _detectionLenght) / 2;
        if (Physics.BoxCast(myCollider.bounds.center, detection, -Vector3.up, out tmpHit, Quaternion.identity, myCollider.bounds.size.y / 2, _platform))
        {
            n++;
        }
        return n != 0 && _rb.velocity.y <= 0;
    }
    
    public bool OnRootable()
    {
        int n = 0;
        RaycastHit tmpHit = new RaycastHit();
        Vector3 detection = new Vector3(myCollider.bounds.size.x - .1f, _detectionLenght) / 2;
        if (Physics.BoxCast(myCollider.bounds.center, detection, -Vector3.up, out tmpHit, Quaternion.identity, myCollider.bounds.size.y / 2, _rootable))
        {
            n++;
            rootableHit = tmpHit;
        }
        return n != 0 && _rb.velocity.y <= 0;
    }
    public bool OnHedge()
    {
        int n = 0;
        RaycastHit tmpHit = new RaycastHit();
        Vector3 detection = new Vector3(myCollider.bounds.size.x - .1f, _detectionLenght) / 2;
        if (Physics.BoxCast(myCollider.bounds.center, detection, -Vector3.up, out tmpHit, Quaternion.identity, myCollider.bounds.size.y / 2, _hedge))
        {
            n++;
        }
        return n != 0 && _rb.velocity.y <= 0;
    }
    public bool OnWall()
    {
        return Physics.Raycast(myCollider.bounds.center, transform.right, (myCollider.bounds.size.x / 2) + .5f, _walkwable);
    }

    public bool OnLedge()
    {
        if (!Grounded()) return false;
        float newPosX = (myCollider.bounds.center + transform.right * myCollider.bounds.size.x / 2).x;
        float newPosY = (myCollider.bounds.min.y + .1f);
        Vector3 newPos = new Vector3(newPosX, newPosY);
        return !Physics.Raycast(newPos, - Vector3.up, .5f, _walkwable);
    }

    public bool OnCeiling() 
    {
        int n = 0;
        RaycastHit tmpHit = new RaycastHit();
        Vector3 detection = new Vector3(myCollider.bounds.size.x - .1f, _detectionLenght) / 2;
        if (Physics.BoxCast(myCollider.bounds.center, detection, Vector3.up, out tmpHit, Quaternion.identity, myCollider.bounds.size.y / 2, _walkwable))
        {
            n++;
        }
        return n != 0;
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
        return new Vector3(direction.x * myCollider.bounds.size.x, direction.y * myCollider.bounds.size.y);
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

    public void SetCollider(Vector2 size, Vector2 position)
    {
        //myCollider.radius = size.x;
        //myCollider.height = size.y;
        //myCollider.center = position;
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

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            float newPosX = (myCollider.bounds.center + transform.right * myCollider.bounds.size.x / 2).x;
            float newPosY = (myCollider.bounds.min.y + .1f);
            Vector3 newPos = new Vector3(newPosX, newPosY);
            Gizmos.DrawLine(newPos, newPos - transform.up * .5f);
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



