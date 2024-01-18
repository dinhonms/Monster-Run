using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using UnityEngine.UI;
using System;

namespace Controller
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] GamePlayController _gamePlayController;

        [Header("UI ELEMENTS")]
        [SerializeField] TMPro.TextMeshProUGUI _displayRound;
        [SerializeField] TMPro.TextMeshProUGUI _displaySpeedRate;
        [SerializeField] TMPro.TextMeshProUGUI _displayRoundTimeInterval;
        [SerializeField] TMPro.TextMeshProUGUI _displayTotalMonstersCreated;
        [SerializeField] Button _playButton;
        [SerializeField] Button _pauseButton;
        [SerializeField] Canvas _gameOverCanvas;

        private void Start()
        {
            _gamePlayController.SubscribeOnStartNextRound(OnStartRound);
        }

        private void OnStartRound(float roundTimeInterval, int amountMonsters)
        {
            SetAmountMonsters(amountMonsters);
            SetRound(_gamePlayController.GetCurrentRound());

            if (_gamePlayController.GetCurrentRound() <= 1)
                return;

            // _gameOverCanvas.enabled = true;
            // StartCoroutine(StartCountNextRoundTime(roundTimeInterval, amountMonsters));
        }

        private IEnumerator StartCountNextRoundTime(float roundTimeInterval, int amountMonsters)
        {
            var time = roundTimeInterval;

            yield return new WaitForSeconds(1f);

            while (time > 0)
            {
                time--;
                _displayRoundTimeInterval.text = time.ToString("f0");

                yield return new WaitForSeconds(1f);
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
        }

        private void SetAmountMonsters(int amountMonsters)
        {
            _displayTotalMonstersCreated.text = amountMonsters.ToString();
        }

        public void PauseOrResume()
        {
            _gamePlayController.PauseOrResume();
        }

        public void ChangeSpeed(float value)
        {
            _displaySpeedRate.text = value.ToString("f2");
            _gamePlayController.SetSpeedChanged(value);
        }

        #endregion
    }
}
