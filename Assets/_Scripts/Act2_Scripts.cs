using UnityEngine;

namespace Animation2D
{
    public class Act2_Scripts : MonoBehaviour
    {
        // Hàm này nhận vào một đối tượng ChatUI cụ thể từ Timeline
        public void First_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Anh em có ý tưởng gì chưa nhỉ?");
            else
                Debug.LogError("Lỗi: Bạn chưa kéo ChatUI vào Signal Receiver trong Timeline!");
        }

        public void Second_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Em có background của trường.");
        }

        public void Third_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Hay mình làm cảnh mở đầu ở trường.");
        }

        public void Fourth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Ừ ý đó cũng ổn đấy.");
        }

        public void Fifth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Tận dụng file thầy cho làm nhân vật.");
        }

        public void Sixth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Thử nhá.");
        }

        public void Seventh_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Đến FPT rồi.");
        }

        public void Eighth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Ban đêm đẹp thật.");
        }

        public void Ninth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Thêm vài nhân vật vào cho hay.");
        }

        public void Tenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Ok luôn.");
        }

        public void Eleventh_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Thêm nhân vật đi ");
        }

        public void Twelfth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Thêm vài con quái nữa anh");
        }

        public void Thirteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Thêm tý animation đi bạn");
        }

        public void Fourteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Làm thêm cho đánh nhau thử đi anh ");
        }

        public void Fifteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Phải cho con quáy đánh lại chứ");
        }

        public void Sixteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Con kia sợ quá cho nó chạy đi anh");
        }

        public void Seventeenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Anh thấy hơi chán nhỉ");
        }

        public void Eighteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Ông em có ý tưởng gì không");
        }

        public void Nineteenth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Em có bộ asset đẹp lắm");
        }

        public void Twentieth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Vào thôi em");
        }

        public void TwentyFirst_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Lâu đài tình ái hả em");
        }

        public void TwentySecond_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Anh em xem có ý tưởng gì không");
        }

        public void TwentyThird_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Em cho 1 con quái đi vào nhé ");
        }

        public void TwentyFourth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Linh ra bảo vệ vui đi");
        }

        public void TwentyFifth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Thêm trap đừng cho vào ");
        }

        public void TwentySixth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Chưa xong đâu anh");
        }

        public void TwentySeventh_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Ơ sao không bắt vua");
        }

        public void TwentyEighth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Thế cho bất ngờ bạn");
        }

        public void TwentyNinth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Cũng ổn rồi anh em merge code thôi ");
        }

        public void Thirtieth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Ok");
        }

        public void ThirtyFirst_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Ok");
        }

        public void ThirtySecond_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Cho máu hiện lên luôn.");
        }

        public void ThirtyThird_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Con quái yếu quá anh.");
        }

        public void ThirtyFourth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Tăng máu nó lên.");
        }

        public void ThirtyFifth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Cho thêm 2 con nữa đi.");
        }

        public void ThirtySixth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Ui đông quá rồi.");
        }

        public void ThirtySeventh_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Nhân vật sắp thua rồi.");
        }

        public void ThirtyEighth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Huy:Cho skill đặc biệt đi.");
        }

        public void ThirtyNinth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Hải:Hiệu ứng skill đẹp đấy.");
        }

        public void Fortieth_Mess(ChatUI targetUI)
        {
            if (targetUI != null)
                targetUI.AddMessage("Việt:Ok cảnh này dùng được rồi.");
        }
    }
}