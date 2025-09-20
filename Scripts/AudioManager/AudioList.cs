using System;
using UnityEngine;

namespace SKit
{
    [CreateAssetMenu(fileName = "RefAudioList", menuName = "SKit/AudioList")]
    public class AudioList : ScriptableObject
    {
        public AudioInfo[] audioInfos = null;

        public void CheckValidation()
        {
            if (audioInfos == null || audioInfos.Length == 0)
            {
                Debug.Log("No audio added");
                return;
            }

            Array audioItemEnumValues = Enum.GetValues(typeof(AudioItem));
            if (audioInfos.Length > audioItemEnumValues.Length)
            {
                Debug.LogWarning("Number of registered AudioItems should not be more than the number of the total AudioItems");
                return;
            }

            bool[] registeredEnumStatuses = new bool[audioItemEnumValues.Length];
            for (int i = 0; i < registeredEnumStatuses.Length; i++)
            {
                registeredEnumStatuses[i] = false;
            }

            int count = 0;
            foreach (var audio in audioInfos)
            {
                if ((int)audio.audioItem > audioItemEnumValues.Length)
                {
                    Debug.LogWarning($"Enum value is greater than it should be for {audio.audioItem} (where it should be in range 0-{audioItemEnumValues.Length - 1})");
                    return;
                }

                if (registeredEnumStatuses[(int)audio.audioItem])
                {
                    Debug.Log($"Multiple audios are registered with same enum ({audio.audioItem})");
                    return;
                }
                registeredEnumStatuses[(int)audio.audioItem] = true;
                count++;
            }
            Debug.Log($"Number of registered enum : {count}\nNumber of unregistered free enum :  {audioItemEnumValues.Length - count}");
        }
    }

    [Serializable]
    public class AudioInfo
    {
        public AudioItem audioItem;
        public AudioClip audioClip;
        public bool isMusicType;
        [Range(AudioManager.MinVolume, 1f)] public float volume = 1f;
    }
}
