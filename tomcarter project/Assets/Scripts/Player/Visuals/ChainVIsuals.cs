using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainVIsuals : MonoBehaviour
{
    [Range(1, 20)]
    public int numberOfSegments;
    public Transform finalPos;
    public GameObject prebafPart;
    public GameObject remain;
    private Vector3 initalPos;
    private SpriteRenderer hookRenderer;
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    private List<GameObject> parts = new List<GameObject>();
    void Start()
    {
        initalPos = transform.position;
        hookRenderer = GetComponent<SpriteRenderer>();
        SetRenderers();
    }

    void Update()
    {
        SetPartsPosition();
    }

    private void SetPartsPosition()
    {

        if (numberOfSegments + 1 != parts.Count)
        {
            Change();
            SetRenderers();
        }
        for (int i = 0; i < parts.Count; i++)
        {
            float position = (1f / parts.Count * i);
            parts[i].transform.position = Vector3.Lerp(transform.position, finalPos.position, position);
        }
    }

    void Change()
    {
        float amount = Mathf.Abs(numberOfSegments + 1 - parts.Count);
        if (numberOfSegments + 1 < parts.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject current = parts[parts.Count - 1];
                parts.RemoveAt(parts.Count - 1);
                Destroy(current);
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject current = Instantiate(prebafPart);
                parts.Add(current);
                current.transform.parent = transform;
            }            
        }
    }

    private void SetRenderers()
    {
        renderers.Clear();
        for (int i = 0; i < parts.Count; i++)
        {
            renderers.Add(parts[i].GetComponent<SpriteRenderer>());
        }
    }

    public void TurnRenderers(bool set)
    {
        hookRenderer.enabled = set;
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = set;
        }
        if (!set)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                GameObject tempRemain = remain;
                Instantiate(tempRemain, parts[i].transform.position, Quaternion.identity);
            }
        }
    }
}
