using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Animation2D.Battle;

namespace Animation2D.UI
{
    /// <summary>
    /// UI hiển thị thông tin Boss (HP bar, tên, phase).
    /// </summary>
    public class BossUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _bossUIPanel;
        [SerializeField] private TextMeshProUGUI _bossNameText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private Image _hpFill;

        [Header("HP Bar Colors")]
        [SerializeField] private Color _highHPColor = Color.green;
        [SerializeField] private Color _midHPColor = Color.yellow;
        [SerializeField] private Color _lowHPColor = Color.red;

        [Header("Boss Reference")]
        [SerializeField] private BossController _boss;

        [Header("Animation")]
        [SerializeField] private float _hpLerpSpeed = 5f;

        private float _displayedHP;

        private void Start()
        {
            if (_boss != null)
            {
                SetupBoss(_boss);
            }
        }

        /// <summary>
        /// Kết nối với Boss
        /// </summary>
        public void SetupBoss(BossController boss)
        {
            _boss = boss;

            if (_boss != null)
            {
                _boss.OnHPChanged.AddListener(OnHPChanged);
                _boss.OnBossDefeated.AddListener(OnBossDefeated);
                _boss.OnPhaseChanged.AddListener(OnPhaseChanged);

                if (_bossNameText != null)
                    _bossNameText.text = _boss.BossName;

                _displayedHP = _boss.CurrentHP;
                UpdateHPUI(_boss.CurrentHP, _boss.MaxHP);
                Show();
            }
        }

        private void Update()
        {
            // Smooth HP bar lerp
            if (_boss != null && _hpSlider != null)
            {
                _displayedHP = Mathf.Lerp(_displayedHP, _boss.CurrentHP, Time.deltaTime * _hpLerpSpeed);
                _hpSlider.value = _displayedHP / _boss.MaxHP;
            }
        }

        private void OnHPChanged(float currentHP, float maxHP)
        {
            UpdateHPUI(currentHP, maxHP);
        }

        private void UpdateHPUI(float currentHP, float maxHP)
        {
            // Update text
            if (_hpText != null)
            {
                _hpText.text = $"{Mathf.CeilToInt(currentHP)}/{Mathf.CeilToInt(maxHP)}";
            }

            // Update color based on HP percent
            if (_hpFill != null)
            {
                float percent = currentHP / maxHP;
                if (percent > 0.5f)
                    _hpFill.color = _highHPColor;
                else if (percent > 0.25f)
                    _hpFill.color = _midHPColor;
                else
                    _hpFill.color = _lowHPColor;
            }
        }

        private void OnBossDefeated()
        {
            Debug.Log("[BossUI] Boss defeated - show victory!");
            // Có thể show animation victory ở đây
        }

        private void OnPhaseChanged(int phase)
        {
            Debug.Log($"[BossUI] Phase changed to {phase}");
            // Có thể đổi UI theme theo phase
        }

        /// <summary>
        /// Hiện Boss UI
        /// </summary>
        public void Show()
        {
            if (_bossUIPanel != null)
                _bossUIPanel.SetActive(true);
        }

        /// <summary>
        /// Ẩn Boss UI
        /// </summary>
        public void Hide()
        {
            if (_bossUIPanel != null)
                _bossUIPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_boss != null)
            {
                _boss.OnHPChanged.RemoveListener(OnHPChanged);
                _boss.OnBossDefeated.RemoveListener(OnBossDefeated);
                _boss.OnPhaseChanged.RemoveListener(OnPhaseChanged);
            }
        }
    }
}

