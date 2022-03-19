using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable 
{
    public void TakeDamage(int Damage, float attackDuration);

    public void Heal(int heal);

    public void Death();

    public IEnumerator InvulnerabilityTime();

    public IEnumerator DamageFlash();
}
