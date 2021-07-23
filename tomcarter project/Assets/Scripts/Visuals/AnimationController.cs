using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation2dArray 
{
    [System.Serializable]
    public struct rowData{
        public List<AnimationClip> row;
    }
    public List<rowData> rows = new List<rowData>();
    public int x;
    public int y;
    
}

public class AnimationController : MonoBehaviour
{
    public Animation2dArray clips;
    private Animator _anim;
    private void Start() {
        _anim = GetComponent<Animator>();
        _anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }
    public void PlayAnimation(int state, int index)
    {
        if(clips.rows[state].row[index].name != null){
            _anim.Play(clips.rows[state].row[index].name);
        }     
        else{
            Debug.LogWarning("there's no animation");
        }  
    }

    public void PlayAnimationAtStart(int state, int index, float start)
    {
        if(clips.rows[state].row[index].name != null){
            _anim.Play(clips.rows[state].row[index].name, -1, start);
        }     
        else{
            Debug.LogWarning("there's no animation");
        }  
    }
}
