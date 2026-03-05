using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        messageText = GetComponentInChildren<TMP_Text>();
    }

    public void SetText(string message)
    {
        if (messageText) messageText.text = message;
    }

    /// <summary>Hiện bubble với alpha = 1, không animate.</summary>
    public void Show(string message)
    {
        gameObject.SetActive(true);
        SetText(message);
        if (canvasGroup) canvasGroup.alpha = 1f;
    }

    /// <summary>Ẩn ngay lập tức.</summary>
    public void HideImmediate()
    {
        if (canvasGroup) canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public bool IsVisible => gameObject.activeSelf && (canvasGroup == null || canvasGroup.alpha > 0f);
    public CanvasGroup CanvasGroup => canvasGroup;
}