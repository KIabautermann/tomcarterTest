using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private List<Animator> animators;
    
    private void Awake() {
        GetAnimators(this.transform);
    }

    public void SetFloat(string parameter, float n){
        for(int i = 0; i<animators.Count; i++){
            animators[i].SetFloat(parameter, n);
        }
    }

    public void SetBool(string parameter, bool n){
        for(int i = 0; i<animators.Count; i++){
            animators[i].SetBool(parameter, n);
        }
    }

    public void SetTrigger(string parameter){
        for(int i = 0; i<animators.Count; i++){
            animators[i].SetTrigger(parameter);
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
}
