using System;
using System.Collections.Generic;
using UnityEngine;

namespace SKit
{
    class Music
    {
        public AudioSource source;
        public float volume;
        public bool shouldBePlaying;
    }

    [AddComponentMenu("SKit/AudioManager")]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioList audioData;

        internal const float MinVolume = 0.1f;

        private readonly Dictionary<AudioItem, Music> musics = new();
        private readonly List<AudioSource> sounds = new();

        private bool isInitOnce = false;
        private bool isMusicOn = true, isSoundOn = true;
        private float musicVolume = 1, soundVolume = 1;
        private AudioInfo[] audioInfos;

        #region Initialization
        public override void Awake2()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            if (isInitOnce)
            {
                return;
            }

            isInitOnce = true;
            int len = Enum.GetValues(typeof(AudioItem)).Length;
            audioInfos = new AudioInfo[len];

            foreach (var audio in audioData.audioInfos)
            {
                audioInfos[(int)audio.audioItem] = audio;
            }
        }
        #endregion


        #region Public Methods
        // Play any audio by enum
        public void Play(AudioItem audioItem)
        {
            AudioInfo audioInfo = audioInfos[(int)audioItem];
            PlayAudio(audioItem, audioInfo.isMusicType, audioInfo.volume);
        }

        // Play click sound
        public void Click()
        {
            Play(AudioItem.Click);
        }

        // On or Off both music and sound
        public void SetStatus(bool isOn)
        {
            SetActivationStatus(isOn, true);
            SetActivationStatus(isOn, false);
        }

        // Set Custom Volume
        public float SetVolume(float volume, bool isMusic)
        {
            bool newStatus = volume >= MinVolume;
            volume = Math.Clamp(volume, MinVolume, 1);
            if (isMusic)
            {
                if (isMusicOn != newStatus)
                {
                    SetActivationStatus(newStatus, isMusic);
                }
                else if (newStatus)
                {
                    musicVolume = volume;
                }
            }
            else
            {
                if (isSoundOn != newStatus)
                {
                    SetActivationStatus(newStatus, isMusic);
                }
                else if (newStatus)
                {
                    soundVolume = volume;
                }
            }
            return newStatus ? volume : 0;
        }
        #endregion


        #region Private Methods
        private void PlayAudio(AudioItem audioItem, bool isMusicType, float volume = 1f)
        {
            if (isMusicType)
            {
                Music element = GetMusicToPlay(audioItem);
                element.shouldBePlaying = true;
                element.volume = volume;
                if (isMusicOn && !element.source.isPlaying)
                {
                    PlayMusicTypeAudio(audioItem);
                }
            }
            else
            {
                if (isSoundOn)
                {
                    AudioSource source = GetSoundToPlay();
                    source.clip = audioInfos[(int)audioItem].audioClip;
                    source.volume = soundVolume * volume;
                    source.Play();
                }
            }
        }

        private void SetActivationStatus(bool isOn, bool isMusic)
        {
            if (isMusic)
            {
                isMusicOn = isOn;
                foreach (var pair in musics)
                {
                    if (!pair.Value.shouldBePlaying || pair.Value.source.isPlaying == isOn)
                    {
                        continue;
                    }
                    if (isOn)
                    {
                        PlayMusicTypeAudio(pair.Key);
                    }
                    else
                    {
                        StopMusicTypeAudio(pair.Key, true);
                    }
                }
            }
            else
            {
                isSoundOn = isOn;
                if (!isOn)
                {
                    foreach (var source in sounds)
                    {
                        source.Stop();
                    }
                }
            }
        }

        private void PlayMusicTypeAudio(AudioItem audio)
        {
            Music element = musics[audio];
            element.source.volume = musicVolume * element.volume;
            element.source.Play();
        }

        public void StopMusicTypeAudio(AudioItem audio, bool byForce = false)
        {
            if (!musics.ContainsKey(audio))
            {
                return;
            }
            if (!byForce)
            {
                musics[audio].shouldBePlaying = false;
            }
            musics[audio].source.Stop();
        }

        private Music GetMusicToPlay(AudioItem audioItem)
        {
            if (!musics.ContainsKey(audioItem))
            {
                AudioSource newSource = CreateAudioSource();
                newSource.loop = true;
                newSource.clip = audioInfos[(int)audioItem].audioClip;
                Music element = new()
                {
                    source = newSource
                };
                musics[audioItem] = element;
            }
            return musics[audioItem];
        }

        private AudioSource GetSoundToPlay()
        {
            foreach (var source in sounds)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            AudioSource audioSource = CreateAudioSource();
            audioSource.loop = false;
            sounds.Add(audioSource);
            return audioSource;
        }

        private AudioSource CreateAudioSource()
        {
            GameObject obj = new($"AudioSourceObj-{transform.childCount.ToString().PadLeft(2)}", typeof(AudioSource));
            obj = Instantiate(obj, transform);
            obj.transform.SetParent(transform);
            return obj.GetComponent<AudioSource>();
        }
        #endregion
    }
}
