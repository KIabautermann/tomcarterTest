using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private MovementController controller;
    [SerializeField]
    private LayerMask _platform;
    private ISet<Collider> _currentIgnoredColliders;
    private bool _checkResolved;
    private Collider[] _detection;
    private bool _phasingDownwards;
    void Start()
    {
        _currentIgnoredColliders = new HashSet<Collider>();
        _checkResolved = false;
        _detection = new Collider[] {};
    }

    public void LogicExit()
    {
        if (_currentIgnoredColliders.Count > 0) {
            ResolveCollision();
        }
        _checkResolved = false;
        _phasingDownwards = false;
    }

    public void LogicUpdated()
    {
        IgnorePlatformCollider();
    }

    private void IgnorePlatformCollider(){

        if (_checkResolved) { return; }

        _detection = Physics.OverlapBox(controller.myCollider.bounds.center, controller.myCollider.size * 1.5f, Quaternion.identity, _platform);
        // Si esta cayendo, decidimos si eyectarlo por arriba de la plataforma, o dejarlo caer, pero siempre regresamos
        if (!controller.OnPlatform() && Mathf.Abs(controller.CurrentVelocity.y) <= Mathf.Epsilon) {
            ResolveCollision();
            return;
        }
        
        // Si ya hay colliders que se agregaron al conjunto de ignorados, no tiene punto buscar mas
        if (_currentIgnoredColliders.Count > 0) {
            // Si no estamos cayendo, ignora las colisiones ya detectadas
            foreach (Collider collider in _currentIgnoredColliders) {
                Physics.IgnoreCollision(controller.myCollider, collider, true);
            }
            // Si se atraveso la plataforma, reactivar los colliders
            if (controller.OnPlatform()) {
                ResolveCollision();
            }
        } else {
     
            // Detecta alguna colision
            for (int i = 0; i < _detection.Length; i++)
            {
                _phasingDownwards = (controller.CurrentVelocity.y < 0);
                Physics.IgnoreCollision(controller.myCollider, _detection[i], true);
                _currentIgnoredColliders.Add(_detection[i]);
            }
        }
    }
    private void ResolveCollision() 
    {
        for (int i = 0; i < _detection.Length; i++)
        {
            if (_currentIgnoredColliders.Contains(_detection[i])) {
                float currPos_y = controller.myCollider.bounds.max.y;
                // Si el player ya al menos un cuarto de la plataforma, entonces ayudalo a subir
                if (_detection[i].transform.position.y - _detection[i].bounds.size.y * 0.25f <= currPos_y) {
                    // Solo si no esta ya en tierra firme en la plataforma, o no tiene ya mas velocidad vertical que le sobre
                    if (!controller.OnPlatform() || controller.CurrentVelocity.y <= Mathf.Epsilon) {
                        float forceToApply = Mathf.Max(_detection[i].bounds.max.y - controller.myCollider.bounds.min.y, 0) / (_detection[i].bounds.size.y);
                        controller.SetVelocityY(10f * forceToApply);
                    }
                }
                // Si el player esta por debajo de la mitad Y del collider, empujarlo para abajo 
                else {
                    controller.SetVelocityY(-2);
                }
                
                break;
            }
        }
        
        _checkResolved = true;
        
        // Borrar los ignore collider pero con algo de delay para permitirle al player que termine de cruzarlos
        StartCoroutine(RemoveAllCollidersIgnored());
    }

    private IEnumerator RemoveAllCollidersIgnored() 
    {
        float secondsToWait = _phasingDownwards ? 1f : 0.1f;
        yield return new WaitForSeconds(secondsToWait);
        foreach (Collider collider in _currentIgnoredColliders) {
            Physics.IgnoreCollision(controller.myCollider, collider, false);
        }
        
        _currentIgnoredColliders = new HashSet<Collider>();
        
    }
}
