using System;
using UnityEngine;

public abstract class AGameAgent: ScriptableObject
{
    protected int Id = 0;
    protected GridBehaviour Grid;
    
    public abstract event Action<int,int> OnDecidePlaceBuilding;

    public virtual void Init(int id, GridBehaviour grid)
    {
        Id = id;
        Grid = grid;
    }

    public abstract void MakeBuildingDecision(GridBehaviour gridBehaviour);
}