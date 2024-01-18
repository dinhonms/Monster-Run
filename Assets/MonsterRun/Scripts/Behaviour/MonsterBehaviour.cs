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
        [SerializeField] ProceduralNameGenerator _proceduralNameGenerator;

        private Transform thisTransform;
        private bool isRunning;
        private float finishLinePos;
        private float speed;
        private UnityAction<float> onSpeedChanged;
        private UnityAction onDidFinish;
        private string _monsterName;
        private WaitForSeconds waitForSeconds;
        private bool isTheSlowest;

        private void Awake()
        {
            thisTransform = transform;
            _spriteRend.color = GenerateRandomColor();
            _monsterName = _proceduralNameGenerator.GenerateRandomName();

            SetEnableb(false);

            waitForSeconds = new WaitForSeconds(1f);
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
            if (isRunning)
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
            SetIsRunning(false);
            _spriteRend.enabled = false;

            if (isTheSlowest)
            {
                onDidFinish?.Invoke();

                onDidFinish = null;
            }

            StartCoroutine(DisableAfterTime());

            IEnumerator DisableAfterTime()
            {
                yield return waitForSeconds;

                SetEnableb(false);
            }

            if (!isTheSlowest)
                return;
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
            _spriteRend.enabled = enabled;
        }

        public MonsterBehaviour Initialize(float finishLinePos)
        {
            this.finishLinePos = finishLinePos;
            isTheSlowest = false;

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

        public MonsterBehaviour SetIsRunning(bool isRunning)
        {
            this.isRunning = isRunning;

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

        public bool GetIsRunning()
        {
            return isRunning;
        }

        public void SetGameObjectName()
        {
            this.gameObject.name = _monsterName;
        }

        public string GetName()
        {
            return this._monsterName;
        }

        public void SetAsSlowest()
        {
            isTheSlowest = true;
        }
    }
}
