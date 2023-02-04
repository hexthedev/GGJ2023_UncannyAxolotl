using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "UncannyAxolotl/Agents/Human")]
public class HumanAgent : AGameAgent
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

        gridBehaviour.SetInteractable(neighbours);

        foreach (int neighbour in neighbours)
            gridBehaviour.OnCellClicked += HandleDecide;

        void HandleDecide(int i)
        {
            gridBehaviour.SetInteractable();
            
            foreach (int neighbour in neighbours)
                gridBehaviour.OnCellClicked -= HandleDecide;

            OnDecidePlaceBuilding?.Invoke(Id, i);
        }
    }
}