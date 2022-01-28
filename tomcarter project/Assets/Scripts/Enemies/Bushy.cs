using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushy : Enemy
{
    private MovementController _controller;
    public float travelDistance;
    public float movementSpeed;
    public int waypointIndex;
    public bool onWall;
    private Vector3[] _waypoints = new Vector3[2];
    
    public override void Start()
    {
        base.Start();
        _waypoints[0] = transform.position + transform.right * travelDistance;
        _waypoints[1] = transform.position + transform.right * -travelDistance;
        waypointIndex = 0;
        _controller = GetComponent<MovementController>();
        _controller.SetAcceleration(1);
    }

    public override void Update()
    {
        base.Update();
        onWall = _controller.OnWall();
        if(Mathf.Abs(transform.position.x - _waypoints[waypointIndex].x) < .2f)
        {
            ChangeDirection();
        }
        _controller.Accelerate(1/.2f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
       if(_controller.OnWall() || _controller.OnLedge())
        {
            ChangeDirection();
        }
        _controller.SetVelocityX(movementSpeed * transform.right.x);
    }

    private void ChangeDirection()
    {
        if (waypointIndex == _waypoints.Length-1)
        {
            waypointIndex = 0;
        }
        else waypointIndex++;
        float tempFloat = (_waypoints[waypointIndex].x - transform.position.x);
        _controller.FlipCheck((int)new Vector3(tempFloat,0,0).normalized.x);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(_waypoints[i], 1);
        }
    }

    


}
