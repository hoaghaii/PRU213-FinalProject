using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation2D
{
    
    public class Act4Controller : MonoBehaviour
    {
        [SerializeField] private ChatUI chatUI;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void FirstMsg()
        {
            chatUI.AddMessage("SpawnFireball() { x: 10, y: 20 }");
        }
        
        public void SecondMsg()
        {
            chatUI.AddMessage("Hải: Vãi nó đánh cháy asset trường rồi kìa");
        }
        
        public void ThirdMsg()
        {
            chatUI.AddMessage("Việt: Ai làm gì tiếp đi");
        }

        public void FourthMsg()
        {
            chatUI.AddMessage("for (int i = 0; i < 15; i++) { SummonPNGFile(); }");
        }
        
        public void FifthMsg()
        {
            chatUI.AddMessage("Huy: Tìm đc chỗ conflict rồi kìa, fix đi!!!!");
        }

        public void SixthMsg()
        {
            chatUI.AddMessage("Việt: Ae để tôi");
        }

        public void SeventhMsg()
        {
            chatUI.AddMessage("git add .\ngit commit -m \"final fix\"");
        }

        public void EightthMsg()
        {
            chatUI.AddMessage("git push origin main");
        }

        public void NinthMsg()
        {
            chatUI.AddMessage("Hải: May quá hết conflict rồi nộp bài cho thầy đi");
        }
        public void TenthMsg()
        {
            chatUI.AddMessage("Việt, Huy, Hải: Thành công rồi ae ơiiiii");
        }
    }
}
