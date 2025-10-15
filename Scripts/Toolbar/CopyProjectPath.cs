using UnityEditor;
using UnityEngine;

namespace SKit.Editor
{
    public class CopyProjectPath
    {
        private const string PrefKey = "IsCopyProjectPathEnabled";

        [InitializeOnLoadMethod]
        private static void Start()
        {
            CopyPathButton(EditorPrefs.GetBool(PrefKey, false));
        }

        [MenuItem("SKit/CopyProjectPath", false, 200)]
        private static void ToggleCopyPathButton()
        {
            bool newState = !EditorPrefs.GetBool(PrefKey, false);
            EditorPrefs.SetBool(PrefKey, newState);
            Menu.SetChecked("SKit/CopyProjectPath", newState);
            CopyPathButton(newState);
        }

        private static void CopyPathButton(bool isEnabled)
        {
            if (isEnabled)
            {
                ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            }
            else
            {
                ToolbarExtender.RightToolbarGUI.Remove(OnToolbarGUI);
            }
        }

        private static void OnToolbarGUI()
        {
            var tex = EditorGUIUtility.IconContent("Folder Icon").image;
            if (GUILayout.Button(new GUIContent(null, tex, "Copy Project Path to Clipboard"), "toolbarButton", GUILayout.Width(30f)))
            {
                string projectPath = Application.dataPath;
                GUIUtility.systemCopyBuffer = projectPath;
                Debug.Log($"Project path copied to clipboard: {projectPath}");
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        [MenuItem("SKit/CopyProjectPath", true)]
        private static bool ToggleCopyPathButtonValidate()
        {
            Menu.SetChecked("SKit/CopyProjectPath", EditorPrefs.GetBool(PrefKey, false));
            return true;
        }
    }
}