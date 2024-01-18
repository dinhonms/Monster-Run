using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviour
{
    public class MonsterBehaviour : MonoBehaviour
    {
        [SerializeField] MonsterSO _monsterSO;
        [SerializeField] SpriteRenderer _spriteRend;

        private Transform thisTransform;
        private bool keepRunning;
        private float finishLinePos;
        private float speed;
        private UnityAction<float> onSpeedChanged;
        private UnityAction onDidFinish;
        private bool hasFinishedRunning;

        private void Awake()
        {
            thisTransform = transform;
            _spriteRend.color = GenerateRandomColor();

            SetEnableb(false);
        }

        private void OnSpeedChanged(float newSpeed)
        {
            this.speed = newSpeed;
        }

        private void OnDestroy()
        {
            UnSubscribeSpeedChanged();
            onDidFinish = null;
        }

        private void Update()
        {
            if (keepRunning)
            {
                thisTransform.Translate(speed * Time.deltaTime, 0f, 0f);

                if (thisTransform.position.x > finishLinePos)
                {
                    FinishRunning();
                }
            }
        }

        private void FinishRunning()
        {
            hasFinishedRunning = true;

            StartCoroutine(FinishAfterTime());

            IEnumerator FinishAfterTime()
            {
                yield return new WaitForSeconds(1f);

                SetEnableb(false);
            }

            onDidFinish?.Invoke();
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
            hasFinishedRunning = false;
            this.finishLinePos = finishLinePos;

            RandomSpeed();
            SetEnableb(true);

            return this;
        }

        private void RandomSpeed()
        {
            speed = _monsterSO.GetSpeed();
        }

        public void SetPosition(Vector3 position)
        {
            thisTransform.position = position;
        }

        public MonsterBehaviour KeepRunning(bool keepRunning)
        {
            this.keepRunning = keepRunning;

            return this;
        }

        public MonsterBehaviour SubscribeSpeedChanged(UnityAction<float> onSpeedChanged)
        {
            this.onSpeedChanged += onSpeedChanged;

            this.onSpeedChanged += OnSpeedChanged;

            return this;
        }

        private void UnSubscribeSpeedChanged()
        {
            this.onSpeedChanged = null;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SubscribeOnDidFinish(UnityAction onDidFinish)
        {
            this.onDidFinish += onDidFinish;
        }

        public bool HasFinished()
        {
            return hasFinishedRunning;
        }
    }
}
