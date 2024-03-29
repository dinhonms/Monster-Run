using System;
using System.Collections;
using System.Threading.Tasks;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Behaviour
{
    public class MonsterBehaviour : MonoBehaviour
    {
        [SerializeField] MonsterSO _monsterSO;
        [SerializeField] Transform thisTransform;
        [SerializeField] SpriteRenderer _spriteRend;
        [SerializeField] bool _useAsync;
        [SerializeField] Rigidbody2D _rigidbody2D;

        private bool isRunning;
        private float finishLinePos;
        private float speed;
        private UnityAction onDidFinish;
        private string _monsterName;
        private WaitForSeconds waitForSeconds;
        private bool isTheSlowest;
        private UnityAction<MonsterBehaviour> onBecomingReady;
        

        private void Awake()
        {
            _spriteRend.color = Utilities.GenerateRandomColor();
            _monsterName = Utilities.GenerateRandomName();

            SetEnableb(false);

            waitForSeconds = new WaitForSeconds(1f);
        }

        private void OnDestroy()
        {
            UnSubscribeAllEvents();
        }

        private void Update()
        {
            if (isRunning)
            {
                // thisTransform.Translate(speed * Time.deltaTime, 0f, 0f);

                if (thisTransform.position.x > finishLinePos)
                {
                    FinishRunning();
                }
            }
        }

        [Obsolete]
        private void OnSpeedChanged(float newSpeed)
        {
            this.speed = newSpeed;
        }

        private void FinishRunning()
        {
            SetIsRunning(false);

            if (isTheSlowest)
            {
                onDidFinish?.Invoke();

                onDidFinish = null;
            }

            if (_useAsync)
            {
                DisableAsync();

                return;
            }

            StartCoroutine(DisableAfterTime());

            IEnumerator DisableAfterTime()
            {
                yield return waitForSeconds;

                SetEnableb(false);

                onBecomingReady?.Invoke(this);
            }

            ///Sometimes async could be better than coroutines
            async void DisableAsync()
            {
                await Task.Delay(1000);

                if (gameObject == null)
                    return;

                SetEnableb(false);

                onBecomingReady?.Invoke(this);
            }
        }

        public MonsterBehaviour Initialize(float finishLinePos)
        {
            this.finishLinePos = finishLinePos;

            isTheSlowest = false;

            RandomSpeed();
            SetEnableb(true);

            _rigidbody2D.simulated = true;
            _rigidbody2D.AddForce(Vector2.right * speed);

            return this;
        }

        private void UnSubscribeAllEvents()
        {
            onDidFinish = null;
            onBecomingReady = null;
        }

        public void SubscribeOnDidFinish(UnityAction onDidFinish)
        {
            this.onDidFinish += onDidFinish;
        }

        public void TrySubscribeOnBecomingReady(UnityAction<MonsterBehaviour> onBecomingReady)
        {
            if (this.onBecomingReady != null)
                return;

            this.onBecomingReady = onBecomingReady;
        }

        #region GETTTERS

        public bool GetIsRunning() => isRunning;

        public string GetName() => this._monsterName;
        public float GetSpeed() => speed;

        public SpriteRenderer GetSpriteRend() => _spriteRend;

        #endregion

        private void RandomSpeed()
        {
            speed = _monsterSO.GetSpeed();
        }

        public bool IsEnabled() => gameObject.activeInHierarchy;

        #region SETTERS

        public MonsterBehaviour SetEnableb(bool enabled)
        {
            gameObject.SetActive(enabled);

            return this;
        }

        public MonsterBehaviour SetPosition(Vector3 position)
        {
            thisTransform.position = position;

            return this;
        }

        public MonsterBehaviour SetIsRunning(bool isRunning)
        {
            this.isRunning = isRunning;
            this._rigidbody2D.simulated = isRunning;

            return this;
        }

        public void SetGameObjectName()
        {
            this.gameObject.name = _monsterName;
        }

        public void SetAsSlowest()
        {
            isTheSlowest = true;
        }

        public void AssignSortingOrder(int lastSortingOrder)
        {
            _spriteRend.sortingOrder = lastSortingOrder;
        }

        #endregion

    }
}
