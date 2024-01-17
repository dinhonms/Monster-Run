using System.Collections.Generic;
using Architecture;
using Behaviour;
using UnityEngine;

namespace Controller
{
    public class RoundController : MonoBehaviour
    {
        [SerializeField] Transform _finishLine;

        private int currentRound = 0;
        private int currentEnemiesCount;

        private List<MonsterBehaviour> roundMonsters;

        public void InitializeGame()
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
                monster.Initialize(_finishLine.position.x).KeepRunning(true);
            }
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
            currentEnemiesCount = CalcFibonacci(currentRound);

            return currentEnemiesCount;
        }

        private int CalcFibonacci(int number)
        {
            if (number < 3)
                return 1;

            return CalcFibonacci(number - 1) + CalcFibonacci(number - 2);
        }
    }
}
