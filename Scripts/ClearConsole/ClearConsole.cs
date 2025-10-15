#if UNITY_EDITOR
using System;
using UnityEditor;

namespace SKit.Editor
{
    public class ClearConsole
    {
        [MenuItem("SKit/ClearConsole _F5", false, 100)]
        public static void ClearLog()
        {
            Type logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor");
            logEntries?.GetMethod("Clear")?.Invoke(new object(), null);
        }
    }
}
#endif