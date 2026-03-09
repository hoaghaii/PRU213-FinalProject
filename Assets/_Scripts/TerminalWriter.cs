using UnityEngine;
using TMPro;
using System.Collections;

public class TerminalWriter : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float typingSpeed = 0.05f;
    public AudioSource typingSound;

    private string historyText = "";

    // Dùng cho người dùng gõ lệnh (hiện từng chữ)
    public void StartTyping(string newContent)
    {
        StopAllCoroutines();
        if (!string.IsNullOrEmpty(textElement.text)) historyText = textElement.text + "\n";
        StartCoroutine(TypeText(newContent));
    }

    // Dùng cho hệ thống phản hồi (hiện cả khối hoặc từng dòng nhanh)
    public void PrintSystemBlock(string blockContent)
    {
        if (!string.IsNullOrEmpty(textElement.text)) historyText = textElement.text + "\n";
        
        // Hiện ngay lập tức dòng phản hồi của Git
        textElement.text = historyText + blockContent;
        historyText = textElement.text; // Cập nhật lịch sử để dòng sau không đè lên
    }

    IEnumerator TypeText(string content)
    {
        for (int i = 0; i <= content.Length; i++)
        {
            textElement.text = historyText + content.Substring(0, i);
            if (typingSound != null && i > 0) typingSound.Play();
            yield return new WaitForSeconds(typingSpeed);
        }
        historyText = textElement.text;
    }
}