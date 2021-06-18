using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    public Collider _myCollider;
    private List<Collider> _ignoredList;
    private int _layerMask;

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
        Collider[] detection = Physics.OverlapBox(_myCollider.bounds.center, (_myCollider.bounds.size + Vector3.one) / 2, Quaternion.identity, _layerMask);
        for (int i = 0; i < detection.Length; i++)
        {
            Physics.IgnoreCollision(_myCollider, detection[i], Ignore(detection[i]));
        }
    }
    bool Ignore(Collider target){
        if(target.bounds.min.y - _myCollider.bounds.max.y < .2f){
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
