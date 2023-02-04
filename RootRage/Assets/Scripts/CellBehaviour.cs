using System;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public event Action OnClicked;

    public Material Default;
    public Material Over;
    public Material Down;

    MeshRenderer mr;

    bool isOver = false;
    
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.material = Default;
    }

    void OnMouseEnter()
    {
        mr.sharedMaterial = Over;
        isOver = true;
    }

    void OnMouseExit()
    {
        isOver = false;
        mr.material = Default;
    }

    void OnMouseDown()
    {
        mr.material = Down;
    }

    void OnMouseUpAsButton()
    {
        if (isOver)
            mr.material = Over;
        else
            mr.material = Default;
        
        OnClicked?.Invoke();
    }
}