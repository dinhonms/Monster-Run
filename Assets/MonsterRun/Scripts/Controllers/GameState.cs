using UnityEngine.Events;

namespace Controller
{
    public enum GameStates
    {
        PLAY,
        PAUSE,
        GAME_OVER
    }

    public static class GameState
    {
        private static GameStates currentGameState;

        private static UnityEvent<GameStates> OnStateChanged;

        public static void SubscribeStateChanged(UnityAction<GameStates> onStateChanged)
        {
            OnStateChanged.AddListener(onStateChanged);
        }

        public static void UnsubscribeStateChanged(UnityAction<GameStates> onStateChanged)
        {
            OnStateChanged.RemoveListener(onStateChanged);
        }

        public static void SetCurrentGameState(GameStates newState)
        {
            currentGameState = newState;
        }

        public static GameStates GetCurrentGameState()
        {
            return currentGameState;
        }

        public static bool IsPlayState() => currentGameState == GameStates.PLAY;
        public static bool IsPauseState() => currentGameState == GameStates.PAUSE;
        public static bool IsGameOverState() => currentGameState == GameStates.GAME_OVER;
    }
}
