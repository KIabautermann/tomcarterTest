using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedLine : MonoBehaviour
{
    private LineRenderer _myRenderer;
    private int animationIndex;
    private float counter;
    [SerializeField]
    private Texture[] _textures;
    [SerializeField]
    private float _fps;


    private void Awake()
    {
        _myRenderer = GetComponent<LineRenderer>();      
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if(counter >= 1f / _fps){
            animationIndex++;
            if (animationIndex == _textures.Length){
                animationIndex = 0;
            }
            _myRenderer.material.SetTexture("_MainTex", _textures[animationIndex]);
            counter = 0;
        }
    }
}
