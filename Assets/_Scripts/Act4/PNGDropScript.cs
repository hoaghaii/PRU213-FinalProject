using UnityEngine;

namespace Animation2D
{
    public class PNGDropScript: MonoBehaviour
    {
        public AudioSource audioSource;

        void OnParticleCollision(GameObject other)
        {
            audioSource.Play();
        }
    }
}