using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "UncannyAxolotl/Agents/Enemy")]
public class EnemyRandomAgent : AGameAgent
{
    public override event Action<int, int> OnDecidePlaceBuilding;

    public override void MakeBuildingDecision(GridBehaviour gridBehaviour)
    {
        List<CellData> x = gridBehaviour.Grid.Where(c => c.IsOccupied && c.PlayerIndex == Id).ToList();
        int[] neighbours = x
            .SelectMany(x => gridBehaviour.GetNeighbours(x.Index))
            .Distinct()
            .Where(i => !gridBehaviour.GetCellData(i).IsOccupied)
            .ToArray();
        
        if(neighbours.Length != 0)
            OnDecidePlaceBuilding?.Invoke(Id, neighbours[Random.Range(0, neighbours.Length)]);
    }
}