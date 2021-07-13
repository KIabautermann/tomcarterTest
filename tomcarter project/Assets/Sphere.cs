using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    private Vector3 initialPos;

    private void Start() {
        initialPos = transform.position;
    }


    private void Update() {
        transform.position += Vector3.right * speed * Time.deltaTime; 

        if(transform.position.x - initialPos.x > maxDistance){
            transform.position = initialPos;
        }
    }
}
