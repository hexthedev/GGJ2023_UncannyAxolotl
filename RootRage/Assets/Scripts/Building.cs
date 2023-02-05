using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    public AgentBehaviour Agent;
    public int Team;

    BuildingConfig _buildingConfig;
    public BuildingConfig buildingConfig
    {
        get => _buildingConfig;
        set
        {
            _buildingConfig = value;
            currentHP = value.HP;
        }
    }

    public MeshRenderer mr;

    public float currentHP;
    TimerBehaviour t;
    public TMP_Text text;

    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
        text.text = $"{currentHP}";
        // if(t != null)
            // text.text = $"{t.CurrentTime.ToString("F1")}/{t.Interval.ToString("F1")}";

    }

    public void SetMaterial(Material m)
    {
        mr.material = m;
    }
    
    public void StartSpawnUnit(Transform enemyBase)
    {
        if(t != null)
            Destroy(t);

        t = gameObject.AddComponent<TimerBehaviour>();
        t.Interval = buildingConfig.SpawnInterval;
        t.Do = Spawn;

        void Spawn()
        {
            AgentBehaviour mt = Instantiate(buildingConfig.unitConfig.unitPrefab, transform);
            mt.UnitConfig = buildingConfig.unitConfig;
            mt.Team = Team;
            mt.EnemyBase = enemyBase;
        }
    }
}
