using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxGenerator : MonoBehaviour
{
    public void CircleHitbox(Vector2 center, float radius, int damage, float duration)
    {
        Collider[] hitbox = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < hitbox.Length; i++)
        {
            if(hitbox[i].GetComponent<IHitable>() != null)
            {
                hitbox[i].GetComponent<IHitable>().TakeDamage(damage, duration);
            }
        }
    }

    public void BoxHitbox(Vector2 center, Vector2 size, int damage, float duration)
    {
        Collider[] hitbox = Physics.OverlapBox(center, size/2);
        for (int i = 0; i < hitbox.Length; i++)
        {
            if (hitbox[i].GetComponent<IHitable>() != null)
            {
                hitbox[i].GetComponent<IHitable>().TakeDamage(damage, duration);
                Debug.Log(hitbox[i].gameObject.name);
            }
        }
    }
    
}
