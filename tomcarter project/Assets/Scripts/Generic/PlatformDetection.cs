using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    public Collider _myCollider;
    private List<Collider> _ignoredList;

    private void Awake()
    {
        _ignoredList = new List<Collider>();
    }
    private void Start(){
        _myCollider = GetComponent<Collider>();
    }
    private void Update(){
        IgnoreCheck();
    }
    void IgnoreCheck(){
        Collider[] detection = Physics.OverlapBox(_myCollider.bounds.center, (_myCollider.bounds.size + Vector3.one) / 2, Quaternion.identity);
        if (_ignoredList.Count != 0){
            for (int i = 0; i < _ignoredList.Count; i++){
                for (int j = 0; j < detection.Length; j++){
                    if (detection[j].Equals(_ignoredList[i])){
                        Physics.IgnoreCollision(_myCollider, detection[i], true);
                    }
                    else{
                        Physics.IgnoreCollision(_myCollider, detection[j], Ignore(detection[j]));
                    }
                    if (_ignoredList[i].bounds.min.y > _myCollider.bounds.max.y || _ignoredList[i].bounds.max.y < _myCollider.bounds.min.y){
                        _ignoredList.Remove(_ignoredList[i]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < detection.Length; i++)
            {
                Physics.IgnoreCollision(_myCollider, detection[i], Ignore(detection[i]));
            }
        }
    }
    bool Ignore(Collider target){
        if(target.bounds.min.y > _myCollider.bounds.max.y -.2f){
            return false;
        }
        return true;
    }
    public void IgnoreAnyways(Collider ignored)
    {
        Physics.IgnoreCollision(ignored, _myCollider, true);
        _ignoredList.Add(ignored);
    }
}
