# 📖 Hướng Dẫn Sử Dụng Code Base PRU213

## Mục Lục
1. [Tổng Quan](#1-tổng-quan)
2. [Cấu Trúc Thư Mục](#2-cấu-trúc-thư-mục)
3. [Core Managers](#3-core-managers)
4. [Story System](#4-story-system)
5. [Battle System](#5-battle-system)
6. [UI Components](#6-ui-components)
7. [Hướng Dẫn Setup Scene](#7-hướng-dẫn-setup-scene)
8. [Ví Dụ Thực Tế](#8-ví-dụ-thực-tế)

---

## 1. Tổng Quan

Code base này được thiết kế cho dự án Animation 2D với chủ đề "Git Conflict Boss Battle". 

**Đặc điểm:**
- 🎯 **Đơn giản** - Dễ hiểu, dễ mở rộng
- 🔌 **Event-driven** - Sử dụng UnityEvent để giao tiếp
- 🎮 **Singleton pattern** - Các Manager có thể truy cập từ bất kỳ đâu
- 📦 **Modular** - Tách biệt Story, Battle, UI

**Namespaces:**
```csharp
using Animation2D.Core;    // GameManager, AudioManager
using Animation2D.Story;   // StoryController, StoryAct, DialogueLine
using Animation2D.Battle;  // BossController, PlayerAttackController
using Animation2D.UI;      // DialogueUI, BossUI, ChatUI, TerminalUI, TimerUI
```

---

## 2. Cấu Trúc Thư Mục

```
Assets/_Scripts/
│
├── Core/                          # Managers chính
│   ├── Singleton.cs               # Base class Singleton
│   ├── GameManager.cs             # Quản lý state game
│   └── AudioManager.cs            # Quản lý âm thanh
│
├── Story/                         # Hệ thống kịch bản
│   ├── StoryData.cs               # Data classes
│   └── StoryController.cs         # Điều khiển story
│
├── Battle/                        # Hệ thống chiến đấu
│   ├── BossController.cs          # Controller Boss
│   └── PlayerAttackController.cs  # Skill tấn công
│
└── UI/                            # UI Components
    ├── DialogueUI.cs              # Dialogue box
    ├── BossUI.cs                  # Boss HP bar
    ├── ChatUI.cs                  # Chat messages
    ├── TerminalUI.cs              # Terminal effect
    └── TimerUI.cs                 # Timer countdown
```

---

## 3. Core Managers

### 3.1 GameManager

Quản lý trạng thái game (Playing, Paused, Cutscene, Battle, etc.)

```csharp
using Animation2D.Core;

// === TRUY CẬP ===
GameManager.Instance.CurrentState;  // Lấy state hiện tại

// === ĐỔI TRẠNG THÁI ===
GameManager.Instance.StartGame();      // → Playing
GameManager.Instance.StartCutscene();  // → Cutscene
GameManager.Instance.StartBattle();    // → Battle
GameManager.Instance.PauseGame();      // → Paused (Time.timeScale = 0)
GameManager.Instance.ResumeGame();     // → Playing
GameManager.Instance.GameOver();       // → GameOver
GameManager.Instance.ReturnToMainMenu(); // → MainMenu

// === TOGGLE PAUSE ===
GameManager.Instance.TogglePause();  // Pause ↔ Resume

// === KIỂM TRA ===
if (GameManager.Instance.IsPaused) { }
if (GameManager.Instance.IsPlaying) { }
if (GameManager.Instance.IsInCutscene) { }
if (GameManager.Instance.IsInBattle) { }

// === EVENTS ===
GameManager.Instance.OnStateChanged.AddListener((GameState state) => {
    Debug.Log($"State changed to: {state}");
});

GameManager.Instance.OnGamePaused.AddListener(() => {
    Debug.Log("Game paused!");
});

GameManager.Instance.OnGameResumed.AddListener(() => {
    Debug.Log("Game resumed!");
});
```

**GameState enum:**
| State | Mô tả |
|-------|-------|
| `MainMenu` | Đang ở menu chính |
| `Playing` | Đang chơi |
| `Paused` | Tạm dừng |
| `Cutscene` | Đang xem cutscene |
| `Battle` | Đang trong boss battle |
| `GameOver` | Game over |

---

### 3.2 AudioManager

Quản lý phát BGM và SFX.

```csharp
using Animation2D.Core;

// === BGM ===
AudioManager.Instance.PlayBGM(audioClip);           // Phát BGM (loop)
AudioManager.Instance.PlayBGM(audioClip, false);    // Phát không loop
AudioManager.Instance.StopBGM();
AudioManager.Instance.PauseBGM();
AudioManager.Instance.ResumeBGM();

// === SFX ===
AudioManager.Instance.PlaySFX(sfxClip);             // Phát SFX
AudioManager.Instance.PlaySFX(sfxClip, 0.5f);       // Volume 50%
AudioManager.Instance.PlaySFXAtPosition(clip, position);  // 3D sound

// === VOLUME CONTROL ===
AudioManager.Instance.SetMasterVolume(0.8f);  // 0.0 - 1.0
AudioManager.Instance.SetBGMVolume(0.5f);
AudioManager.Instance.SetSFXVolume(1.0f);

// === STOP ALL ===
AudioManager.Instance.StopAll();
```

---

## 4. Story System

### 4.1 Data Classes (StoryData.cs)

```csharp
// === STORY ACT ===
// Một Act trong câu chuyện (ví dụ: ACT 1, ACT 2...)
[Serializable]
public class StoryAct
{
    public string ActName;           // "ACT 1: Mọi thứ tưởng ổn"
    public string Description;       // Mô tả
    public float StartTime;          // Thời điểm bắt đầu
    public float Duration;           // Thời lượng
    public List<DialogueLine> Dialogues;  // Các dialogue
    public string OnStartEvent;      // Event khi Act bắt đầu
    public string OnEndEvent;        // Event khi Act kết thúc
}

// === DIALOGUE LINE ===
// Một dòng dialogue
[Serializable]
public class DialogueLine
{
    public string Speaker;           // "Nam", "Minh", "" (system)
    public string Text;              // "Đã commit xong!"
    public float DisplayTime;        // Thời gian hiển thị
    public AudioClip VoiceClip;      // Voice (optional)
    public Sprite Portrait;          // Portrait (optional)
}
```

### 4.2 StoryController

Điều khiển flow của câu chuyện.

```csharp
using Animation2D.Story;

// === BẮT ĐẦU STORY ===
StoryController.Instance.StartStory();

// === ĐIỀU KHIỂN ===
StoryController.Instance.NextAct();       // Chuyển Act tiếp theo
StoryController.Instance.NextDialogue();  // Dialogue tiếp theo
StoryController.Instance.PauseStory();
StoryController.Instance.ResumeStory();
StoryController.Instance.StopStory();

// === NHẢY ĐẾN ACT ===
StoryController.Instance.GoToAct(2);           // Theo index
StoryController.Instance.GoToAct("ACT 3");     // Theo tên

// === TRIGGER EVENT THỦ CÔNG ===
StoryController.Instance.TriggerEvent("BossAppear");

// === KIỂM TRA ===
StoryController.Instance.IsPlaying;
StoryController.Instance.CurrentAct;
StoryController.Instance.StoryTime;

// === EVENTS ===
StoryController.Instance.OnActStarted.AddListener((StoryAct act) => {
    Debug.Log($"Starting: {act.ActName}");
});

StoryController.Instance.OnActEnded.AddListener((StoryAct act) => {
    Debug.Log($"Ended: {act.ActName}");
});

StoryController.Instance.OnDialogueShow.AddListener((DialogueLine line) => {
    Debug.Log($"{line.Speaker}: {line.Text}");
});

StoryController.Instance.OnStoryCompleted.AddListener(() => {
    Debug.Log("Story completed!");
});

// Event tùy chỉnh (từ Act.OnStartEvent, Act.OnEndEvent)
StoryController.Instance.OnStoryEvent.AddListener((string eventName) => {
    switch (eventName)
    {
        case "BossAppear":
            // Spawn boss
            break;
        case "StartBattle":
            GameManager.Instance.StartBattle();
            break;
    }
});
```

---

## 5. Battle System

### 5.1 BossController

Controller cho Boss với HP, phases, damage.

```csharp
using Animation2D.Battle;

// === SETUP (Inspector hoặc code) ===
// Thêm BossController component vào Boss GameObject

// === DAMAGE ===
boss.TakeDamage(10f);      // Gây 10 damage
boss.Heal(5f);             // Hồi 5 HP
boss.SetHP(50f);           // Set HP trực tiếp

// === KIỂM TRA ===
boss.CurrentHP;            // HP hiện tại
boss.MaxHP;                // HP tối đa
boss.HPPercent;            // 0.0 - 1.0
boss.IsAlive;              // true/false
boss.BossName;             // "GIT CONFLICT"

// === ATTACK (gọi trong Animation Events hoặc AI) ===
boss.Attack();             // Attack thường
boss.SpecialAttack(0);     // Special attack

// === RESET ===
boss.Reset();              // Reset về trạng thái ban đầu

// === EVENTS ===
boss.OnHPChanged.AddListener((float current, float max) => {
    Debug.Log($"HP: {current}/{max}");
});

boss.OnDamageTaken.AddListener((float damage) => {
    Debug.Log($"Boss took {damage} damage!");
});

boss.OnPhaseChanged.AddListener((int phase) => {
    Debug.Log($"Phase {phase}!");
    // Phase 1: HP > 50%
    // Phase 2: HP 25-50%
    // Phase 3: HP < 25%
});

boss.OnBossDefeated.AddListener(() => {
    Debug.Log("VICTORY!");
});
```

### 5.2 PlayerAttackController

Các skill tấn công của Player (mapping với kịch bản).

```csharp
using Animation2D.Battle;

// === SETUP ===
// Thêm PlayerAttackController vào Player
// Kéo Boss vào field "Target Boss" trong Inspector

// === CÁC SKILL ===
// ACT 3: Code Bullet (-10 HP)
playerAttack.UseCodeBullet();

// ACT 4: Asset Throw (-15 HP)
playerAttack.UseAssetThrow();

// ACT 5: Merge Slash (-30 HP)
playerAttack.UseMergeSlash();

// ACT 6: Particle Shockwave (-20 HP)
playerAttack.UseParticleShockwave();

// ACT 7: Final Commit (3 hits, -25 HP each)
playerAttack.UseFinalCommit();  // git add, commit, push

// === DAMAGE THỦ CÔNG ===
playerAttack.DealDamage(50f, "CustomAttack");

// === SET TARGET (runtime) ===
playerAttack.SetTargetBoss(bossController);

// === EVENTS ===
playerAttack.OnAttackUsed.AddListener((string attackName) => {
    Debug.Log($"Used: {attackName}");
});

playerAttack.OnDamageDealt.AddListener((float damage) => {
    Debug.Log($"Dealt {damage} damage!");
});
```

---

## 6. UI Components

### 6.1 DialogueUI

Dialogue box với typewriter effect.

```csharp
using Animation2D.UI;

// Tự động kết nối với StoryController
// Chỉ cần thêm component và setup UI references

// === THỦ CÔNG ===
dialogueUI.ShowDialogue(dialogueLine);
dialogueUI.Hide();
dialogueUI.Continue();  // Skip typewriter hoặc next dialogue
```

**Inspector Setup:**
- `Dialogue Panel`: GameObject chứa dialogue UI
- `Speaker Text`: TextMeshProUGUI hiển thị tên speaker
- `Dialogue Text`: TextMeshProUGUI hiển thị text
- `Portrait Image`: Image hiển thị portrait
- `Continue Button`: Button để next dialogue
- `Use Typewriter`: Bật/tắt hiệu ứng đánh chữ
- `Type Speed`: Tốc độ đánh chữ (0.03 = 30ms/ký tự)

---

### 6.2 BossUI

HP bar và thông tin Boss.

```csharp
using Animation2D.UI;

// === SETUP ===
bossUI.SetupBoss(bossController);  // Kết nối với Boss

// === HIỂN THỊ ===
bossUI.Show();
bossUI.Hide();
```

**Inspector Setup:**
- `Boss UI Panel`: Panel chứa UI
- `Boss Name Text`: Tên boss
- `HP Slider`: Slider HP bar
- `HP Text`: Text hiển thị "HP: 80/100"
- `HP Fill`: Image của HP bar (để đổi màu)
- `High/Mid/Low HP Color`: Màu theo mức HP
- `Boss`: Reference đến BossController

---

### 6.3 ChatUI

Hiển thị chat messages (Visual Novel style).

```csharp
using Animation2D.UI;

// === THÊM MESSAGE ===
chatUI.AddMessage("Hello world");
chatUI.AddChatMessage("Nam", "Đã commit xong!");
chatUI.AddSystemMessage("git push origin main");
chatUI.AddErrorMessage("CONFLICT: Merge failed");
chatUI.AddSuccessMessage("Merge successful!");

// === CONTROL ===
chatUI.ClearMessages();
chatUI.Show();
chatUI.Hide();
```

**MessageType:**
- `Normal` - Màu trắng
- `Warning` - Màu vàng
- `Error` - Màu đỏ
- `Success` - Màu xanh

---

### 6.4 TerminalUI

Terminal/code typing effect.

```csharp
using Animation2D.UI;

// === TYPING ===
terminalUI.TypeLine("git status");        // > git status
terminalUI.TypeText("Hello world\n");     // Không có prefix
terminalUI.TypeCode(new string[] {        // Nhiều dòng
    "git add .",
    "git commit -m 'fix'",
    "git push"
}, 0.5f);  // Delay 0.5s giữa các dòng

// === SPECIAL ===
terminalUI.ShowGitConflict();      // Hiện conflict message
terminalUI.ShowConflictMarkers();  // Hiện <<<<<<< ======= >>>>>>>

// === CONTROL ===
terminalUI.Clear();
terminalUI.AppendImmediate("text");  // Không có animation
terminalUI.Show();
terminalUI.Hide();

// === EVENTS ===
terminalUI.OnTypeComplete.AddListener(() => {
    Debug.Log("Typing done!");
});
```

---

### 6.5 TimerUI

Đồng hồ đếm ngược (deadline effect).

```csharp
using Animation2D.UI;

// === CONTROL ===
timerUI.StartTimer();
timerUI.StopTimer();
timerUI.ResetTimer();
timerUI.SetTime(60f);  // Set 60 giây

// === DISPLAY CUSTOM TIME ===
timerUI.DisplayTime(23, 59);  // Hiển thị 23:59

// === PROPERTIES ===
timerUI.CurrentTime;
timerUI.TotalTime;
timerUI.IsRunning;

// === EVENTS ===
timerUI.OnWarningStart.AddListener(() => {
    Debug.Log("Warning! Time is running out!");
});

timerUI.OnTimerComplete.AddListener(() => {
    Debug.Log("Time's up!");
});
```

---

## 7. Hướng Dẫn Setup Scene

### Bước 1: Tạo Managers

```
Hierarchy:
└── Managers (Empty GameObject)
    ├── GameManager (Component)
    └── AudioManager (Component)
```

### Bước 2: Tạo StoryController

```
Hierarchy:
└── StoryController (Empty GameObject)
    └── StoryController (Component)
        └── Thêm Acts trong Inspector
```

### Bước 3: Tạo Boss

```
Hierarchy:
└── Boss (Sprite)
    ├── SpriteRenderer
    ├── Animator (optional)
    └── BossController (Component)
```

### Bước 4: Tạo Player

```
Hierarchy:
└── Player (Sprite)
    ├── SpriteRenderer
    ├── Animator
    └── PlayerAttackController (Component)
        └── Drag Boss vào "Target Boss"
```

### Bước 5: Tạo UI

```
Hierarchy:
└── Canvas
    ├── DialoguePanel
    │   ├── Background
    │   ├── SpeakerText (TMP)
    │   ├── DialogueText (TMP)
    │   ├── PortraitImage
    │   └── ContinueButton
    │   └── DialogueUI (Component)
    │
    ├── BossUIPanel
    │   ├── BossNameText (TMP)
    │   ├── HPSlider
    │   └── HPText (TMP)
    │   └── BossUI (Component)
    │
    ├── ChatPanel
    │   ├── MessageContainer (Vertical Layout)
    │   └── MessagePrefab
    │   └── ChatUI (Component)
    │
    ├── TerminalPanel
    │   ├── TerminalText (TMP)
    │   └── Cursor (Image)
    │   └── TerminalUI (Component)
    │
    └── TimerPanel
        ├── TimerText (TMP)
        └── TimerFill (Image)
        └── TimerUI (Component)
```

---

## 8. Ví Dụ Thực Tế

### Ví dụ 1: Setup một Act đơn giản

```csharp
// Trong Inspector của StoryController, thêm Act:
// ActName: "ACT 1: Mọi thứ tưởng ổn"
// OnStartEvent: "ShowTimer"
// Dialogues:
//   [0] Speaker: "Nam", Text: "Cuối cùng cũng xong!"
//   [1] Speaker: "Minh", Text: "Chờ tao merge nào..."
//   [2] Speaker: "", Text: "22:50 - Đêm trước deadline..."
```

### Ví dụ 2: Script điều khiển flow game

```csharp
using UnityEngine;
using Animation2D.Core;
using Animation2D.Story;
using Animation2D.Battle;
using Animation2D.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private BossController boss;
    [SerializeField] private BossUI bossUI;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private TerminalUI terminalUI;

    void Start()
    {
        // Setup events
        StoryController.Instance.OnStoryEvent.AddListener(HandleStoryEvent);
        StoryController.Instance.OnStoryCompleted.AddListener(OnStoryComplete);
        boss.OnBossDefeated.AddListener(OnBossDefeated);

        // Bắt đầu story
        StoryController.Instance.StartStory();
    }

    void HandleStoryEvent(string eventName)
    {
        switch (eventName)
        {
            case "ShowTimer":
                timerUI.Show();
                timerUI.DisplayTime(22, 50);
                break;

            case "GitConflict":
                terminalUI.Show();
                terminalUI.ShowGitConflict();
                break;

            case "BossAppear":
                boss.gameObject.SetActive(true);
                bossUI.SetupBoss(boss);
                bossUI.Show();
                GameManager.Instance.StartBattle();
                break;

            case "Victory":
                timerUI.DisplayTime(23, 59);
                break;
        }
    }

    void OnBossDefeated()
    {
        GameManager.Instance.EndCutscene();
        StoryController.Instance.NextAct();
    }

    void OnStoryComplete()
    {
        Debug.Log("Animation hoàn thành!");
        // Load credits scene, etc.
    }
}
```

### Ví dụ 3: Trigger attack từ Animation Event

```csharp
using UnityEngine;
using Animation2D.Battle;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAttackController attackController;

    // Gọi từ Animation Event
    public void OnAttackHit()
    {
        attackController.UseCodeBullet();
    }

    public void OnSpecialAttack()
    {
        attackController.UseMergeSlash();
    }
}
```

---

## Tips & Best Practices

1. **Luôn kiểm tra null khi dùng Singleton:**
   ```csharp
   if (GameManager.Instance != null)
       GameManager.Instance.PauseGame();
   ```

2. **Unsubscribe events trong OnDestroy:**
   ```csharp
   void OnDestroy()
   {
       if (StoryController.Instance != null)
           StoryController.Instance.OnActStarted.RemoveListener(MyHandler);
   }
   ```

3. **Sử dụng OnStoryEvent cho custom logic:**
   ```csharp
   // Trong Act, set OnStartEvent = "MyCustomEvent"
   // Trong code, lắng nghe:
   StoryController.Instance.OnStoryEvent.AddListener((e) => {
       if (e == "MyCustomEvent") DoSomething();
   });
   ```

4. **Extend BossController cho boss đặc biệt:**
   ```csharp
   public class GitConflictBoss : BossController
   {
       public override void Attack()
       {
           base.Attack();
           // Custom attack logic
       }
   }
   ```

---

**Happy Coding! 🎮**

