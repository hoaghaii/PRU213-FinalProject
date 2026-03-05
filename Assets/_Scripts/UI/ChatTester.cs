using UnityEngine;

/// <summary>
/// Script test tạm: nhấn Space để gửi message vào ChatUI.
/// Gắn vào bất kỳ GameObject nào trong scene.
/// </summary>
public class ChatTester : MonoBehaviour
{
    [SerializeField] private ChatUI chatUI;

    private int _count = 0;

    private void Start()
    {
        if (chatUI == null)
            Debug.LogError("[ChatTester] chatUI chưa được gán! Kéo GameObject có script ChatUI vào field 'Chat UI' trên ChatTester.", this);
        else
            Debug.Log("[ChatTester] Sẵn sàng. Nhấn SPACE để gửi chat.", this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (chatUI == null)
            {
                Debug.LogError("[ChatTester] chatUI = null, không thể gửi message!", this);
                return;
            }

            _count++;
            string msg = $"Message #{_count} ({Time.time:F1}s)";
            Debug.Log($"[ChatTester] Gửi: {msg}");
            chatUI.AddMessage(msg);
        }
    }
}

