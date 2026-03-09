using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TypewriterDialogue dialogue;

    public void PlayLine(string text)
    {
        dialogue.ShowDialogue(text);
    }
}
