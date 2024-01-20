using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private Dictionary<MonsterBehaviour, bool> roundMonsters;
        private List<MonsterBehaviour> currentRoundMonsters;
        private int amountMonsters;
        private UnityAction<float> onSpeedChanged;
        private UnityAction<float, int> onRoundStarted;
        private WaitForSeconds waitForSeconds;
        [SerializeField] bool _useAsync;
        private Action<float> onRoundEnded;

        private void Awake()
        {
            Instance = this;
            waitForSeconds = new WaitForSeconds(_nextRoundInterval);
        }

        private void OnDestroy()
        {
            onSpeedChanged = null;
            onRoundStarted = null;
            onRoundEnded = null;
        }

        public void InitializeRound()
        {
            currentRound++;

            if (currentRound == 1)
            {
                SetFirstRound();
            }

            else
            {
                SetUpMonstersSecondRoundAndOnwards();
            }

            currentRoundMonsters = new List<MonsterBehaviour>();
            currentRoundMonsters.AddRange(roundMonsters.Keys.ToList());

            roundMonsters.Clear();
            amountMonsters = 0;

            //prepares for the second round and onwards
            PrepareNextRound();
        }

        private void SetFirstRound()
        {
            roundMonsters = new Dictionary<MonsterBehaviour, bool>();
            amountMonsters = FibonacciRound.GetMonstersByRound(currentRound);
            onRoundStarted?.Invoke(_nextRoundInterval, currentRound);

            SetUpMonsters();
        }

        private void PrepareNextRound()
        {
            int nextRound = currentRound + 1;
            roundMonsters = new Dictionary<MonsterBehaviour, bool>();

            amountMonsters = FibonacciRound.GetMonstersByRound(nextRound);
        }

        private void SetUpMonstersSecondRoundAndOnwards()
        {
            onRoundStarted?.Invoke(_nextRoundInterval, amountMonsters);

            MonsterFactory.Instance.GetOrCreateMonster(amountMonsters);

            roundMonsters = RoundDTO.RoundMonsters;

            if (roundMonsters == null || !roundMonsters.Any())
                return;

            MonsterBehaviour slowestMonster = roundMonsters.FirstOrDefault().Key;
            var cSpeed = roundMonsters.ElementAt(0).Key.GetSpeed();

            for (int a = 0; a < roundMonsters.Count; a++)
            {
                var cMonst = roundMonsters.ElementAt(a).Key;

                cMonst.Initialize(_finishLine.position.x)
                    .SetIsRunning(true);

                if (cMonst.GetSpeed() < cSpeed)
                {
                    cSpeed = cMonst.GetSpeed();
                    slowestMonster = cMonst;
                }
            }

            slowestMonster.SubscribeOnDidFinish(OnMonsterDidFinish);
            slowestMonster.SetAsSlowest();

            slowestMonster = null;
        }

        private void SetUpMonsters()
        {
            MonsterFactory.Instance.GetOrCreateMonster(amountMonsters);

            roundMonsters = RoundDTO.RoundMonsters;

            MonsterBehaviour slowestMonster = roundMonsters.FirstOrDefault().Key;
            var cSpeed = roundMonsters.ElementAt(0).Key.GetSpeed();

            for (int a = 0; a < roundMonsters.Count; a++)
            {
                var cMonst = roundMonsters.ElementAt(a).Key;

                cMonst.Initialize(_finishLine.position.x)
                    .SetIsRunning(true);

                if (cMonst.GetSpeed() < cSpeed)
                {
                    cSpeed = cMonst.GetSpeed();
                    slowestMonster = cMonst;
                }
            }

            slowestMonster.SubscribeOnDidFinish(OnMonsterDidFinish);
            slowestMonster.SetAsSlowest();

            slowestMonster = null;
        }

        private void OnMonsterDidFinish()
        {
            if (_useAsync)
            {
                HandleNextRoundAsync();

                return;
            }

            StartCoroutine(HandleNextRound());
        }

        [Obsolete]
        private bool IsTheresAMonsterStillRunning()
        {
            foreach (var monster in roundMonsters.Keys)
            {
                if (monster.GetIsRunning())
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator HandleNextRound()
        {
            onRoundEnded?.Invoke(_nextRoundInterval);

            yield return waitForSeconds;

            InitializeRound();
        }


        async void HandleNextRoundAsync()
        {
            onRoundEnded?.Invoke(_nextRoundInterval);

            await Task.Delay((int)_nextRoundInterval * 1000);

            InitializeRound();
        }

        internal void PauseGame()
        {
            foreach (var monster in currentRoundMonsters)
            {
                if (!monster.IsEnabled())
                    return;

                monster.SetIsRunning(false);
            }
        }

        internal void ResumeGame()
        {
            foreach (var monster in currentRoundMonsters)
            {
                if (!monster.IsEnabled())
                    return;

                monster.SetIsRunning(true);
            }
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
            this.onRoundStarted += onNextRound;
        }

        internal void SubscribeOnRoundEnded(Action<float> onRoundEnded)
        {
            this.onRoundEnded += onRoundEnded;
        }
    }
}
