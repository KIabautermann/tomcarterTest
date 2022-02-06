using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : Enemy
{
    public float attackTime;
    public float searchTime;
    public LayerMask targetMask;
    public LayerMask walkable;
    public float detectionRadius;
    [SerializeField]
    private Transform _target;
    private float _attackCounter;
    private float _searchCounter;
    private Vector3 attackPosition;

    public override void Update()
    {
        base.Update();
        if(currentHealth > 0)
        {
            _searchCounter += Time.deltaTime;
            if (_searchCounter > searchTime)
            {
                Collider[] detection = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
                if (detection.Length != 0)
                {
                    if(_target==null) _attackCounter = 0;
                    _target = detection[0].GetComponent<Transform>();
                }
                else _target = null;
                _searchCounter = 0;
            }          
            if (_target != null)
            {
                _attackCounter += Time.deltaTime;
                float direction = _target.position.x - transform.position.x;
                Vector3 rotationVector = new Vector3(0, direction > 0 ? 0 : 180, 0);
                transform.rotation = Quaternion.Euler(rotationVector);
                if (_attackCounter > attackTime)
                {
                    RaycastHit hit;
                    Physics.Raycast(_target.position, -Vector3.up, out hit, detectionRadius * 2, walkable);
                    attackPosition = hit.point;
                    _animator.SetTrigger("attack");
                    _attackCounter = 0;
                }
            }
        }      
    }

    public void Attack()
    {
        vfxSpawn.InstanceEffect(null, attackPosition, Quaternion.Euler(0, 0, 90), vfxSpawn.EffectRepository.rootAttack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


}
