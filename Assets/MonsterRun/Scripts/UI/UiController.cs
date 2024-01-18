using System;
using System.Collections;
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
        [SerializeField] Button _playButton;
        [SerializeField] Button _pauseButton;
        [SerializeField] Canvas _gameOverCanvas;
        private WaitForSeconds waitForSeconds;

        private void Start()
        {
            _gamePlayController.SubscribeOnStartNextRound(OnStartRound);

            waitForSeconds = new WaitForSeconds(1f);
        }

        private void Update()
        {
            if(GameState.GetCurrentGameState() == GameStates.PLAY)
            {
                var timerFormat = _timerComponent.GetElapsedTime();

                _displayElapsedTime.text = string.Format("{0:D2}:{1:D2}:{2:D1}", timerFormat.Hours, timerFormat.Minutes, timerFormat.Seconds);
            }
        }

        private void OnStartRound(float roundTimeInterval, int amountMonsters)
        {
            SetAmountMonsters(amountMonsters);
            SetRound(_gamePlayController.GetCurrentRound());
            ClearElapsedTime();

            if (_gamePlayController.GetCurrentRound() <= 1)
                return;

            // _gameOverCanvas.enabled = true;
            // StartCoroutine(StartCountNextRoundTime(roundTimeInterval, amountMonsters));
        }

        private void ClearElapsedTime()
        {
            _timerComponent.ClearElapsedTime();
        }

        private IEnumerator StartCountNextRoundTime(float roundTimeInterval, int amountMonsters)
        {
            var time = roundTimeInterval;

            yield return waitForSeconds;

            while (time > 0)
            {
                time--;
                _displayRoundTimeInterval.text = time.ToString("f0");

                yield return waitForSeconds;
            }

            _gameOverCanvas.enabled = true;
        }

        internal void SetRound(int currentRound)
        {
            _displayRound.text = currentRound.ToString();
        }

        #region UI INPUT

        public void Play()
        {
            _gamePlayController.Play();
            _playButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
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
