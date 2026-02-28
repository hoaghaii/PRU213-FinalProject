using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Animation2D.UI
{
    /// <summary>
    /// UI hiển thị thời gian đếm ngược (cho deadline effect).
    /// </summary>
    public class TimerUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _timerFill;

        [Header("Timer Settings")]
        [SerializeField] private float _totalTime = 180f; // 3 phút
        [SerializeField] private float _currentTime;
        [SerializeField] private bool _isCountingDown = false;

        [Header("Display Format")]
        [SerializeField] private string _timeFormat = "HH:mm"; // 23:59 style
        [SerializeField] private int _startHour = 22;
        [SerializeField] private int _startMinute = 48;

        [Header("Warning")]
        [SerializeField] private float _warningThreshold = 30f;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _warningColor = Color.red;

        public float CurrentTime => _currentTime;
        public float TotalTime => _totalTime;
        public bool IsRunning => _isCountingDown;

        // Events
        public UnityEngine.Events.UnityEvent OnTimerComplete = new UnityEngine.Events.UnityEvent();
        public UnityEngine.Events.UnityEvent OnWarningStart = new UnityEngine.Events.UnityEvent();

        private bool _warningTriggered = false;

        private void Start()
        {
            _currentTime = _totalTime;
            UpdateUI();
        }

        private void Update()
        {
            if (_isCountingDown && _currentTime > 0)
            {
                _currentTime -= Time.deltaTime;

                // Check warning
                if (!_warningTriggered && _currentTime <= _warningThreshold)
                {
                    _warningTriggered = true;
                    OnWarningStart?.Invoke();
                    if (_timerText != null)
                        _timerText.color = _warningColor;
                }

                // Check complete
                if (_currentTime <= 0)
                {
                    _currentTime = 0;
                    _isCountingDown = false;
                    OnTimerComplete?.Invoke();
                }

                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (_timerText != null)
            {
                // Tính toán thời gian hiển thị theo format giờ đồng hồ
                float elapsedTime = _totalTime - _currentTime;
                int totalMinutes = _startHour * 60 + _startMinute + Mathf.FloorToInt(elapsedTime / 60f);
                int hours = (totalMinutes / 60) % 24;
                int minutes = totalMinutes % 60;

                _timerText.text = $"{hours:D2}:{minutes:D2}";
            }

            if (_timerFill != null)
            {
                _timerFill.fillAmount = _currentTime / _totalTime;
            }
        }

        /// <summary>
        /// Bắt đầu đếm ngược
        /// </summary>
        public void StartTimer()
        {
            _isCountingDown = true;
        }

        /// <summary>
        /// Dừng đếm ngược
        /// </summary>
        public void StopTimer()
        {
            _isCountingDown = false;
        }

        /// <summary>
        /// Reset timer
        /// </summary>
        public void ResetTimer()
        {
            _currentTime = _totalTime;
            _warningTriggered = false;
            if (_timerText != null)
                _timerText.color = _normalColor;
            UpdateUI();
        }

        /// <summary>
        /// Set thời gian còn lại (seconds)
        /// </summary>
        public void SetTime(float seconds)
        {
            _currentTime = Mathf.Max(0, seconds);
            UpdateUI();
        }

        /// <summary>
        /// Hiển thị thời gian cụ thể (cho scripted moments)
        /// </summary>
        public void DisplayTime(int hour, int minute)
        {
            if (_timerText != null)
            {
                _timerText.text = $"{hour:D2}:{minute:D2}";
            }
        }
    }
}

