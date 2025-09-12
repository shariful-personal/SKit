using System;
using UnityEditor;


namespace SKit.Editor
{
    public class ClearConsole
    {
        [MenuItem("SKit/Clear Console _F5", false, 100)]
        public static void ClearLog()
        {
#if UNITY_EDITOR
            Type logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor");
            logEntries?.GetMethod("Clear")?.Invoke(new object(), null);
#endif
        }

    }
}