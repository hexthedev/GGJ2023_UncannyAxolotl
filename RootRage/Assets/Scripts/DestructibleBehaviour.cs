using System.Collections;
using DefaultNamespace;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehaviour : MonoBehaviour
{
    TimerBehaviour t;

    void Start()
    {
        t = gameObject.AddComponent<TimerBehaviour>();
        t.Interval = 10f;
        t.Do = Fade;

        void Fade()
        {
            Destroy(gameObject);
            // Do beautiful fade
        }
    }
}
