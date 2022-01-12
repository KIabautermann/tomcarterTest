using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainVisualEffect : MonoBehaviour
{
    
    private List<GameObject> chain = new List<GameObject>();
    [Range(1,10)]
    private int quantity;
    private Transform pointA, pointB;
    public VisualEffectSpawner vfxSpawn;

    private void Update()
    {     
        for(int i = 0; i<chain.Count; i++)
        {
            float lerpIndex = (float)1 / quantity ; 
            chain[i].transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpIndex * (i + 1));
        }
    }
    public void AssignPoints(Transform A, Transform B)
    {
        pointA = A;
        pointB = B;
    }
    public void ListUpdate(int newQuantity)
    {
        for(int i = 0; i < chain.Count; i++)
        {
            chain[i].GetComponent<PoolableObject>().Dispose();
        }
        chain.Clear();
        quantity = newQuantity;
        for (int i = 0; i < quantity; i++)
            {
            chain.Add(vfxSpawn.InstanceEffect(null, transform.position, Quaternion.identity, vfxSpawn.EffectRepository.playerVineIdle).gameObject);
        }
    }

    public void EndChain()
    {
        for (int i = 0; i < chain.Count; i++)
        {
            chain[i].GetComponent<Animator>().SetTrigger("end");
        }
        pointA = null;
        pointB = null;
        chain.Clear();
    }
}
