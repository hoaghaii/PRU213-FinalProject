using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Animation2D.UI
{
    /// <summary>
    /// UI hiển thị terminal/code typing effect.
    /// Dùng cho các đoạn gõ code trong kịch bản.
    /// </summary>
    public class TerminalUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _terminalPanel;
        [SerializeField] private TextMeshProUGUI _terminalText;
        [SerializeField] private Image _cursor;

        [Header("Typewriter Settings")]
        [SerializeField] private float _typeSpeed = 0.05f;
        [SerializeField] private bool _showCursor = true;
        [SerializeField] private float _cursorBlinkSpeed = 0.5f;

        [Header("Style")]
        [SerializeField] private string _promptPrefix = "> ";
        [SerializeField] private Color _textColor = Color.green;
        [SerializeField] private Color _errorColor = Color.red;

        private Coroutine _typeCoroutine;
        private Coroutine _cursorCoroutine;
        private string _fullText = "";

        // Events
        public UnityEngine.Events.UnityEvent OnTypeComplete = new UnityEngine.Events.UnityEvent();

        private void Start()
        {
            if (_terminalText != null)
                _terminalText.color = _textColor;

            if (_showCursor && _cursor != null)
                _cursorCoroutine = StartCoroutine(BlinkCursor());
        }

        /// <summary>
        /// Type một dòng code mới
        /// </summary>
        public void TypeLine(string text, bool isError = false)
        {
            string line = _promptPrefix + text + "\n";
            TypeText(line, isError);
        }

        /// <summary>
        /// Type text với hiệu ứng
        /// </summary>
        public void TypeText(string text, bool isError = false)
        {
            if (_typeCoroutine != null)
                StopCoroutine(_typeCoroutine);

            _typeCoroutine = StartCoroutine(TypeTextCoroutine(text, isError));
        }

        /// <summary>
        /// Type nhiều dòng code
        /// </summary>
        public void TypeCode(string[] lines, float delayBetweenLines = 0.3f)
        {
            StartCoroutine(TypeCodeCoroutine(lines, delayBetweenLines));
        }

        private System.Collections.IEnumerator TypeTextCoroutine(string text, bool isError)
        {
            foreach (char c in text)
            {
                _fullText += c;
                if (_terminalText != null)
                {
                    _terminalText.text = _fullText;
                    if (isError)
                        _terminalText.color = _errorColor;
                }
                yield return new WaitForSeconds(_typeSpeed);
            }

            if (!isError && _terminalText != null)
                _terminalText.color = _textColor;

            OnTypeComplete?.Invoke();
        }

        private System.Collections.IEnumerator TypeCodeCoroutine(string[] lines, float delay)
        {
            foreach (string line in lines)
            {
                yield return TypeTextCoroutine(_promptPrefix + line + "\n", false);
                yield return new WaitForSeconds(delay);
            }
        }

        private System.Collections.IEnumerator BlinkCursor()
        {
            while (true)
            {
                if (_cursor != null)
                {
                    _cursor.enabled = !_cursor.enabled;
                }
                yield return new WaitForSeconds(_cursorBlinkSpeed);
            }
        }

        /// <summary>
        /// Clear terminal
        /// </summary>
        public void Clear()
        {
            _fullText = "";
            if (_terminalText != null)
                _terminalText.text = "";
        }

        /// <summary>
        /// Append text ngay lập tức (không có hiệu ứng)
        /// </summary>
        public void AppendImmediate(string text)
        {
            _fullText += text;
            if (_terminalText != null)
                _terminalText.text = _fullText;
        }

        /// <summary>
        /// Hiển thị Git conflict message
        /// </summary>
        public void ShowGitConflict()
        {
            string conflictMsg = 
@"CONFLICT (content): Merge conflict in FinalScene.unity
Automatic merge failed.";
            TypeText(conflictMsg, true);
        }

        /// <summary>
        /// Hiển thị conflict markers
        /// </summary>
        public void ShowConflictMarkers()
        {
            Clear();
            string markers = 
@"<<<<<<< HEAD
CameraZoom = 5;
=======
CameraZoom = 7;
>>>>>>> branch";
            AppendImmediate(markers);
        }

        /// <summary>
        /// Hi���n terminal
        /// </summary>
        public void Show()
        {
            if (_terminalPanel != null)
                _terminalPanel.SetActive(true);
        }

        /// <summary>
        /// Ẩn terminal
        /// </summary>
        public void Hide()
        {
            if (_terminalPanel != null)
                _terminalPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_typeCoroutine != null)
                StopCoroutine(_typeCoroutine);
            if (_cursorCoroutine != null)
                StopCoroutine(_cursorCoroutine);
        }
    }
}

