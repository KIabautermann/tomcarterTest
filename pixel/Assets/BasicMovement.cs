using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Rigidbody2D _rb;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.rotation = Quaternion.Euler(0, 0, -Time.time*90);
        _rb.velocity = direction * speed;
    }
}
