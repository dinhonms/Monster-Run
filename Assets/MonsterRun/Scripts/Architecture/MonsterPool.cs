using System.Collections.Generic;
using Behaviour;
using UnityEngine;

namespace Architecture
{
    public class MonsterPool : MonoBehaviour
    {
        public static MonsterPool Instance;


        [SerializeField] MonsterBehaviour _monsterPrefab;
        [SerializeField] int _startMonstersAmount = 10;
        [SerializeField] Transform _startPoint;

        private List<MonsterBehaviour> monsters = new List<MonsterBehaviour>();

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
                monsters.Add(newMonster);
            }
        }

        private MonsterBehaviour InstantiateMonster()
        {
            var monster = Instantiate(_monsterPrefab, _startPoint);
            monster.SetPosition(_startPoint.position);

            return monster;
        }

        private bool IsAvailable(MonsterBehaviour monster)
        {
            return !monster.gameObject.activeInHierarchy;
        }

        public MonsterBehaviour GetOrCreateMonster()
        {
            foreach (var monst in monsters)
            {
                if (IsAvailable(monst))
                {
                    monst.SetPosition(_startPoint.position);

                    return monst;
                }
            }

            MonsterBehaviour newMonster = InstantiateMonster();
            newMonster.SetPosition(_startPoint.position);
            
            monsters.Add(newMonster);

            return newMonster;
        }
    }
}
