using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class OneTimerBehaviour : MonoBehaviour
    {
        public float Interval;
        public float CurrentTime;
        public Action Do;

        void Update()
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= Interval)
            {
                Do();
                CurrentTime = 0;
                Destroy(this);
            }
        }
    }
}