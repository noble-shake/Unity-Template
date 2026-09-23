using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Manager;
using RottenNoble.Cores.Resource;

namespace RottenNoble.Cores.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private readonly string Tag = $"[{nameof(SoundManager)}]";

        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Inject] private TableDataManager tableDataManager;

        private readonly Dictionary<string, AudioClip> clipCache = new();

        private void Awake()
        {
            if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
        }

        private void Start()
        {
            bgmSource.volume = tableDataManager.AppSetting.DefaultBGMVolume;
            sfxSource.volume = tableDataManager.AppSetting.DefaultSFXVolume;
        }

        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        public void StopBGM() => bgmSource.Stop();

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip);
        }

        public void SetVolume(SoundType type, float volume)
        {
            if (type == SoundType.BGM) bgmSource.volume = volume;
            else sfxSource.volume = volume;
        }

        public float GetVolume(SoundType type)
            => type == SoundType.BGM ? bgmSource.volume : sfxSource.volume;
    }
}
