using UnityEngine;

public class TestChat : MonoBehaviour
{
    [SerializeField] private ChatUI chatUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            chatUI.AddMessage("Hello " + Time.time.ToString("F1"));
    }
}