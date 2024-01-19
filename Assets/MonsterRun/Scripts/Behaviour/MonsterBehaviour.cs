using System.Collections;
using System.Threading.Tasks;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviour
{
    public class MonsterBehaviour : MonoBehaviour
    {
        [SerializeField] MonsterSO _monsterSO;
        [SerializeField] Transform thisTransform;
        [SerializeField] SpriteRenderer _spriteRend;
        [SerializeField] ProceduralNameGenerator _proceduralNameGenerator;
        [SerializeField] bool _useAsync;
        [SerializeField] bool _useRigidbody;
        [SerializeField] Rigidbody2D rigidbody2D;

        private bool isRunning;
        private float finishLinePos;
        private bool isReadyToBePooled;
        private float speed;
        private UnityAction<float> onSpeedChanged;
        private UnityAction onDidFinish;
        private string _monsterName;
        private WaitForSeconds waitForSeconds;
        private bool isTheSlowest;
        private bool isInitialized;
        private UnityAction<MonsterBehaviour> onBecomingReady;


        private void Awake()
        {
            _spriteRend.color = GenerateRandomColor();
            _monsterName = _proceduralNameGenerator.GenerateRandomName();

            SetEnableb(false);

            waitForSeconds = new WaitForSeconds(1f);
        }

        private void OnDestroy()
        {
            UnSubscribeAllEvents();
        }

        // private void Update()
        // {
        //     if (isRunning)
        //     {
        //         thisTransform.Translate(speed * Time.deltaTime, 0f, 0f);

        //         if (thisTransform.position.x > finishLinePos)
        //         {
        //             FinishRunning();
        //         }
        //     }
        // }

        private void FixedUpdate()
        {
            if (_useRigidbody && isRunning)
            {
                rigidbody2D.velocity = new Vector2(speed, 0);

                if (thisTransform.position.x > finishLinePos)
                {
                    FinishRunning();
                }
            }
        }

        private void OnSpeedChanged(float newSpeed)
        {
            this.speed = newSpeed;
        }

        private void FinishRunning()
        {
            SetIsRunning(false);
            isInitialized = false;

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

                isReadyToBePooled = true;
                SetEnableb(false);

                onBecomingReady?.Invoke(this);
            }

            async void DisableAsync()
            {
                await Task.Delay(1000);

                if (gameObject == null)
                    return;

                isReadyToBePooled = true;
                SetEnableb(false);

                onBecomingReady?.Invoke(this);
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

        public MonsterBehaviour SetEnableb(bool enabled)
        {
            gameObject.SetActive(enabled);

            return this;
        }

        public MonsterBehaviour Initialize(float finishLinePos, bool isReadyToBePooled = false)
        {
            this.finishLinePos = finishLinePos;
            this.isReadyToBePooled = isReadyToBePooled;

            isTheSlowest = false;
            isInitialized = true;

            RandomSpeed();
            SetEnableb(true);

            return this;
        }

        private void RandomSpeed()
        {
            speed = _monsterSO.GetSpeed();
        }

        public MonsterBehaviour SetPosition(Vector3 position)
        {
            thisTransform.position = position;

            return this;
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

        private void UnSubscribeAllEvents()
        {
            this.onSpeedChanged = null;
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

        #region GET

        public bool GetIsRunning() => isRunning;

        public string GetName() => this._monsterName;
        public float GetSpeed() => speed;

        public SpriteRenderer GetSpriteRend() => _spriteRend;

        public bool GetIsReadyToBePooled() => isReadyToBePooled;

        public bool GetIsInitialized() => isInitialized;

        #endregion

    }
}
