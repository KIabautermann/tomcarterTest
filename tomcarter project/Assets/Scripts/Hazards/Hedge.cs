using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedge : MonoBehaviour
{
    public bool _active;
    public Color[] typeColors = new Color[2];
    private SpriteRenderer _renderer;
    [SerializeField]
    private float _colorLerp;
    public int type;
    private HedgeColors _colors;
    void Start()
    {
        type = Mathf.Clamp(type, 0, 2);
        switch (type)
        {
            case 0:
                typeColors[0] = _colors.disabledA;
                typeColors[1] = _colors.enabledA;
                break;
            case 1:
                typeColors[0] = _colors.disabledB;
                typeColors[1] = _colors.enabledB;
                break;
            case 2:
                typeColors[0] = _colors.disabledC;
                typeColors[1] = _colors.enabledC;
                break;
        }
        _renderer = GetComponent<SpriteRenderer>();
        _colorLerp = _active ? 1 : 0;
    }

    void Update()
    {
        if(_active)
        {
            _colorLerp += Time.deltaTime * 2;
            _colorLerp = Mathf.Clamp(_colorLerp, 0, 1);
        }
        else
        {
            _colorLerp += -Time.deltaTime * 2;
            _colorLerp = Mathf.Clamp(_colorLerp, 0, 1);
        }
        _renderer.color = Color.Lerp(typeColors[0], typeColors[1], _colorLerp);
    }

    public void Set(bool active)
    {
        _active = active;
        if (active) gameObject.layer = 10;
        else gameObject.layer = 8;
    }


    public void SetColors(HedgeColors newcolors)
    {
        _colors = newcolors;
    }
}
