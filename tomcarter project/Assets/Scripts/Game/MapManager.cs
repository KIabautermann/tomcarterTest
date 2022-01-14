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
    public Vector2 _coordinates;
    private GameObject icon;
    public int rows, cols;
    public Transform UILocation;



    private void Update()
    {
        UpdateIcon();
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GameObject referenceTile = Instantiate(BackgroundPrefab);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject tile = Instantiate(referenceTile, UILocation);
                float posX = j * tilesize;
                float posY = i * -tilesize;
                tile.transform.localPosition = new Vector2(posX, posY) * 100;
            }
        }
        icon = Instantiate(iconPrefab, UILocation);
        UpdateIcon();
        Destroy(referenceTile);
    }
    private void UpdateIcon()
    {
        int x = (int)Mathf.Abs((target.position.x - pointZero.position.x)/unitSize);
        int y = (int)Mathf.Abs((target.position.y - pointZero.position.y) / unitSize);
        _coordinates.Set(x, y);
        float posX = _coordinates.x * tilesize;
        float posY = _coordinates.y * -tilesize;
        icon.transform.localPosition = new Vector2(posX, posY) * 100;       
    }
}
