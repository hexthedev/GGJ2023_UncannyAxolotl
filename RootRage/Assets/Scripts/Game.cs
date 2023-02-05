using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public GridBehaviour Grid;
    public BuildingConfig[] buildings;

    public AGameAgent[] Agents;
    public Material[] Materials;
    public TMP_Text[] TimerTexts;

    CellData[] HomeBases;

    public float choiceInterval = 5;
    public float spawnRate = 2;

    TimerBehaviour _choiceTimer;

    public GameOverSceen GameOverScreen; 
    
    public UnitConfig[] UnitConfigs;
    
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

        _choiceTimer = gameObject.AddComponent<TimerBehaviour>();
        _choiceTimer.Interval = choiceInterval;
        _choiceTimer.Do = () =>
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
        var buildingConfig = buildings[Random.Range(0, buildings.Length)];
        Building go = Instantiate(buildingConfig.buildingPrefab, Grid.transform);
        go.buildingConfig = buildingConfig;
        go.Team = player;
        cell.Building = go;
        go.SetMaterial(Materials[player]);
        go.transform.localPosition = cell.Position - Vector3.up;
    }

    void Update()
    {
        foreach (TMP_Text tmpText in TimerTexts)
            tmpText.text = $"{_choiceTimer.CurrentTime.ToString("F1")}/{_choiceTimer.Interval.ToString("F1")}";

        for (var i = 0; i < HomeBases.Length; i++)
        {
            if (HomeBases[i].Building == null || HomeBases[i].Building.currentHP <= 0)
            {
                GameOverForPlayer(i);
            }
        }
    }

    void GameOverForPlayer(int i)
    {
        // Stop the choice loop. 
        Destroy(GetComponent<TimerBehaviour>());
        
        // Show a message on the screen
        string message = i == 1 ? "You have saved the forest!" : "Oh no! The forest has been take. You lose";
        
        GameOverScreen.SetText(message);
        GameOverScreen.Show();
    }
}
