using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Animation2D.Core;

namespace Animation2D.Story
{
    /// <summary>
    /// Điều khiển flow của câu chuyện theo các Act.
    /// Hỗ trợ auto-play và manual control.
    /// </summary>
    public class StoryController : Singleton<StoryController>
    {
        [Header("Story Configuration")]
        [SerializeField] private List<StoryAct> _acts = new List<StoryAct>();
        [SerializeField] private bool _autoPlay = false;

        [Header("Current State")]
        [SerializeField] private int _currentActIndex = -1;
        [SerializeField] private int _currentDialogueIndex = 0;
        [SerializeField] private float _storyTime = 0f;
        [SerializeField] private bool _isPlaying = false;

        // Events
        public UnityEvent<StoryAct> OnActStarted = new UnityEvent<StoryAct>();
        public UnityEvent<StoryAct> OnActEnded = new UnityEvent<StoryAct>();
        public UnityEvent<DialogueLine> OnDialogueShow = new UnityEvent<DialogueLine>();
        public UnityEvent OnStoryCompleted = new UnityEvent();

        // Custom event system (string-based để dễ mở rộng)
        public UnityEvent<string> OnStoryEvent = new UnityEvent<string>();

        public StoryAct CurrentAct => (_currentActIndex >= 0 && _currentActIndex < _acts.Count) 
            ? _acts[_currentActIndex] : null;
        public bool IsPlaying => _isPlaying;
        public float StoryTime => _storyTime;
        public List<StoryAct> Acts => _acts;

        private Coroutine _playCoroutine;

        /// <summary>
        /// Bắt đầu story từ đầu
        /// </summary>
        public void StartStory()
        {
            _storyTime = 0f;
            _currentActIndex = -1;
            _currentDialogueIndex = 0;
            _isPlaying = true;

            if (_autoPlay)
            {
                _playCoroutine = StartCoroutine(AutoPlayRoutine());
            }
            else
            {
                NextAct();
            }
        }

        /// <summary>
        /// Dừng story
        /// </summary>
        public void StopStory()
        {
            _isPlaying = false;
            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
                _playCoroutine = null;
            }
        }

        /// <summary>
        /// Pause story
        /// </summary>
        public void PauseStory()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// Resume story
        /// </summary>
        public void ResumeStory()
        {
            _isPlaying = true;
        }

        /// <summary>
        /// Chuyển sang Act tiếp theo
        /// </summary>
        public void NextAct()
        {
            // End current act
            if (CurrentAct != null)
            {
                if (!string.IsNullOrEmpty(CurrentAct.OnEndEvent))
                    OnStoryEvent?.Invoke(CurrentAct.OnEndEvent);
                OnActEnded?.Invoke(CurrentAct);
            }

            _currentActIndex++;
            _currentDialogueIndex = 0;

            if (_currentActIndex >= _acts.Count)
            {
                // Story completed
                _isPlaying = false;
                OnStoryCompleted?.Invoke();
                Debug.Log("[StoryController] Story completed!");
                return;
            }

            // Start new act
            var act = CurrentAct;
            Debug.Log($"[StoryController] Starting Act: {act.ActName}");
            
            if (!string.IsNullOrEmpty(act.OnStartEvent))
                OnStoryEvent?.Invoke(act.OnStartEvent);
            
            OnActStarted?.Invoke(act);

            // Show first dialogue if exists
            if (act.Dialogues.Count > 0)
            {
                ShowDialogue(act.Dialogues[0]);
            }
        }

        /// <summary>
        /// Chuyển sang dialogue tiếp theo trong Act hiện tại
        /// </summary>
        public void NextDialogue()
        {
            if (CurrentAct == null) return;

            _currentDialogueIndex++;

            if (_currentDialogueIndex >= CurrentAct.Dialogues.Count)
            {
                // Act completed, move to next
                NextAct();
                return;
            }

            ShowDialogue(CurrentAct.Dialogues[_currentDialogueIndex]);
        }

        /// <summary>
        /// Nhảy đến Act cụ thể theo index
        /// </summary>
        public void GoToAct(int actIndex)
        {
            if (actIndex < 0 || actIndex >= _acts.Count)
            {
                Debug.LogWarning($"[StoryController] Invalid act index: {actIndex}");
                return;
            }

            _currentActIndex = actIndex - 1; // -1 vì NextAct sẽ ++
            NextAct();
        }

        /// <summary>
        /// Nhảy đến Act theo tên
        /// </summary>
        public void GoToAct(string actName)
        {
            int index = _acts.FindIndex(a => a.ActName == actName);
            if (index >= 0)
            {
                GoToAct(index);
            }
            else
            {
                Debug.LogWarning($"[StoryController] Act not found: {actName}");
            }
        }

        /// <summary>
        /// Trigger một story event thủ công
        /// </summary>
        public void TriggerEvent(string eventName)
        {
            OnStoryEvent?.Invoke(eventName);
        }

        private void ShowDialogue(DialogueLine dialogue)
        {
            OnDialogueShow?.Invoke(dialogue);
            Debug.Log($"[Dialogue] {dialogue.Speaker}: {dialogue.Text}");
        }

        private IEnumerator AutoPlayRoutine()
        {
            NextAct();

            while (_isPlaying && CurrentAct != null)
            {
                // Wait for current dialogue display time
                if (CurrentAct.Dialogues.Count > 0 && _currentDialogueIndex < CurrentAct.Dialogues.Count)
                {
                    var dialogue = CurrentAct.Dialogues[_currentDialogueIndex];
                    yield return new WaitForSeconds(dialogue.DisplayTime);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }

                if (_isPlaying)
                {
                    NextDialogue();
                }
            }
        }

        private void Update()
        {
            if (_isPlaying)
            {
                _storyTime += Time.deltaTime;
            }
        }

        #region Editor Helper - Add Acts

        /// <summary>
        /// Thêm Act mới (cho editor/runtime)
        /// </summary>
        public void AddAct(StoryAct act)
        {
            _acts.Add(act);
        }

        /// <summary>
        /// Xóa tất cả Acts
        /// </summary>
        public void ClearActs()
        {
            _acts.Clear();
        }

        #endregion
    }
}

