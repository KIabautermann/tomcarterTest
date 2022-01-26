using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    public IEnumerator DamageFlash()
    {
        yield return new WaitForSeconds(.1f);

        StopCoroutine(DamageFlash());
    }

    public void TakeDamage(int Damage)
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
