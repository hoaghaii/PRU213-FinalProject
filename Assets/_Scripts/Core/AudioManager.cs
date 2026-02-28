using UnityEngine;

namespace Animation2D.Core
{
    /// <summary>
    /// Quản lý âm thanh BGM và SFX đơn giản.
    /// Sử dụng: AudioManager.Instance.PlayBGM(clip);
    /// </summary>
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float MasterVolume = 1f;
        [Range(0f, 1f)] public float BGMVolume = 0.7f;
        [Range(0f, 1f)] public float SFXVolume = 1f;

        protected override void Awake()
        {
            base.Awake();
            SetupAudioSources();
        }

        private void SetupAudioSources()
        {
            if (_bgmSource == null)
            {
                var bgmObj = new GameObject("BGM Source");
                bgmObj.transform.SetParent(transform);
                _bgmSource = bgmObj.AddComponent<AudioSource>();
                _bgmSource.loop = true;
                _bgmSource.playOnAwake = false;
            }

            if (_sfxSource == null)
            {
                var sfxObj = new GameObject("SFX Source");
                sfxObj.transform.SetParent(transform);
                _sfxSource = sfxObj.AddComponent<AudioSource>();
                _sfxSource.playOnAwake = false;
            }
        }

        #region BGM

        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (clip == null) return;
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            _bgmSource.volume = BGMVolume * MasterVolume;
            _bgmSource.Play();
        }

        public void StopBGM()
        {
            _bgmSource.Stop();
        }

        public void PauseBGM()
        {
            _bgmSource.Pause();
        }

        public void ResumeBGM()
        {
            _bgmSource.UnPause();
        }

        #endregion

        #region SFX

        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip, SFXVolume * MasterVolume * volumeScale);
        }

        public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volumeScale = 1f)
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, SFXVolume * MasterVolume * volumeScale);
        }

        #endregion

        #region Volume Control

        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public void SetBGMVolume(float volume)
        {
            BGMVolume = Mathf.Clamp01(volume);
            _bgmSource.volume = BGMVolume * MasterVolume;
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
        }

        private void UpdateVolumes()
        {
            _bgmSource.volume = BGMVolume * MasterVolume;
        }

        public void StopAll()
        {
            _bgmSource.Stop();
            _sfxSource.Stop();
        }

        #endregion
    }
}

