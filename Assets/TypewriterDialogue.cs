using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterDialogue : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.2f;

    Coroutine typingCoroutine;

    public void ShowDialogue(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        textUI.text = "";

        foreach (char c in message)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
