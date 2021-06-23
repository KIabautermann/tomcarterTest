using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    public int sides;
    public int radious;
    public int divisions;
    private List<GameObject> corners = new List<GameObject>();
    private List<GameObject> midVertices = new List<GameObject>();
    public Vector2 InitialDirection;
    public GameObject vertexPrefab;

    private void Start() {
        Vector3 position;
        corners.Clear();
        for(int i = 0; i<sides; i++){
            position = transform.position + (Quaternion.Euler(0,0,(360/sides)*i) * new Vector3(InitialDirection.x,InitialDirection.y)).normalized * radious;  
            corners.Add(Instantiate(vertexPrefab, position, Quaternion.identity));
            corners[i].GetComponent<Rigidbody>().isKinematic = true;
        midVertices.Clear();
        }
        for(int i = 0; i<corners.Count; i++){           
            Vector3 distance;
            if(i!= corners.Count-1){
                distance = corners[i+1].transform.position - corners[i].transform.position;
            }
            else{
                distance = corners[0].transform.position - corners[i].transform.position;
            }          
            for(int j = 1; j<=divisions; j++)
            {
                position = corners[i].transform.position + (distance/(divisions+1)*j);
                midVertices.Add(Instantiate(vertexPrefab, position, Quaternion.identity));                       
            }    
            for(int j = 0; j< midVertices.Count; j++){
                 if(j==0){
                    Debug.Log("this");
                    midVertices[j].GetComponent<SpringJoint>().connectedBody = corners[i-1].GetComponent<Rigidbody>();
                }
                else if(j == midVertices.Count-1){
                    if(i == corners.Count-1){

                        Debug.Log("this");
                        midVertices[j].GetComponent<SpringJoint>().connectedBody = corners[0].GetComponent<Rigidbody>();
                    }
                    else{
                        Debug.Log("this");
                        midVertices[j].GetComponent<SpringJoint>().connectedBody = corners[i+1].GetComponent<Rigidbody>();
                    }
                }
                else{
                    Debug.Log("this");
                    midVertices[j].GetComponent<SpringJoint>().connectedBody = midVertices[j+1].GetComponent<Rigidbody>();
                }   
            } 
        } 
    }  
}


