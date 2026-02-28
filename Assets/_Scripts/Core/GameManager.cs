using UnityEngine;
using UnityEngine.Events;

namespace Animation2D.Core
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Cutscene,
        Battle,
        GameOver
    }

    /// <summary>
    /// Quản lý trạng thái game và flow chính.
    /// Sử dụng: GameManager.Instance.StartGame();
    /// </summary>
    public class GameManager : PersistentSingleton<GameManager>
    {
        [Header("Game State")]
        [SerializeField] private GameState _currentState = GameState.MainMenu;
        
        // Events
        public UnityEvent<GameState> OnStateChanged = new UnityEvent<GameState>();
        public UnityEvent OnGamePaused = new UnityEvent();
        public UnityEvent OnGameResumed = new UnityEvent();

        public GameState CurrentState => _currentState;
        public bool IsPaused => _currentState == GameState.Paused;
        public bool IsPlaying => _currentState == GameState.Playing;
        public bool IsInCutscene => _currentState == GameState.Cutscene;
        public bool IsInBattle => _currentState == GameState.Battle;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Đổi trạng thái game
        /// </summary>
        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            GameState previousState = _currentState;
            _currentState = newState;

            // Xử lý TimeScale
            switch (newState)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    OnGamePaused?.Invoke();
                    break;
                case GameState.Playing:
                case GameState.Cutscene:
                case GameState.Battle:
                    Time.timeScale = 1f;
                    if (previousState == GameState.Paused)
                        OnGameResumed?.Invoke();
                    break;
            }

            OnStateChanged?.Invoke(newState);
            Debug.Log($"[GameManager] State: {previousState} -> {newState}");
        }

        public void StartGame() => SetState(GameState.Playing);
        public void PauseGame() => SetState(GameState.Paused);
        public void ResumeGame() => SetState(GameState.Playing);
        public void StartCutscene() => SetState(GameState.Cutscene);
        public void StartBattle() => SetState(GameState.Battle);
        public void EndCutscene() => SetState(GameState.Playing);
        public void GameOver() => SetState(GameState.GameOver);
        public void ReturnToMainMenu() => SetState(GameState.MainMenu);

        public void TogglePause()
        {
            if (_currentState == GameState.Paused)
                ResumeGame();
            else if (_currentState == GameState.Playing || _currentState == GameState.Battle)
                PauseGame();
        }

        public void QuitGame()
        {
            Debug.Log("[GameManager] Quit Game");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

