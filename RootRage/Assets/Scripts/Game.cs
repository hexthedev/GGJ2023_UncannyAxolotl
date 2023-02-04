using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public GridBehaviour Grid;
    public GameObject Building;

    List<int> _takenIndices = new();

    float totalTime = 5;
    float curtime = 0;
    
    void Start()
    {
        Grid.OnCellClicked += SpawnBuildingAtIndex;
        
        SpawnBuildingAtIndex(Grid.GetIndex(new Vector2(0,0)));
        SpawnBuildingAtIndex(Grid.GetIndex(new Vector2(4,4)));
    }

    void Update()
    {
        curtime += Time.deltaTime;

        if (curtime > totalTime)
        {
            int take = Random.Range(0, 25);

            while (_takenIndices.Contains(take))
                take = Random.Range(0, 25);
            
            Grid.SetInteractable(Grid.GetCoord(take));
            curtime = 0;
        }
    }

    void SpawnBuildingAtIndex(int coord)
    {
        CellData cell = Grid.GetCellData(coord);
        GameObject go = Instantiate(Building, Grid.transform);
        go.transform.localPosition = cell.Position - Vector3.up;
        _takenIndices.Add(coord);
    }
}
