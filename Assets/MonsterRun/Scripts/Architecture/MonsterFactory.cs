using System;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;

namespace Architecture
{
    public class MonsterFactory : MonoBehaviour
    {
        public static MonsterFactory Instance;

        private void Awake()
        {
            Instance = this;
        }

        public MonsterBehaviour GetOrCreateMonster()
        {
            return MonsterPool.Instance.GetOrCreateMonster();
        }
    }
}
