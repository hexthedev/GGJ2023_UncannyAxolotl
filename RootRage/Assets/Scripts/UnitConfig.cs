using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Axolotyl/Unit")]
public class UnitConfig : ScriptableObject
{
    public float Range;
    public float Cost;
    public float HP;
    public float Damage;
    public float MoveSpeed;
    public float attackInterval;
    public bool FocusBuildings;
    public bool CanAttackFlying;
    public bool Flying;
    public AgentBehaviour unitPrefab;
}
