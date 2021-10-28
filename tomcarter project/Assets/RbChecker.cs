using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbChecker : MonoBehaviour
{
    private Rigidbody rb;
    public float velocityY;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocityY = rb.velocity.y;
    }
}
