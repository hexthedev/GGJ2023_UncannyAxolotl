using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimerBehaviour : MonoBehaviour
    {
        public float Interval;
        public float CurrentTime;
        public Action Do;

        void Update()
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= Interval)
            {
                CurrentTime = 0;
                Do();
            }
        }
    }
}