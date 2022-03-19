using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeTrailEffect : PoolableObject
{
    private GameObject poolerParent;
    private ParticleSystem sporeParticleSystem;
    public override void ResetSceneReferences()
    {
        sporeParticleSystem.Stop();
        this.gameObject.transform.SetParent(poolerParent.transform);
    }

    protected override void Start()
    {
        sporeParticleSystem = GetComponentInChildren<ParticleSystem>();
        poolerParent = this.gameObject.transform.parent.gameObject;
        this.gameObject.transform.position = Vector3.zero;
        base.Start();
    }

    public void LogicStart()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        sporeParticleSystem.Play();
    }

    public void LogicEnd()
    {
        StartCoroutine(FadeOutAndDispose());
    }

    private IEnumerator FadeOutAndDispose()
    {
        // Dejamos un extra de particulas antes de frenar para casos como entrando a un Hedge
        yield return new WaitForSeconds(0.05f);
        
        sporeParticleSystem.Stop();

        // Dar tiempo para que las particulas se desvanezcan solas
        yield return new WaitForSeconds(0.5f);;
        Dispose();
    }
}
