using System;
using System.Collections.Generic;
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

    public float enemyChoiceInterval;
    TimerBehaviour _enemyTimer;

    public float playerResourceGainPerSecond;
    public float playerMaxResource;
    TimerBehaviour _playerTimer;

    public GameOverSceen GameOverScreen;
    public PlayerUIController PlayerUI;
    
    public UnitConfig[] UnitConfigs;

    bool isChoosingSpace = false;

    public List<Building> SpawnedBuildings = new();

    bool isFirstInit = false;

    BuildingConfig playerLastChosenBuilding = null;
    
    public AudioSource placeBuilding;
    public AudioSource gameOver;
        
    public void ResetGame()
    {
        // Cleanup
        foreach (TimerBehaviour timerBehaviour in gameObject.GetComponents<TimerBehaviour>())
            Destroy(timerBehaviour);

        foreach (Building spawnedBuilding in SpawnedBuildings)
        {
            if(spawnedBuilding != null)
                Destroy(spawnedBuilding.gameObject);
        }
        
        SpawnedBuildings.Clear();

        GameOverScreen.gameObject.SetActive(false);

        // Start
        Grid.Init();
        HomeBases = new[] {Grid.Grid[0], Grid.Grid[Grid.Grid.Length-1]};
         
        SpawnBuildingAtIndex(0,0, buildings[0]);
        SpawnBuildingAtIndex(1,Grid.Grid.Length-1, buildings[0]);

        foreach (CellData homeBase in HomeBases)
            homeBase.Building.StartSpawnUnit(HomeBases[(homeBase.Index+1)%2].Building.transform);


        _enemyTimer = gameObject.AddComponent<TimerBehaviour>();
        _enemyTimer.Interval = enemyChoiceInterval;
        _enemyTimer.Do = () =>
        {
            Agents[1].MakeBuildingDecision(Grid);
            _enemyTimer.CurrentTime += Random.Range(-2, 2);
        };

        _playerTimer = gameObject.AddComponent<TimerBehaviour>();
        _playerTimer.Interval = float.PositiveInfinity;
        
        // events
        if (!isFirstInit)
        {
            for (var i = 0; i < Agents.Length; i++)
            {
                Agents[i].Init(i, Grid);
                Agents[i].OnDecidePlaceBuilding += HandlePlaceBuild;
            }
            PlayerUI.OnBuildingClicked += PlayerUIOnOnBuildingClicked;
        }

        isFirstInit = true;
    }

    void PlayerUIOnOnBuildingClicked(int obj)
    {
        isChoosingSpace = true;

        playerLastChosenBuilding = buildings[obj];
        Agents[0].MakeBuildingDecision(Grid);
    }


    void HandlePlaceBuild(int player, int coord)
    {
        AudioSource s = Instantiate(placeBuilding);
        s.Play();
        if (player == 0)
        {
            _playerTimer.CurrentTime = 0;
            isChoosingSpace = false;
        }
        
        SpawnBuildingAtIndex(
            player, 
            coord,
            player == 0 ? 
                playerLastChosenBuilding :
                buildings[Random.Range(1, buildings.Length)]
        );

        CellData data = Grid.Grid[coord];
        data.Building.StartSpawnUnit(HomeBases[(data.Index+1)%2].Building.transform);
    }

    void SpawnBuildingAtIndex(int player, int coord, BuildingConfig building)
    {
        CellData cell = Grid.GetCellData(coord);
        cell.PlayerIndex = player;
        cell.IsOccupied = true;

        Building go = Instantiate(building.buildingPrefab, Grid.transform);
        go.buildingConfig = building;
        go.Team = player;
        cell.Building = go;
        go.SetMaterial(Materials[player]);
        go.transform.localPosition = cell.Position - Vector3.up;
        SpawnedBuildings.Add(go);
    }

    void Update()
    {
        if (!isFirstInit)
            return;
        
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
        AudioSource s = Instantiate(gameOver);
        s.Play();
        // Stop the choice loop. 
        Destroy(GetComponent<TimerBehaviour>());
        
        // Show a message on the screen
        string message = i == 1 ? "You have saved the forest! :)" : "The forest has been taken :(";
        
        GameOverScreen.SetText(message);
        GameOverScreen.Show();
    }
}
