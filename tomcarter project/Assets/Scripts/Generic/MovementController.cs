using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    
    #region Components
    private Rigidbody _rb;

    public Collider myCollider;
    #endregion

    #region Checks
    [SerializeField]
    private float _detectionLenght;
    [SerializeField]
    private LayerMask _walkwable;
    #endregion
    
    public Vector2 CurrentVelocity;
    public float AcelerationIndex { get; private set; }
    public int facingDirection { get; private set; } 
    private Vector2 _cornerA;
    private Vector2 _cornerB;
    private Vector2 _cornerC;
    private Vector2 workspace;


    private void Awake() 
    {
        facingDirection = 1;      
    }
    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }
    #region Check Functions

    private void Update() 
    {
        _cornerA = new Vector2(myCollider.bounds.max.x-.01f, myCollider.bounds.max.y-.01f);
        _cornerB = new Vector2(myCollider.bounds.max.x-.01f, myCollider.bounds.min.y+.01f);
        _cornerC = new Vector2(myCollider.bounds.min.x+.01f, myCollider.bounds.min.y+.01f);
        CurrentVelocity = _rb.velocity;
    }
    public bool Grounded()
    {
       int n = 0;
       if(Physics.Raycast(_cornerC, -Vector2.up, _detectionLenght, _walkwable))
        {
           n++;
        }
       if(Physics.Raycast(_cornerB, -Vector2.up, _detectionLenght, _walkwable))
        {
            n++;
        }
       return n!=0 && CurrentVelocity.y < .01f;
    }
    public bool OnWall()
    {
       int n = 0;
       if(Physics.Raycast(_cornerA, Vector3.right * facingDirection, _detectionLenght, _walkwable))
        {
           n++;
        }
       if(Physics.Raycast(_cornerB, Vector3.right * facingDirection, _detectionLenght, _walkwable))
        {
            n++;
        }
       return n!=0;
    }
    
    public void FlipCheck(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
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
    #endregion
    
    #region DoFunctions
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        SetAcceleration(.25f);
    }
    public void Force(Vector2 direction, float force, ForceMode mode)
    {
        _rb.AddForce(direction * force, mode);
    }

    #endregion

    private void OnDrawGizmos() 
    {
       Vector2 cornerA = new Vector2(myCollider.bounds.max.x-.01f, myCollider.bounds.max.y-.01f);
       Vector2 cornerB = new Vector2(myCollider.bounds.max.x-.01f, myCollider.bounds.min.y+.01f);
       Vector2 cornerC = new Vector2(myCollider.bounds.min.x+.01f, myCollider.bounds.min.y+.01f);
       Gizmos.DrawLine(cornerC, cornerC - Vector2.up * _detectionLenght);
       Gizmos.DrawLine(cornerB, cornerB - Vector2.up * _detectionLenght);
       Gizmos.DrawLine(cornerA, cornerA + Vector2.right * _detectionLenght);
       Gizmos.DrawLine(cornerB, cornerB + Vector2.right * _detectionLenght);

    }
    
}



