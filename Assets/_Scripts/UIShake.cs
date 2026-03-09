using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null) originalPos = rectTransform.anchoredPosition;
    }

    // Hàm này để gọi từ Signal Timeline
    public void ShakeUI(float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ProcessShake(0.7f, magnitude)); // 0.5s là thời gian rung
    }

    IEnumerator ProcessShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude * 100f; // Nhân 100 để thấy rõ trên UI
            float y = Random.Range(-1f, 1f) * magnitude * 100f;

            rectTransform.anchoredPosition = new Vector2(originalPos.x + x, originalPos.y + y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}