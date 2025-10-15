using System;
using UnityEngine;

namespace SKit
{
    [Serializable]
    public class OfflineData
    {
        #region Data Members
        // All Data Members must be public to be serialized by JsonUtility
        #endregion

        #region Helpers
        // Add helper methods here if needed
        #endregion

        #region Instance Management
        private static OfflineData instance;
        public static OfflineData Instance
        {
            get
            {
                if (instance == null)
                {
                    Load();
                }
                return instance;
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(instance);
            PlayerPrefs.SetString("OfflineData", json);
            PlayerPrefs.Save();
            // Debug.Log("OfflineData saved" + json);
        }

        public static void Load()
        {
            if (PlayerPrefs.HasKey("OfflineData"))
            {
                string json = PlayerPrefs.GetString("OfflineData");
                instance = JsonUtility.FromJson<OfflineData>(json);
            }
            else
            {
                instance = new OfflineData();
            }
        }
        #endregion
    }
}