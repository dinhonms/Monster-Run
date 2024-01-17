using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Behaviour
{
    public class MonsterBehaviour : MonoBehaviour
    {
        [SerializeField] MonsterSO _monsterSO;
        [SerializeField] float _speed;
        [SerializeField] SpriteRenderer _spriteRend;

        private Transform thisTransform;
        private bool keepRunning;
        private float finishLinePos;

        private void Start()
        {
            thisTransform = transform;
            _spriteRend.color = GenerateRandomColor();

            SetEnableb(false);
        }

        private void Update()
        {
            if (keepRunning)
            {
                thisTransform.Translate(_speed * Time.deltaTime, 0f, 0f);

                if (thisTransform.position.x > finishLinePos)
                {
                    FinishRunning();
                }
            }
        }

        private void FinishRunning()
        {
            StartCoroutine(FinishAfterTime());

            IEnumerator FinishAfterTime()
            {
                yield return new WaitForSeconds(1f);

                SetEnableb(false);
            }
        }

        private Color GenerateRandomColor()
        {
            var color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            return color;
        }

        public void SetEnableb(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        public MonsterBehaviour Initialize(float finishLinePos)
        {
            this.finishLinePos = finishLinePos;

            RandomSpeed();
            SetEnableb(true);

            return this;
        }

        private void RandomSpeed()
        {
            _speed = _monsterSO.GetSpeed();
        }

        public void SetPosition(Vector3 position)
        {
            thisTransform.position = position;
        }

        public void KeepRunning(bool keepRunning)
        {
            this.keepRunning = keepRunning;
        }
    }
}
