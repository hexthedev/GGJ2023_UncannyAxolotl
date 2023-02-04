using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Game : MonoBehaviour
{
    public GridBehaviour Grid;
    public Building Building;

    public AGameAgent[] Agents;
    public Material[] Materials;


    CellData[] HomeBases;

    public float testInterval = 5;
    public float spawnRate = 2;

    void Awake()
    {
        Grid.Init();
        HomeBases = new[] {Grid.Grid[0], Grid.Grid[Grid.Grid.Length-1]};
        
        SpawnBuildingAtIndex(0,0);
        SpawnBuildingAtIndex(1,Grid.Grid.Length-1);

        foreach (CellData homeBase in HomeBases)
            homeBase.Building.StartSpawnUnit(spawnRate, HomeBases[(homeBase.Index+1)%2].Building.transform);

        for (var i = 0; i < Agents.Length; i++)
        {
            Agents[i].Init(i, Grid);
            Agents[i].OnDecidePlaceBuilding += HandlePlaceBuild;
        }

        var tb = gameObject.AddComponent<TimerBehaviour>();
        tb.Interval = testInterval;
        tb.Do = () =>
        {
            foreach (AGameAgent aGameAgent in Agents)
                aGameAgent.MakeBuildingDecision(Grid);
        };
    }

    void HandlePlaceBuild(int player, int coord)
    {
        SpawnBuildingAtIndex(player, coord);

        CellData data = Grid.Grid[coord];
        data.Building.StartSpawnUnit(spawnRate, HomeBases[(data.Index+1)%2].Building.transform);
    }

    void SpawnBuildingAtIndex(int player, int coord)
    {
        CellData cell = Grid.GetCellData(coord);
        cell.PlayerIndex = player;
        cell.IsOccupied = true;
        Building go = Instantiate(Building, Grid.transform);
        cell.Building = go;
        go.SetMaterial(Materials[player]);
        go.transform.localPosition = cell.Position - Vector3.up;
    }
}
