using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Animation2D.Story;

namespace Animation2D.UI
{
    /// <summary>
    /// UI hiển thị dialogue box cho Visual Novel style.
    /// Kết nối với StoryController để nhận dialogue.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private Button _continueButton;

        [Header("Typewriter Effect")]
        [SerializeField] private bool _useTypewriter = true;
        [SerializeField] private float _typeSpeed = 0.03f;

        private Coroutine _typewriterCoroutine;
        private string _currentFullText;
        private bool _isTyping = false;

        private void Start()
        {
            // Kết nối với StoryController
            if (StoryController.Instance != null)
            {
                StoryController.Instance.OnDialogueShow.AddListener(ShowDialogue);
                StoryController.Instance.OnStoryCompleted.AddListener(Hide);
            }

            if (_continueButton != null)
            {
                _continueButton.onClick.AddListener(OnContinueClicked);
            }

            Hide();
        }

        /// <summary>
        /// Hiển thị dialogue
        /// </summary>
        public void ShowDialogue(DialogueLine dialogue)
        {
            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(true);

            // Set speaker
            if (_speakerText != null)
            {
                _speakerText.text = dialogue.Speaker;
                _speakerText.gameObject.SetActive(!string.IsNullOrEmpty(dialogue.Speaker));
            }

            // Set portrait
            if (_portraitImage != null)
            {
                if (dialogue.Portrait != null)
                {
                    _portraitImage.sprite = dialogue.Portrait;
                    _portraitImage.gameObject.SetActive(true);
                }
                else
                {
                    _portraitImage.gameObject.SetActive(false);
                }
            }

            // Set dialogue text
            if (_dialogueText != null)
            {
                _currentFullText = dialogue.Text;

                if (_useTypewriter)
                {
                    if (_typewriterCoroutine != null)
                        StopCoroutine(_typewriterCoroutine);
                    _typewriterCoroutine = StartCoroutine(TypewriterEffect(dialogue.Text));
                }
                else
                {
                    _dialogueText.text = dialogue.Text;
                }
            }
        }

        /// <summary>
        /// Ẩn dialogue box
        /// </summary>
        public void Hide()
        {
            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(false);
        }

        /// <summary>
        /// Hiệu ứng đánh chữ từng ký tự
        /// </summary>
        private System.Collections.IEnumerator TypewriterEffect(string text)
        {
            _isTyping = true;
            _dialogueText.text = "";

            foreach (char c in text)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(_typeSpeed);
            }

            _isTyping = false;
        }

        /// <summary>
        /// Xử lý khi click Continue hoặc click vào dialogue
        /// </summary>
        private void OnContinueClicked()
        {
            if (_isTyping)
            {
                // Skip typewriter, hiện full text
                if (_typewriterCoroutine != null)
                    StopCoroutine(_typewriterCoroutine);
                
                _dialogueText.text = _currentFullText;
                _isTyping = false;
            }
            else
            {
                // Chuyển sang dialogue tiếp theo
                if (StoryController.Instance != null)
                {
                    StoryController.Instance.NextDialogue();
                }
            }
        }

        /// <summary>
        /// Gọi từ bên ngoài để skip/continue
        /// </summary>
        public void Continue()
        {
            OnContinueClicked();
        }

        private void OnDestroy()
        {
            if (StoryController.Instance != null)
            {
                StoryController.Instance.OnDialogueShow.RemoveListener(ShowDialogue);
                StoryController.Instance.OnStoryCompleted.RemoveListener(Hide);
            }
        }
    }
}

