using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    [SerializeField]
    protected MaterialsRepository enemymaterials;
    protected SpriteRenderer myRenderer;
    protected float iFrames;
    [SerializeField]
    protected int maxHealth;
    protected int currentHealth;
    private bool vulnerable;
    private bool deathLerp;
    private bool damageLerp;
    private float deathIndex;
    private float damageIndex;
    private Animator _animator;
    
    public IEnumerator DamageFlash()
    {
        damageLerp = true;
        yield return new WaitForSeconds(.1f);
        damageLerp = false;
        StopCoroutine(DamageFlash());
    }

    public void Heal(int heal)
    {
        //enemies can't heal...i hope
    }

    public void Death()
    {
        Debug.Log("PELOTUDO DE MIERDA, MATASTE A BUSHY, A VOS TE PARECE?");
        _animator.SetTrigger("death");
    }

    public IEnumerator InvulnerabilityTime()
    {
        yield return new WaitForSeconds(iFrames);
        vulnerable = true;
        StopCoroutine(InvulnerabilityTime());
    }

    public void TakeDamage(int damage, float attackduration)
    {
        if (vulnerable)
        {
            vulnerable = false;
            iFrames = attackduration;
            currentHealth += -damage;
            if (currentHealth > 0)
            {
                StartCoroutine(DamageFlash());
                StartCoroutine(InvulnerabilityTime());
            }
            else deathLerp = true;
        }     
    }

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        vulnerable = true;
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if((damageLerp && damageIndex!=1) || (!damageLerp && damageIndex != 0))
        {
            if (damageLerp) damageIndex += Time.deltaTime / .05f;
            else damageIndex += -Time.deltaTime / .05f;
            damageIndex = Mathf.Clamp(damageIndex, 0, 1);
            myRenderer.material.SetFloat("_damage", damageIndex);
        }
        if (deathLerp && deathIndex!=1)
        {
            deathIndex += Time.deltaTime / .05f;
            deathIndex = Mathf.Clamp(deathIndex, 0, 1);
            myRenderer.material.SetFloat("_death", deathIndex);
            if (deathIndex >= 1) Death();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
