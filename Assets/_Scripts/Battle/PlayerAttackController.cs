using UnityEngine;
using UnityEngine.Events;
using Animation2D.Core;

namespace Animation2D.Battle
{
    /// <summary>
    /// Quản lý các skill/attack của Player trong battle.
    /// Kết nối với UI và gửi damage đến Boss.
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BossController _targetBoss;

        [Header("Attack Settings")]
        [SerializeField] private float _codeBulletDamage = 10f;
        [SerializeField] private float _assetThrowDamage = 15f;
        [SerializeField] private float _mergeSlashDamage = 30f;
        [SerializeField] private float _particleShockwaveDamage = 20f;
        [SerializeField] private float _finalCommitDamage = 25f; // Per command

        // Events
        public UnityEvent<string> OnAttackUsed = new UnityEvent<string>();
        public UnityEvent<float> OnDamageDealt = new UnityEvent<float>();

        /// <summary>
        /// ACT 3: Code Bullet - Gõ code và bắn đạn
        /// </summary>
        public void UseCodeBullet()
        {
            DealDamage(_codeBulletDamage, "CodeBullet");
            Debug.Log("[Player] if (conflict) Resolve(); → Bắn đạn code!");
        }

        /// <summary>
        /// ACT 4: Asset Throw - Ném asset PNG
        /// </summary>
        public void UseAssetThrow()
        {
            DealDamage(_assetThrowDamage, "AssetThrow");
            Debug.Log("[Player] Ném conflict_slayer.png → Pixel explosion!");
        }

        /// <summary>
        /// ACT 5: Merge Slash - Resolve conflict
        /// </summary>
        public void UseMergeSlash()
        {
            DealDamage(_mergeSlashDamage, "MergeSlash");
            Debug.Log("[Player] Keep Theirs → Slash ánh sáng!");
        }

        /// <summary>
        /// ACT 6: Particle Shockwave
        /// </summary>
        public void UseParticleShockwave()
        {
            DealDamage(_particleShockwaveDamage, "ParticleShockwave");
            Debug.Log("[Player] Particle System → Shockwave + sét!");
        }

        /// <summary>
        /// ACT 7: Final Commit - git add, commit, push
        /// </summary>
        public void UseFinalCommit()
        {
            // 3 lệnh git, mỗi lệnh gây damage
            StartCoroutine(FinalCommitSequence());
        }

        private System.Collections.IEnumerator FinalCommitSequence()
        {
            Debug.Log("[Player] git add .");
            DealDamage(_finalCommitDamage, "GitAdd");
            yield return new WaitForSeconds(0.5f);

            Debug.Log("[Player] git commit -m \"final fix\"");
            DealDamage(_finalCommitDamage, "GitCommit");
            yield return new WaitForSeconds(0.5f);

            Debug.Log("[Player] git push origin main");
            DealDamage(_finalCommitDamage, "GitPush");
        }

        /// <summary>
        /// Gây damage cho Boss
        /// </summary>
        public void DealDamage(float damage, string attackName)
        {
            if (_targetBoss != null && _targetBoss.IsAlive)
            {
                _targetBoss.TakeDamage(damage);
                OnDamageDealt?.Invoke(damage);
                OnAttackUsed?.Invoke(attackName);
            }
        }

        /// <summary>
        /// Set target boss (runtime)
        /// </summary>
        public void SetTargetBoss(BossController boss)
        {
            _targetBoss = boss;
        }
    }
}

