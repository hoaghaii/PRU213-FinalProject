using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation2D.Story
{
    /// <summary>
    /// Định nghĩa một Act trong câu chuyện.
    /// Mỗi Act chứa các DialogueLine và có thể trigger events.
    /// </summary>
    [Serializable]
    public class StoryAct
    {
        public string ActName;
        [TextArea(2, 4)] public string Description;
        public float StartTime; // Thời điểm bắt đầu Act (giây)
        public float Duration;  // Thời lượng Act
        public List<DialogueLine> Dialogues = new List<DialogueLine>();
        
        [Header("Act Events")]
        public string OnStartEvent;  // Event name để trigger khi Act bắt đầu
        public string OnEndEvent;    // Event name để trigger khi Act kết thúc
    }

    /// <summary>
    /// Một dòng dialogue trong Act.
    /// </summary>
    [Serializable]
    public class DialogueLine
    {
        public string Speaker;           // Tên người nói (hoặc để trống cho system message)
        [TextArea(2, 5)] public string Text;
        public float DisplayTime = 2f;   // Thời gian hiển thị
        public AudioClip VoiceClip;      // Voice clip (optional)
        public Sprite Portrait;          // Portrait của speaker (optional)
    }
}

