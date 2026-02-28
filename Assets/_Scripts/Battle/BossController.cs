using UnityEngine;
using UnityEngine.Events;
using Animation2D.Core;

namespace Animation2D.Battle
{
    /// <summary>
    /// Controller cơ bản cho Boss.
    /// Quản lý HP, trạng thái và các phase battle.
    /// </summary>
    public class BossController : MonoBehaviour
    {
        [Header("Boss Stats")]
        [SerializeField] private string _bossName = "GIT CONFLICT";
        [SerializeField] private float _maxHP = 100f;
        [SerializeField] private float _currentHP;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        // Events
        public UnityEvent<float, float> OnHPChanged = new UnityEvent<float, float>(); // current, max
        public UnityEvent<float> OnDamageTaken = new UnityEvent<float>();
        public UnityEvent OnBossDefeated = new UnityEvent();
        public UnityEvent<int> OnPhaseChanged = new UnityEvent<int>();

        public string BossName => _bossName;
        public float CurrentHP => _currentHP;
        public float MaxHP => _maxHP;
        public float HPPercent => _currentHP / _maxHP;
        public bool IsAlive => _currentHP > 0;

        private int _currentPhase = 1;

        private void Awake()
        {
            _currentHP = _maxHP;
            
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            OnHPChanged?.Invoke(_currentHP, _maxHP);
        }

        /// <summary>
        /// Gây sát thương cho Boss
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _currentHP = Mathf.Max(0, _currentHP - damage);
            
            OnDamageTaken?.Invoke(damage);
            OnHPChanged?.Invoke(_currentHP, _maxHP);

            Debug.Log($"[Boss] {_bossName} took {damage} damage. HP: {_currentHP}/{_maxHP}");

            // Check phase transitions
            CheckPhaseTransition();

            // Play hit animation
            if (_animator != null)
            {
                _animator.SetTrigger("Hit");
            }

            // Flash effect
            StartCoroutine(FlashDamage());

            if (_currentHP <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Hồi HP cho Boss
        /// </summary>
        public void Heal(float amount)
        {
            _currentHP = Mathf.Min(_maxHP, _currentHP + amount);
            OnHPChanged?.Invoke(_currentHP, _maxHP);
        }

        /// <summary>
        /// Set HP trực tiếp (cho scripted events)
        /// </summary>
        public void SetHP(float hp)
        {
            _currentHP = Mathf.Clamp(hp, 0, _maxHP);
            OnHPChanged?.Invoke(_currentHP, _maxHP);
            
            if (_currentHP <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Reset Boss về trạng thái ban đầu
        /// </summary>
        public void Reset()
        {
            _currentHP = _maxHP;
            _currentPhase = 1;
            OnHPChanged?.Invoke(_currentHP, _maxHP);
            OnPhaseChanged?.Invoke(_currentPhase);
        }

        private void CheckPhaseTransition()
        {
            // Ví dụ: Phase 2 khi HP < 50%, Phase 3 khi HP < 25%
            int newPhase = _currentPhase;

            if (HPPercent <= 0.25f)
                newPhase = 3;
            else if (HPPercent <= 0.50f)
                newPhase = 2;

            if (newPhase != _currentPhase)
            {
                _currentPhase = newPhase;
                OnPhaseChanged?.Invoke(_currentPhase);
                Debug.Log($"[Boss] Phase changed to {_currentPhase}!");
                
                if (_animator != null)
                {
                    _animator.SetInteger("Phase", _currentPhase);
                }
            }
        }

        private void Die()
        {
            Debug.Log($"[Boss] {_bossName} defeated!");
            
            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            OnBossDefeated?.Invoke();
        }

        private System.Collections.IEnumerator FlashDamage()
        {
            if (_spriteRenderer == null) yield break;

            Color originalColor = _spriteRenderer.color;
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = originalColor;
        }

        #region Attack Methods (Override hoặc gọi từ Animation Events)

        /// <summary>
        /// Boss tấn công cơ bản
        /// </summary>
        public virtual void Attack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }
            Debug.Log($"[Boss] {_bossName} attacks!");
        }

        /// <summary>
        /// Boss sử dụng skill đặc biệt
        /// </summary>
        public virtual void SpecialAttack(int skillIndex = 0)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Special");
            }
            Debug.Log($"[Boss] {_bossName} uses special attack {skillIndex}!");
        }

        #endregion
    }
}

