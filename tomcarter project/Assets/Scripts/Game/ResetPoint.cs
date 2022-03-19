using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetPoint : MonoBehaviour
{   
    private SaveLoadController controller;
    private void Start() {
        controller = FindObjectOfType<SaveLoadController>();
    }
    [SerializeField]
    private LayerMask _playerLayer;
    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.layer == Math.Log(_playerLayer.value, 2)) {
          controller.ResetGame();
      }
    }
}
