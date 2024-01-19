using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Controller
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] GamePlayController _gamePlayController;
        [SerializeField] TimerComponent _timerComponent;

        [Header("UI ELEMENTS")]
        [SerializeField] TMPro.TextMeshProUGUI _displayRound;
        [SerializeField] TMPro.TextMeshProUGUI _displaySpeedRate;
        [SerializeField] TMPro.TextMeshProUGUI _displayRoundTimeInterval;
        [SerializeField] TMPro.TextMeshProUGUI _displayTotalMonstersCreated;
        [SerializeField] TMPro.TextMeshProUGUI _displayElapsedTime;
        [SerializeField] GameObject _playButtonOb;
        [SerializeField] GameObject _pauseButtonObj;
        [SerializeField] GameObject _gameOverCanvas;
        private WaitForSeconds waitForSeconds;
        StringBuilder timer = new StringBuilder();

        private void Start()
        {
            _gamePlayController.SubscribeOnRoundStarted(OnStartRound);
            _gamePlayController.SubscribeOnRoundEnded(OnRoundEnded);

            waitForSeconds = new WaitForSeconds(1f);
             
            InitializeUIState();
        }

        private void InitializeUIState()
        {
            _pauseButtonObj.SetActive(false);
            _playButtonOb.SetActive(true);
            _gameOverCanvas.SetActive(false);
        }

        private void Update()
        {
            if (GameState.GetCurrentGameState() == GameStates.PLAY)
            {
                var timerFormat = _timerComponent.GetElapsedTime();

                _displayElapsedTime.text = new StringBuilder().AppendFormat("{0:D2}:{1:D2}:{2:D1}", timerFormat.Hours, timerFormat.Minutes, timerFormat.Seconds).ToString();
            }
        }

        private void OnStartRound(float roundTimeInterval, int amountMonsters)
        {
            SetAmountMonsters(amountMonsters);
            SetRound(_gamePlayController.GetCurrentRound());
            ClearElapsedTime();

            if (_gamePlayController.GetCurrentRound() <= 1)
                return;


        }

        private void OnRoundEnded(float roundTimeInterval)
        {
            _gameOverCanvas.SetActive(true);
            // _displayRoundTimeInterval.text = new StringBuilder($"{roundTimeInterval} seconds").ToString();

            StartCoroutine(HideGameOverCanvas());
        }

        private void ClearElapsedTime()
        {
            _timerComponent.ClearElapsedTime();
        }

        private IEnumerator HideGameOverCanvas()
        {
            // var time = roundTimeInterval;

            yield return waitForSeconds;

            //Countdown
            // while (time > 0)
            // {
            //     time--;
            //     _displayRoundTimeInterval.text = time.ToString("f0");

            //     yield return waitForSeconds;
            // }

            _gameOverCanvas.SetActive(false);
        }

        internal void SetRound(int currentRound)
        {
            _displayRound.text = currentRound.ToString();
        }

        #region UI INPUT

        public void Play()
        {
            _gamePlayController.Play();
            _playButtonOb.SetActive(false);
            _pauseButtonObj.SetActive(true);
            _timerComponent.ToggleTimer();
        }

        private void SetAmountMonsters(int amountMonsters)
        {
            _displayTotalMonstersCreated.text = amountMonsters.ToString();
        }

        public void PauseOrResume()
        {
            _gamePlayController.PauseOrResume();
            _timerComponent.ToggleTimer();
        }

        public void ChangeSpeed(float value)
        {
            _displaySpeedRate.text = value.ToString("f2");
            _gamePlayController.SetSpeedChanged(value);
        }

        #endregion
    }
}
