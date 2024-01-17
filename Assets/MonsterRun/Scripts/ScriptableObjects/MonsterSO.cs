using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MonsterSO", menuName = "SO/Monster SO", order = 1)]
    public class MonsterSO : ScriptableObject
    {
        [SerializeField] float _minSpeed;
        [SerializeField] float _maxSpeed;
        
        private float speed;

        public float GetSpeed()
        {
            return speed = Random.Range(_minSpeed, _maxSpeed);
        }
    }

}
