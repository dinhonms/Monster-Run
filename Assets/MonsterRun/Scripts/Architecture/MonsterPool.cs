using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour;
using UnityEngine;
using UnityEngine.Events;

namespace Architecture
{
    public class MonsterPool : MonoBehaviour
    {
        public static MonsterPool Instance;

        [SerializeField] MonsterBehaviour _monsterPrefab;
        [SerializeField] Transform _pool;
        [SerializeField] int _startMonstersAmount = 10;
        [SerializeField] int _maxCapacity = 1000;
        [SerializeField] Transform _startPoint;
        [SerializeField] float _minYOffset;
        [SerializeField] float _maxYOffset;
        [SerializeField] float _preparedPosXOffset = -150f;

        private Dictionary<MonsterBehaviour, bool> monsters = new Dictionary<MonsterBehaviour, bool>();
        private int lastSortingOrder;
        private int desiredMonstersForThisRound;
        private int countMonster;
        private int firstIndex;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartMonstersPool();
        }

        private void StartMonstersPool()
        {
            for (int i = 0; i < _startMonstersAmount; i++)
            {
                MonsterBehaviour newMonster = InstantiateMonster();
                newMonster.SetEnableb(false);

                monsters.Add(newMonster, false);
            }
        }

        private MonsterBehaviour InstantiateMonster()
        {
            Vector3 newPosition = new Vector3(_startPoint.position.x, _startPoint.position.y + GetYOffsetPos(), _startPoint.position.z);
            var monster = Instantiate(_monsterPrefab, newPosition, Quaternion.identity, _pool);
            monster.SetGameObjectName();
            monster.AssignSortingOrder(lastSortingOrder);
            lastSortingOrder++;

            return monster;
        }

        private bool IsAvailable(MonsterBehaviour monster) => !monster.gameObject.activeInHierarchy;

        public MonsterPool ResetCountMonsters(int fixedAmount)
        {
            this.desiredMonstersForThisRound = fixedAmount;

            countMonster = 0;
            firstIndex = 0;

            return this;
        }

        public void GetOrCreateMonster(int amountMonsters)
        {
            if (countMonster >= desiredMonstersForThisRound)
                return;

            var monster = GetOrCreateMonster();

            RoundDTO.TryAddMosnter(monster);

            countMonster++;

            GetOrCreateMonster(countMonster);
        }

        public MonsterBehaviour GetOrCreateMonster()
        {
            if (firstIndex > monsters.Count - 1)
            {
                MonsterBehaviour newMonster = InstantiateMonster();
                newMonster.SetEnableb(true);

                if (monsters.Count <= _maxCapacity)
                    monsters.Add(newMonster, false);

                return newMonster;
            }

            var monst = monsters.ElementAt(firstIndex).Key;

            if (IsAvailable(monst))
            {
                Vector3 newPosition = new Vector3(_startPoint.position.x, _startPoint.position.y + GetYOffsetPos(), _startPoint.position.z);
                monst.SetPosition(newPosition);
                monst.SetEnableb(true);

                return monst;
            }

            firstIndex++;

            var monster = GetOrCreateMonster();

            return monster;
        }

        private float GetYOffsetPos()
        {
            float yOffset = 0f;
            yOffset = UnityEngine.Random.Range(_minYOffset, _maxYOffset);

            return yOffset;
        }
        
    }
}
