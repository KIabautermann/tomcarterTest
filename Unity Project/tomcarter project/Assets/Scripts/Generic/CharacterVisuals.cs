using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisuals : MonoBehaviour
{
    private Animator anim;
    public void Flip(){
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void TriggerAnimation(string animation){
        anim.SetTrigger(animation);
    }
}
