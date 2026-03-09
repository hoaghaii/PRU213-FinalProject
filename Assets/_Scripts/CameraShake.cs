using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Settings")]
    public float shakeDuration = 2f; // Chỉnh thời gian rung ở đây cho dễ

    // Unity chỉ hiện hàm có 1 tham số duy nhất ở Signal Receiver
    public void Shake(float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ProcessShake(shakeDuration, magnitude));
    }

    IEnumerator ProcessShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}