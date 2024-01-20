using System;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;
using UnityEngine.Events;

namespace Architecture
{
    public class MonsterFactory : MonoBehaviour
    {
        public static MonsterFactory Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void GetOrCreateMonster(int amountMonsters)
        {
            MonsterPool.Instance.ResetCountMonsters(amountMonsters).GetOrCreateMonster(amountMonsters);
        }
    }
}
