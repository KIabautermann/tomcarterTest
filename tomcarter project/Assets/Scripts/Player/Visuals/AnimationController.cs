using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public List<Animator> animators;
    
    private void Awake() {
        GetAnimators(this.transform);
    }

    public void SetFloat(string param, float n){
        for(int i = 0; i<animators.Count; i++){
            if(HasParameter(param, animators[i])){
                animators[i].SetFloat(param, n);
            }           
        }
    }

    public void SetBool(string param, bool n){
        for(int i = 0; i<animators.Count; i++){
            if(HasParameter(param, animators[i])){
                animators[i].SetBool(param, n);
            }           
        }
    }

    public void SetTrigger(string param){
        for(int i = 0; i<animators.Count; i++){
            if(HasParameter(param, animators[i])){
                animators[i].SetTrigger(param);
            }           
        }
    }
    private void GetAnimators(Transform target){
        int count = target.childCount;
        if(target.GetComponent<Animator>() != null){
            animators.Add(target.GetComponent<Animator>());
        }
        if(count == 0){
            return;
        }
        for(int i = 0; i < count; i++){
            GetAnimators(target.GetChild(i));
        }
    }

    private bool HasParameter( string paramName, Animator animator )
{
    foreach( AnimatorControllerParameter param in animator.parameters )
    {
        if( param.name == paramName )
            return true;
    }
    return false;
}
}
