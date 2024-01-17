using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] RoundController _roundController;

        public void Play()
        {
            _roundController.InitializeGame();

            GameState.SetCurrentGameState(GameStates.PLAY);
        }

        public void Pause()
        {
            _roundController.PauseGame();

            GameState.SetCurrentGameState(GameStates.PAUSE);
        }

        public void Resume()
        {
            _roundController.ResumeGame();
            
            GameState.SetCurrentGameState(GameStates.PAUSE);
        }
    }
}
