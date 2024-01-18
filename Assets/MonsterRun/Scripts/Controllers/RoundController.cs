using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Architecture;
using Behaviour;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public class RoundController : MonoBehaviour
    {
        public static RoundController Instance;

        [SerializeField] Transform _finishLine;

        [Header("NEXT ROUND TIME INTERVAL")]
        [SerializeField] float _nextRoundInterval = 5f;

        private int currentRound = 0;
        private int currentMonstersCount;

        private List<MonsterBehaviour> roundMonsters;
        private UnityAction<float> onSpeedChanged;
        private UnityAction<float> onStartNextRound;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            onSpeedChanged = null;
            onStartNextRound = null;
        }

        public void InitializeRound()
        {
            currentRound++;

            roundMonsters = new List<MonsterBehaviour>();

            var amountMonsters = GetMonstersByRound();

            for (int i = 0; i < amountMonsters; i++)
            {
                var monster = MonsterFactory.Instance.GetOrCreateMonster();
                roundMonsters.Add(monster);
            }

            foreach (var monster in roundMonsters)
            {
                monster.Initialize(_finishLine.position.x)
                    .KeepRunning(true)
                    .SubscribeSpeedChanged(onSpeedChanged)
                    .SubscribeOnDidFinish(OnMonsterDidFinish);
            }
        }

        private void OnMonsterDidFinish()
        {
            if (IsSomeMonsterStillRunning())
                return;

            StartCoroutine(HandleNextRounds());
        }

        private bool IsSomeMonsterStillRunning()
        {
            foreach (var monster in roundMonsters)
            {
                if (!monster.HasFinished())
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator HandleNextRounds()
        {
            onStartNextRound?.Invoke(_nextRoundInterval);

            yield return new WaitForSeconds(_nextRoundInterval);

            InitializeRound();
        }

        internal void PauseGame()
        {
            foreach (var monster in roundMonsters)
            {
                monster.KeepRunning(false);
            }
        }

        internal void ResumeGame()
        {
            foreach (var monster in roundMonsters)
            {
                monster.KeepRunning(true);
            }
        }

        private int GetMonstersByRound()
        {
            currentMonstersCount = CalcFibonacci(currentRound);

            return currentMonstersCount;
        }

        private int CalcFibonacci(int number)
        {
            if (number < 3)
                return 1;

            return CalcFibonacci(number - 1) + CalcFibonacci(number - 2);
        }

        internal int GetCurrentRound()
        {
            return currentRound;
        }

        internal void SetSpeedChanged(float value)
        {
            onSpeedChanged?.Invoke(value);
        }

        internal void SubscribeOnStartNextRound(UnityAction<float> onNextRound)
        {
            this.onStartNextRound += onNextRound;
        }
    }
}
