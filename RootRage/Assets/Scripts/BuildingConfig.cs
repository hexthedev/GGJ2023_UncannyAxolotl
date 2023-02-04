using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UncannyAxolotl/Buildings/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public Mesh mesh;
    public UnitConfig unitConfig;
}
