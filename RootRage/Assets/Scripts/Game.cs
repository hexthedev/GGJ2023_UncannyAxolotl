using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GridBehaviour Grid;
    public Building Building;

    
    public AGameAgent[] Agents;
    public Material[] Materials;
    
    List<int> _takenIndices = new();

    public float testInterval = 5;
    float curtime = 0;
    
    void Start()
    {
        SpawnBuildingAtIndex(0,Grid.GetIndex(new Vector2(0,0)));
        SpawnBuildingAtIndex(1,Grid.GetIndex(new Vector2(4,4)));

        for (var i = 0; i < Agents.Length; i++)
        {
            Agents[i].Init(i);
            Agents[i].OnDecidePlaceBuilding += SpawnBuildingAtIndex;
        }
    }

    void Update()
    {
        curtime += Time.deltaTime;

        if (curtime > testInterval)
        {
            foreach (AGameAgent aGameAgent in Agents)
                aGameAgent.MakeBuildingDecision(Grid);

            curtime = 0;
        }
    }

    void SpawnBuildingAtIndex(int player, int coord)
    {
        CellData cell = Grid.GetCellData(coord);
        cell.PlayerIndex = player;
        cell.IsOccupied = true;
        Building go = Instantiate(Building, Grid.transform);
        go.SetMaterial(Materials[player]);
        go.transform.localPosition = cell.Position - Vector3.up;
        _takenIndices.Add(coord);
    }
}
