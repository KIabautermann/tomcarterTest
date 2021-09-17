using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public List<string> queuedAnimations;
    public bool locked;
    private bool lockRequest;
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }
    private void Update(){
        if (!locked && queuedAnimations.Count != 0){
            PlayAnimation();
        }       
    }
    private void PlayAnimation(){
        anim.Play(queuedAnimations[0]);
        queuedAnimations.RemoveAt(0);
        locked = lockRequest;
        lockRequest = false;
    }

    public void Queue(string anim, bool lockAnimation, bool priority){
        if (priority)
        {
            queuedAnimations.Clear();
            locked = false;
        }
        queuedAnimations.Add(anim);
        if (lockAnimation)
        {
            lockRequest = lockAnimation;
        }

    }

    public void AnimationUnlock()
    {
        locked = false;
    }
}
