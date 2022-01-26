using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable 
{
    public void TakeDamage(int Damage);

    public IEnumerator DamageFlash();
}
