using System;
using UnityEngine;

public abstract class AGameAgent: ScriptableObject
{
    protected int Id = 0;
    
    public abstract event Action<int,int> OnDecidePlaceBuilding;

    public void Init(int id)
    {
        Id = id;
    }

    public abstract void MakeBuildingDecision(GridBehaviour gridBehaviour);
}