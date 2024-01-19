using System;
using UnityEngine;

namespace Util
{
    public class TimerComponent : MonoBehaviour
    {
        //performance technique
        [SerializeField] bool _useTimeSlicing;
        [SerializeField] int _frameInterval = 3;

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

            // if (_useTimeSlicing)
            // {
            //     if (Time.frameCount % _frameInterval == 0)
            //     {
            //         if (startTimer)
            //         {
            //             timer += Time.deltaTime;
            //         }
            //     }
            // }

        }

        public void ClearElapsedTime()
        {
            timer = 0f;
        }
    }
}
