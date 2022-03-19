using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableObject
{
    private float _speed;
    private Vector2 _direction;
    private Rigidbody _rb;
    private GameObject poolerParent;

    public override void DisposeAnimation()
    {
        
    }

    public override void ResetSceneReferences()
    {
        this.gameObject.transform.position = Vector3.zero;
        StopAllCoroutines();
        gameObject.transform.SetParent(poolerParent.transform);
    }

    protected override void Start()
    {
        base.Start();
        if (transform.parent != null) poolerParent = transform.parent.gameObject;
        _rb = GetComponent<Rigidbody>();
    }

    public void Set(Vector2 newDirection, float newSpeed)
    {
        _direction = newDirection;
        _speed = newSpeed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction.normalized * _speed;

    }


}
