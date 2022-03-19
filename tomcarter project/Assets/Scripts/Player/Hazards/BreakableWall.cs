using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IBreakable
{
    [SerializeField]
    private float endurance;
    public void onBreak(float velocity)
    {
        if(velocity >= endurance){
            Destroy(gameObject);
        }  
    }
}
