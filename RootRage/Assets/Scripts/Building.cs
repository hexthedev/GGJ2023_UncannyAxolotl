using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    public MoveTo Agent;
    
    MeshRenderer mr;

    TimerBehaviour t;
    public TMP_Text text;
    
    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(t != null)
            text.text = $"{t.CurrentTime.ToString("F1")}/{t.Interval.ToString("F1")}";
    }

    public void SetMaterial(Material m)
    {
        mr.material = m;
    }
    
    public void StartSpawnUnit(float interval, Transform goal)
    {
        if(t != null)
            Destroy(t);

        t = gameObject.AddComponent<TimerBehaviour>();
        t.Interval = interval;
        t.Do = Spawn;

        void Spawn()
        {
            MoveTo mt = Instantiate(Agent, transform);
            mt.GetComponent<MeshRenderer>().material = mr.material;
            mt.home = transform;
            mt.goal = goal;
        }
    }
}
