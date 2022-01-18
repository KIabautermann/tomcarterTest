using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class MapManager : MonoBehaviour
{
    public Transform pointZero, target;
    public GameObject iconPrefab, BackgroundPrefab;
    public int unitSize;
    public int tilesize;
    private Vector2 _coordinates;
    private GameObject _icon;
    private RectTransform _background;
    public float mapUnitsize;
    public int rows, cols;
    public Transform UILocation;
    public bool _active;
    private bool _wasActive;
    RectTransform mapRect;



    private void Update()
    {
        UpdateIcon();
        if (_wasActive != _active) toggleMap();
    }

    private void Start()
    {
        UILocation.gameObject.SetActive(_active);
        mapRect = UILocation.GetComponent<RectTransform>();
        mapUnitsize = mapRect.sizeDelta.x;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

            }
        }
        _background = Instantiate(BackgroundPrefab.GetComponent<RectTransform>(), UILocation);
        _background.localPosition = Vector3.zero;
        _background.sizeDelta = new Vector2(cols * mapUnitsize, rows * mapUnitsize);
        _icon = Instantiate(iconPrefab, UILocation);
        UpdateIcon();
    }
    private void UpdateIcon()
    {
        if (_active)
        {
            int x = (int)((target.position.x - pointZero.position.x) / unitSize);
            int y = (int)((target.position.y - pointZero.position.y) / unitSize);
            _coordinates.Set(x, y);
            float posX = _coordinates.x * tilesize - cols/2;
            float posY = _coordinates.y * -tilesize + rows/2;
            _icon.transform.localPosition = new Vector2(posX, posY) * mapUnitsize;
        }    
    }

    private void toggleMap()
    {
        UILocation.gameObject.SetActive(!UILocation.gameObject.active);
        _wasActive = _active;
    }

}
