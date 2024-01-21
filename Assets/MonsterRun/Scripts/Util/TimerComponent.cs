using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TimerComponent : MonoBehaviour
    {
        //performance technique
        [SerializeField] bool _useTimeSlicing;
        [SerializeField] int _frameInterval = 3;

        private float timer;
        private bool startTimer;

        public UnityAction<TimeSpan> OnTimeChanged;

        private float currentTime;

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

                currentTime = timer;

                if (currentTime >= 1f)
                {
                    OnTimeChanged?.Invoke(GetElapsedTime());
                    currentTime = 0;
                }
            }
        }

        public void ClearElapsedTime()
        {
            timer = 0f;
            currentTime = 0;
        }

        public void StopTimer()
        {
            startTimer = false;
        }

        public void StartTimer()
        {
            startTimer = true;
        }
    }
}
