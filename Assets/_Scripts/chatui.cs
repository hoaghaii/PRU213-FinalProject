using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation2D
{
    public class chatui : MonoBehaviour
    {
        [SerializeField] private ChatUI chatUI;

        public void sendMess(string message)
        {
            // Truyền tham số nhận được vào hàm hiển thị của hệ thống Chat
            chatUI.AddMessage(message);
        }
    }
}