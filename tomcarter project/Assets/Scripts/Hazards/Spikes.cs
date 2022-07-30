using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Vector2 size;
    public Vector2 offset;
    public LayerMask playerMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Collider [] dectection = Physics.OverlapBox(transform.position + transform.rotation * (Vector3)offset, size/2, transform.rotation, playerMask);
        if(dectection.Length != 0){
            dectection[0].GetComponent<PlayerHealth>().TakeDamage();
            Debug.Log("asd");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + (Vector3)offset, size);      
    }
}
