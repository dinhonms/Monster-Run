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
        [SerializeField] Button _playButton;
        [SerializeField] Button _pauseButton;
        [SerializeField] Canvas _gameOverCanvas;

        private void Start()
        {
            _gamePlayController.SubscribeOnStartNextRound(OnStartNextRound);
        }

        private void OnStartNextRound(float roundTimeInterval)
        {
            _gameOverCanvas.enabled = true;

            StartCoroutine(StartCountNextRoundTime(roundTimeInterval));
        }

        private IEnumerator StartCountNextRoundTime(float roundTimeInterval)
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

            SetRound(_gamePlayController.GetCurrentRound());
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
