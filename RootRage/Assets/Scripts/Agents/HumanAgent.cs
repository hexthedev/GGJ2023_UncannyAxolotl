using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "UncannyAxolotl/Agents/Human")]
public class HumanAgent : AGameAgent
{
    public override event Action<int, int> OnDecidePlaceBuilding;

    public override void Init(int id, GridBehaviour grid)
    {
        base.Init(id, grid);
        Grid.OnCellClicked += HandleDecide;
    }

    public override void MakeBuildingDecision(GridBehaviour gridBehaviour)
    {
        List<CellData> x = gridBehaviour.Grid.Where(c => c.IsOccupied && c.PlayerIndex == Id).ToList();
        int[] neighbours = x
            .SelectMany(x => gridBehaviour.GetNeighbours(x.Index))
            .Distinct()
            .Where(i => !gridBehaviour.GetCellData(i).IsOccupied)
            .ToArray();

        gridBehaviour.SetInteractable(neighbours);
    }
    
    void HandleDecide(int i)
    {
        Grid.SetInteractable();
        OnDecidePlaceBuilding?.Invoke(Id, i);
    }
}