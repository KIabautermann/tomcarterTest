using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgeManager : MonoBehaviour
{
    public List<Hedge> hedgesA = new List<Hedge>();
    public List<Hedge> hedgesB = new List<Hedge>();
    public List<Hedge> hedgesC = new List<Hedge>();
    public int status;
    private int previousStatus;
    public bool A;
    public bool B;
    public bool C;
    public HedgeColors colorData;

    private void Awake()
    {
        for (int i = 0; i < hedgesA.Count; i++)
        {
            hedgesA[i].SetColors(colorData);
        }
        for (int i = 0; i < hedgesB.Count; i++)
        {
            hedgesB[i].SetColors(colorData);
        }
        for (int i = 0; i < hedgesC.Count; i++)
        {
            hedgesC[i].SetColors(colorData);
        }
    }
    void Start()
    {
        UpdateHedges();
    }

    void Update()
    {
        if (A & status !=0)
        {
            SetStatus(0);
            B = false;
            C = false;
        }
        if (B & status != 1)
        {
            SetStatus(1);
            A = false;
            C = false;
        }
        if (C & status != 2)
        {
            SetStatus(2);
            A = false;
            B = false;
        }

        if (previousStatus != status) UpdateHedges();
    }

    public void UpdateHedges()
    {
        status = Mathf.Clamp(status,0, 2);
        switch (status)
        {
            case 0:
                for (int i = 0; i < hedgesA.Count; i++)
                {
                    hedgesA[i].Set(true);
                }
                for (int i = 0; i < hedgesB.Count; i++)
                {
                    hedgesB[i].Set(true);
                }
                for (int i = 0; i < hedgesC.Count; i++)
                {
                    hedgesC[i].Set(false);
                }
                break;
            case 1:
                for (int i = 0; i < hedgesA.Count; i++)
                {
                    hedgesA[i].Set(true);
                }
                for (int i = 0; i < hedgesB.Count; i++)
                {
                    hedgesB[i].Set(false);
                }
                for (int i = 0; i < hedgesC.Count; i++)
                {
                    hedgesC[i].Set(true);
                }
                break;
            case 2:
                for (int i = 0; i < hedgesA.Count; i++)
                {
                    hedgesA[i].Set(false);
                }
                for (int i = 0; i < hedgesB.Count; i++)
                {
                    hedgesB[i].Set(true);
                }
                for (int i = 0; i < hedgesC.Count; i++)
                {
                    hedgesC[i].Set(true);
                }
                break;            
        }
        previousStatus = status;
    }

    public void SetStatus(int newStatus)
    {
        status = newStatus;
    }
}
