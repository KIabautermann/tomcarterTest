using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IBreakable
{
    [SerializeField]
    private float endurance;
    public void onBreak(Vector3 velocity)
    {
       if(Mathf.Abs(velocity.x) > endurance)
       {
           Debug.Log("break");
       }
       else
       {
           Debug.Log("weak");
       }
    }
}
