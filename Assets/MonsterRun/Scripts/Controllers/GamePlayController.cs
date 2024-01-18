using System;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public class GamePlayController : MonoBehaviour
    {
        public void Play()
        {
            RoundController.Instance.InitializeRound();

            GameState.SetCurrentGameState(GameStates.PLAY);
        }

        public void PauseOrResume()
        {
            if (GameState.GetCurrentGameState() == GameStates.PLAY)
            {
                RoundController.Instance.PauseGame();

                GameState.SetCurrentGameState(GameStates.PAUSE);
            }

            else
            {
                RoundController.Instance.ResumeGame();

                GameState.SetCurrentGameState(GameStates.PLAY);
            }
        }

        public void Resume()
        {
            RoundController.Instance.ResumeGame();

            GameState.SetCurrentGameState(GameStates.PAUSE);
        }

        public int GetCurrentRound()
        {
            return RoundController.Instance.GetCurrentRound();
        }

        public void SetSpeedChanged(float value)
        {
            RoundController.Instance.SetSpeedChanged(value);
        }

        public void SubscribeOnStartNextRound(UnityAction<float> onNextRound)
        {
            RoundController.Instance.SubscribeOnStartNextRound(onNextRound);
        }
    }
}
