using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int amountMonsters;
        private UnityAction<float> onSpeedChanged;
        private UnityAction<float, int> onStarRound;

        private Dictionary<string, bool> checkFinishDict = new Dictionary<string, bool>();
        private WaitForSeconds waitForSeconds;

        private void Awake()
        {
            Instance = this;
            waitForSeconds = new WaitForSeconds(_nextRoundInterval);
        }

        private void OnDestroy()
        {
            onSpeedChanged = null;
            onStarRound = null;
        }

        public void InitializeRound()
        {
            if (checkFinishDict.Any())
                checkFinishDict.Clear();

            currentRound++;

            roundMonsters = new List<MonsterBehaviour>();

            amountMonsters = GetMonstersByRound();

            SetUpMonsters();

            onStarRound?.Invoke(_nextRoundInterval, amountMonsters);
        }

        private void SetUpMonsters()
        {
            for (int i = 0; i < amountMonsters; i++)
            {
                var monster = MonsterFactory.Instance.GetOrCreateMonster();
                roundMonsters.Add(monster);
            }

            MonsterBehaviour slowestMonster = roundMonsters.FirstOrDefault();
            var cSpeed = roundMonsters[0].GetSpeed();

            for (int a = 0; a < roundMonsters.Count; a++)
            {
                roundMonsters[a].Initialize(_finishLine.position.x)
                    .SetIsRunning(true)
                    .SubscribeSpeedChanged(onSpeedChanged);

                if (roundMonsters[a].GetSpeed() < cSpeed)
                {
                    cSpeed = roundMonsters[a].GetSpeed();
                    slowestMonster = roundMonsters[a];
                }
            }

            slowestMonster.SubscribeOnDidFinish(OnMonsterDidFinish);
            slowestMonster.SetAsSlowest();
        }

        private void OnMonsterDidFinish()
        {
            StartCoroutine(HandleNextRound());
        }

        [Obsolete]
        private bool IsTheresAMonsterStillRunning()
        {
            foreach (var monster in roundMonsters)
            {
                if (monster.GetIsRunning())
                {
                    // Debug.Log(new StringBuilder($"Monster {monster.GetName()} is istill Running!"));
                    return true;
                }
            }

            return false;
        }

        private IEnumerator HandleNextRound()
        {
            yield return waitForSeconds;

            InitializeRound();
        }

        internal void PauseGame()
        {
            foreach (var monster in roundMonsters)
            {
                monster.SetIsRunning(false);
            }
        }

        internal void ResumeGame()
        {
            foreach (var monster in roundMonsters)
            {
                monster.SetIsRunning(true);
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

        internal void SubscribeOnStartNextRound(UnityAction<float, int> onNextRound)
        {
            this.onStarRound += onNextRound;
        }
    }
}
