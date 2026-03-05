using System.Collections;
using UnityEngine;

/// <summary>
/// Chat window với 2 bubble slot cố định trong prefab (không Instantiate/Destroy).
/// slot[0] = bubble trên (cũ hơn) — slot[1] = bubble dưới (mới nhất).
/// Khi có message mới:
///   1. slot[0] fade ra
///   2. slot[1] move lên vị trí slot[0]
///   3. slot[1] (vị trí mới) hiện message mới
/// </summary>
public class ChatUI : MonoBehaviour
{
    [Header("Bubble Slots — kéo trực tiếp từ prefab hierarchy")]
    [SerializeField] private ChatBubble slot0; // bubble trên (cũ hơn)
    [SerializeField] private ChatBubble slot1; // bubble dưới (mới nhất)

    [Header("Behavior")]
    [SerializeField] private float fadeDuration    = 0.3f;
    [SerializeField] private float slideDuration   = 0.25f;
    [SerializeField] private float slideUpDistance = 80f;

    // lưu vị trí gốc của 2 slot để biết chỗ cần reset về
    private Vector2 _slot0OriginPos;
    private Vector2 _slot1OriginPos;

    private bool _isAnimating;
    private string _pendingMessage;

    private void Awake()
    {
        if (slot0 == null || slot1 == null)
        {
            Debug.LogError("[ChatUI] slot0 hoặc slot1 chưa được gán!", this);
            return;
        }

        _slot0OriginPos = ((RectTransform)slot0.transform).anchoredPosition;
        _slot1OriginPos = ((RectTransform)slot1.transform).anchoredPosition;

        // Ẩn cả 2 lúc đầu
        slot0.HideImmediate();
        slot1.HideImmediate();
    }

    // ─────────────────────────────────────────
    //  PUBLIC API
    // ─────────────────────────────────────────

    public void AddMessage(string message)
    {
        if (_isAnimating)
        {
            // Đang animate → queue message, hiện sau khi xong
            _pendingMessage = message;
            return;
        }
        StartCoroutine(ShowMessageRoutine(message));
    }

    // ─────────────────────────────────────────
    //  INTERNAL
    // ─────────────────────────────────────────

    private IEnumerator ShowMessageRoutine(string message)
    {
        _isAnimating = true;

        var rt0 = (RectTransform)slot0.transform;
        var rt1 = (RectTransform)slot1.transform;

        bool slot1HasContent = slot1.IsVisible;

        if (!slot1HasContent)
        {
            // ── Lần đầu: chỉ hiện slot1 ở vị trí gốc ──
            rt1.anchoredPosition = _slot1OriginPos;
            slot1.Show(message);
        }
        else
        {
            // ── Đã có nội dung ở slot1: đẩy lên ──

            // 1. Fade OUT slot0 (nếu đang hiện)
            if (slot0.IsVisible)
                yield return StartCoroutine(FadeOut(slot0.CanvasGroup, fadeDuration));

            slot0.HideImmediate();
            rt0.anchoredPosition = _slot0OriginPos;

            // 2. Copy nội dung slot1 → slot0, rồi slide slot0 xuất hiện ở vị trí slot1-origin
            //    (trông như slot1 "dịch chuyển lên")
            slot0.Show(slot1.CanvasGroup != null
                ? GetTextFrom(slot1)
                : "");

            // Đặt slot0 tại vị trí slot1 rồi slide lên về _slot0OriginPos
            rt0.anchoredPosition = _slot1OriginPos;
            slot0.CanvasGroup.alpha = 1f;

            yield return StartCoroutine(SlideToPosition(rt0, _slot0OriginPos, slideDuration));

            // 3. Reset slot1 về vị trí gốc và hiện message mới
            rt1.anchoredPosition = _slot1OriginPos;
            slot1.Show(message);
        }

        _isAnimating = false;

        // Xử lý pending message nếu có
        if (_pendingMessage != null)
        {
            string next = _pendingMessage;
            _pendingMessage = null;
            StartCoroutine(ShowMessageRoutine(next));
        }
    }

    private string GetTextFrom(ChatBubble bubble)
    {
        var tmp = bubble.GetComponentInChildren<TMPro.TMP_Text>();
        return tmp != null ? tmp.text : "";
    }

    private IEnumerator FadeOut(CanvasGroup cg, float duration)
    {
        if (cg == null) yield break;
        float t = 0f;
        float start = cg.alpha;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, 0f, t / duration);
            yield return null;
        }
        cg.alpha = 0f;
    }

    private IEnumerator SlideToPosition(RectTransform rt, Vector2 target, float duration)
    {
        float t = 0f;
        Vector2 start = rt.anchoredPosition;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            rt.anchoredPosition = Vector2.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }
        rt.anchoredPosition = target;
    }
}