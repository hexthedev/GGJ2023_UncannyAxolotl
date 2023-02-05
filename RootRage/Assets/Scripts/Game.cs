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
    
    CellData[] HomeBases;

    public float spawnRate = 2;

    public float enemyChoiceInterval;
    TimerBehaviour _enemyTimer;

    public float playerResourceGainPerSecond;
    public float playerMaxResource;
    TimerBehaviour _playerTimer;

    public GameOverSceen GameOverScreen;
    public PlayerUIController PlayerUI;
    
    public UnitConfig[] UnitConfigs;

    bool isChoosingSpace = false;
    
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

        _enemyTimer = gameObject.AddComponent<TimerBehaviour>();
        _enemyTimer.Interval = enemyChoiceInterval;
        _enemyTimer.Do = () => Agents[1].MakeBuildingDecision(Grid);

        _playerTimer = gameObject.AddComponent<TimerBehaviour>();
        _playerTimer.Interval = float.PositiveInfinity;
        
        PlayerUI.OnBuildingClicked += PlayerUIOnOnBuildingClicked;
    }

    void PlayerUIOnOnBuildingClicked(int obj)
    {
        isChoosingSpace = true;
        
        Agents[0].MakeBuildingDecision(Grid);
    }


    void HandlePlaceBuild(int player, int coord)
    {
        if (player == 0)
        {
            _playerTimer.CurrentTime = 0;
            isChoosingSpace = false;
        }
        
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
        // update the player ui
        if(!isChoosingSpace)
            PlayerUI.SetResource(_playerTimer.CurrentTime/playerMaxResource);

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
