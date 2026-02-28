using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Animation2D.UI
{
    /// <summary>
    /// UI hiển thị chat messages theo style Visual Novel.
    /// Dùng cho các đoạn chat của nhóm sinh viên.
    /// </summary>
    public class ChatUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _chatPanel;
        [SerializeField] private Transform _messageContainer;
        [SerializeField] private GameObject _messagePrefab;

        [Header("Settings")]
        [SerializeField] private int _maxMessages = 10;
        [SerializeField] private float _messageSpacing = 10f;

        [Header("Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _errorColor = Color.red;
        [SerializeField] private Color _successColor = Color.green;

        private List<GameObject> _messageObjects = new List<GameObject>();

        /// <summary>
        /// Thêm message mới
        /// </summary>
        public void AddMessage(string message, MessageType type = MessageType.Normal)
        {
            if (_messagePrefab == null || _messageContainer == null) return;

            // Tạo message object
            var msgObj = Instantiate(_messagePrefab, _messageContainer);
            var tmpText = msgObj.GetComponentInChildren<TextMeshProUGUI>();

            if (tmpText != null)
            {
                tmpText.text = message;
                tmpText.color = GetColorForType(type);
            }

            _messageObjects.Add(msgObj);

            // Giới hạn số message
            while (_messageObjects.Count > _maxMessages)
            {
                var oldest = _messageObjects[0];
                _messageObjects.RemoveAt(0);
                Destroy(oldest);
            }
        }

        /// <summary>
        /// Thêm chat message với format "Speaker: Message"
        /// </summary>
        public void AddChatMessage(string speaker, string message)
        {
            string formatted = string.IsNullOrEmpty(speaker) 
                ? $"- \"{message}\"" 
                : $"- {speaker}: \"{message}\"";
            AddMessage(formatted, MessageType.Normal);
        }

        /// <summary>
        /// Thêm system/terminal message
        /// </summary>
        public void AddSystemMessage(string message)
        {
            AddMessage($"> {message}", MessageType.Warning);
        }

        /// <summary>
        /// Thêm error message (như Git conflict)
        /// </summary>
        public void AddErrorMessage(string message)
        {
            AddMessage(message, MessageType.Error);
        }

        /// <summary>
        /// Thêm success message
        /// </summary>
        public void AddSuccessMessage(string message)
        {
            AddMessage(message, MessageType.Success);
        }

        /// <summary>
        /// Xóa tất cả messages
        /// </summary>
        public void ClearMessages()
        {
            foreach (var obj in _messageObjects)
            {
                Destroy(obj);
            }
            _messageObjects.Clear();
        }

        /// <summary>
        /// Hiện chat panel
        /// </summary>
        public void Show()
        {
            if (_chatPanel != null)
                _chatPanel.SetActive(true);
        }

        /// <summary>
        /// Ẩn chat panel
        /// </summary>
        public void Hide()
        {
            if (_chatPanel != null)
                _chatPanel.SetActive(false);
        }

        private Color GetColorForType(MessageType type)
        {
            return type switch
            {
                MessageType.Warning => _warningColor,
                MessageType.Error => _errorColor,
                MessageType.Success => _successColor,
                _ => _normalColor
            };
        }
    }

    public enum MessageType
    {
        Normal,
        Warning,
        Error,
        Success
    }
}

