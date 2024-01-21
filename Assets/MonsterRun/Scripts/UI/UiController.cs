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
        [SerializeField] TMPro.TextMeshProUGUI _pauseText;
        [SerializeField] TMPro.TextMeshProUGUI _displayRound;
        [SerializeField] TMPro.TextMeshProUGUI _displaySpeedRate;
        [SerializeField] TMPro.TextMeshProUGUI _displayRoundTimeInterval;
        [SerializeField] TMPro.TextMeshProUGUI _displayTotalMonstersCreated;
        [SerializeField] TMPro.TextMeshProUGUI _displayElapsedTime;
        [SerializeField] GameObject _playButtonOb;
        [SerializeField] GameObject _pauseButtonObj;
        [SerializeField] GameObject _gameOverCanvas;
        StringBuilder timer = new StringBuilder();

        [SerializeField] string _pauseStr = "Pause";
        [SerializeField] string _playStr = "Play";

        private void Start()
        {
            _gamePlayController.SubscribeOnRoundStarted(OnStartRound);
            _gamePlayController.SubscribeOnRoundEnded(OnRoundEnded);

            _timerComponent.OnTimeChanged += UpdateTime;
            InitializeUIState();
        }

        private void UpdateTime(TimeSpan time)
        {
            _displayElapsedTime.text = new StringBuilder().AppendFormat("{0:D2}:{1:D2}:{2:D1}", time.Hours, time.Minutes, time.Seconds).ToString();
        }

        private void InitializeUIState()
        {
            _pauseButtonObj.SetActive(false);
            _playButtonOb.SetActive(true);
            _gameOverCanvas.SetActive(false);
        }

        private void OnStartRound(float roundTimeInterval, int amountMonsters)
        {
            SetAmountMonsters(amountMonsters);
            SetRound(_gamePlayController.GetCurrentRound());
            ClearElapsedTime();
            _displayElapsedTime.text = "00:00:00";
            _timerComponent.StartTimer();
        }

        private void OnRoundEnded(float roundTimeInterval)
        {
            _gameOverCanvas.SetActive(true);
            _pauseText.text = _pauseStr;
            _timerComponent.StopTimer();
            _displayElapsedTime.text = "00:00:00";

            StartCoroutine(HideGameOverCanvas(roundTimeInterval));
        }

        private void ClearElapsedTime()
        {
            _timerComponent.ClearElapsedTime();
        }

        private IEnumerator HideGameOverCanvas(float roundTimeInterval)
        {
            yield return new WaitForSeconds(roundTimeInterval);

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
            _timerComponent.StartTimer();
        }

        private void SetAmountMonsters(int amountMonsters)
        {
            _displayTotalMonstersCreated.text = amountMonsters.ToString();
        }

        public void PauseOrResume()
        {
            _pauseText.text = GameState.GetCurrentGameState() == GameStates.PAUSE ? _pauseStr : _playStr;
            _gamePlayController.PauseOrResume();
            _timerComponent.ToggleTimer();
        }

        #endregion
    }
}
