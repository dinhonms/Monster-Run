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
        private Vector3 preparedPosistion;
        private int fixedAmount;
        private int countMonster;
        private int min;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            StartMonstersPool();
            preparedPosistion = new Vector3(_startPoint.position.x - _preparedPosXOffset, _startPoint.position.y + GetYOffsetPos(), _startPoint.position.z);
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

        private bool IsAvailable(MonsterBehaviour monster)
        {
            return !monster.gameObject.activeInHierarchy;
        }

        public MonsterPool ResetCountMonsters(int fixedAmount)
        {
            this.fixedAmount = fixedAmount;
            countMonster = 0;
            min = 0;

            return this;
        }

        public void GetOrCreateMonster(int amountMonsters)
        {
            if (countMonster >= fixedAmount)
                return;

            var monster = GetOrCreateMonster();

            RoundDTO.TryAddMosnter(monster);

            countMonster++;

            GetOrCreateMonster(countMonster);
        }

        public MonsterBehaviour GetOrCreateMonster()
        {
            if (min > monsters.Count - 1)
            {
                MonsterBehaviour newMonster = InstantiateMonster();
                newMonster.SetEnableb(true);

                if (monsters.Count <= _maxCapacity)
                    monsters.Add(newMonster, false);

                return newMonster;
            }

            var monst = monsters.ElementAt(min).Key;

            if (IsAvailable(monst))
            {
                Vector3 newPosition = new Vector3(_startPoint.position.x, _startPoint.position.y + GetYOffsetPos(), _startPoint.position.z);
                monst.SetPosition(newPosition);
                monst.SetEnableb(true);

                return monst;
            }

            min++;

            var monster = GetOrCreateMonster();

            return monster;
        }

        private float GetYOffsetPos()
        {
            float yOffset = 0f;
            yOffset = UnityEngine.Random.Range(_minYOffset, _maxYOffset);

            return yOffset;
        }

        public Vector3 GetPreparedPosition()
        {
            return new Vector3(_startPoint.position.x, _startPoint.position.y + GetYOffsetPos(), _startPoint.position.z);
        }
    }
}
