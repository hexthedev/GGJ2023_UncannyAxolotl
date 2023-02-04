using System;
using System.Linq;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public CellBehaviour Prefab;

    public event Action<int> OnCellClicked; 

    public int X;
    public int Y;

    public float Spacing;
    public Vector3 Scale;
    public float CellOffset;
    
    public CellData[] Grid;

    void Start()
    {
        SpawnGrid();
        SetInteractable();
    }

    public void SetInteractable(params Vector2[] coords)
    {
        foreach (CellData cellData in Grid)
            cellData.Viz.gameObject.SetActive(false);

        foreach (int index in coords.Select(c => GetIndex(c)))
            Grid[index].Viz.gameObject.SetActive(true);
    }

    public CellData GetCellData(int index) => Grid[index];
    public CellData GetCellData(Vector2 index) => Grid[GetIndex(index)];


    void SpawnGrid()
    {
        if (Grid != null)
        {
            foreach (CellData cellData in Grid)
                Destroy(cellData.Viz);
        }

        int total = X * Y;
        Grid = new CellData[X * Y];
        
        for (int index = 0; index < total; index++)
        {
            Vector2 vec2 = GetCoord(index);
            Vector3 pos = new Vector3(vec2.x * Spacing, 1, vec2.y * Spacing);

            Grid[index] = new CellData()
            {
                Index = index,
                Viz = Instantiate(Prefab, transform),
                Position = pos
            };

            Grid[index].Viz.transform.localPosition = pos + new Vector3(0,CellOffset,0);
            Grid[index].Viz.transform.localScale = Scale;

            int pin = index;
            Grid[index].Viz.OnClicked += () => OnCellClicked?.Invoke(pin);
        }
    }

    public int GetIndex(Vector2 vec) => (int)(vec[0] + vec[1] * X);
    public Vector2 GetCoord(int coord) => new Vector2(coord % X, coord / X);
}