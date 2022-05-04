using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Respawner : MonoBehaviour
{
    private Vector3 lastSafeZone;
    private MovementController movement;
    private Coroutine safeZoneCheckRoutine;
    public PlayerData data;

    [SerializeField]
    public float hazardCheckRadius;
    [SerializeField]
    public float solidGroundMinimum;
    [SerializeField]
    public float x_maxColliderProximity;
    void Start()
    {
        movement = GetComponent<MovementController>();
        lastSafeZone = this.gameObject.transform.position;
        safeZoneCheckRoutine = StartCoroutine(CheckForSafeZone());
        PlayerEventSystem.GetInstance().OnHazardHit += HazardHitHandler;
    }

    private void HazardHitHandler(object sender, EventArgs args) 
    {
        this.gameObject.transform.position = lastSafeZone;
    }
    
    private void OnDestroy() {
        if (safeZoneCheckRoutine != null) StopCoroutine(safeZoneCheckRoutine);
        PlayerEventSystem.GetInstance().OnHazardHit -= HazardHitHandler;
    }

    private IEnumerator CheckForSafeZone() 
    {
        // Todo: No permitir hacer una safe zone si esta con una skill de uso limitado/no desbloqueado (y ya se gasto al menos un uso??)
        while (true) {
            yield return new WaitForSeconds(1);
            
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, hazardCheckRadius, data.hazard);
            if (colliders.Length != 0) {
                continue;
            }
            // No esta en el suelo
            if (!movement.Grounded()) {
               continue; 
            }
            
            // Esta muy cerca de una pared
            colliders = Physics.OverlapBox(transform.position, new Vector3(x_maxColliderProximity, movement.myCollider.size.y / 4, 0.1f));

            int wallCollision = new List<Collider>(colliders).Where(x => x.gameObject != gameObject).Count();

            if (wallCollision > 0) {
                continue;
            }

            RaycastHit groundHit = movement.GetGroundHit();
            var ground = groundHit.collider.gameObject;

            // No esta sobre tierra firme (no hedge)
            if (Mathf.Pow(2, ground.layer) != data.solidGround.value) {
                continue;
            }

            // No es ideal pero si es una vez cada 3 segundos, tampoco es para tanto bardo la eficiencia
            BoxCollider groundCollider = ground.GetComponent<BoxCollider>();
            
            Vector3 min = groundCollider.bounds.min;
            Vector3 max = groundCollider.bounds.max;
            Vector3 currPos = this.gameObject.transform.position;
            
            // Tener cuidado que esto no va a funcionar en pozos, si el walkable de abajo no es el mismo game object
            // que el de las paredes al costado
            if (min.x < currPos.x - solidGroundMinimum && max.x > currPos.x + solidGroundMinimum) {
                lastSafeZone = this.gameObject.transform.position;
            }
        }
        
    }

}
