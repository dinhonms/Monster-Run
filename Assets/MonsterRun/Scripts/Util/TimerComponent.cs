using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class TimerComponent : MonoBehaviour
    {
        private float timer;
        private bool startTimer;

        public void ToggleTimer()
        {
            startTimer = !startTimer;
        }

        public TimeSpan GetElapsedTime()
        {
            var timerFormat = TimeSpan.FromSeconds(timer);

            return timerFormat;
        }

        void Update()
        {
            if (startTimer)
            {
                timer += Time.deltaTime;
            }
        }

        public void ClearElapsedTime()
        {
            timer = 0f;
        }
    }
}
