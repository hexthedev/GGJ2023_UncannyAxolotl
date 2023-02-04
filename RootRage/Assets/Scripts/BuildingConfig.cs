using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UncannyAxolotl/Buildings/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public float HP;
    public Building buildingPrefab;
    public UnitConfig unitConfig;
}
