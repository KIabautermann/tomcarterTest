using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSpawner", menuName = "BulletSpawn")]
public class ProjectileSpawner : ObjectPooler
{
   
    public Projectile Shoot(Vector2 position, Vector2 direction, int quantity, float angle, float speed, Transform parent)
    {
        Quaternion tempAngleA = Quaternion.Euler(new Vector3(0, 0, angle));
        Quaternion tempAngleB = Quaternion.Euler(new Vector3(0, 0, -angle));
        for (int i = 0; i <= quantity - 1; i++)
        {
            Vector3 tempDirection = quantity > 1 ? Vector3.Slerp(tempAngleA * direction, tempAngleB * direction, (float)i / (quantity - 1)) : direction;
            ComponentCache<MonoBehaviour> bulletComponets = GetItem(position, Quaternion.identity);
            bulletComponets.GetInstance(typeof(Projectile), out MonoBehaviour tmp);
            Projectile pais = tmp as Projectile;
            pais.Set(tempDirection, speed);
            if (parent != null) pais.gameObject.transform.SetParent(parent.transform, true);
        }
        return null;
    }
}

