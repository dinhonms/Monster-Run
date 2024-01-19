using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MonsterSO", menuName = "SO/Monster SO", order = 1)]
    public class MonsterSO : ScriptableObject
    {        
        [SerializeField] float _minSpeed = 1f;
        [SerializeField] float _maxSpeed = 8f;
        
        private float speed;

        public float GetSpeed()
        {
            return speed = Random.Range(_minSpeed, _maxSpeed);
        }
    }

}
