#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SKit.Editor
{
    public class SceneList : ScriptableObject
    {
        [SerializeField] private SceneAsset[] scenes = null;
        public SceneAsset[] Scenes => scenes;
    }

    public class SceneOpener : EditorWindow
    {
        private const string path = "Assets/Resources/SceneList.asset";

        [MenuItem("SKit/Scene Opener _F1", false, 1)]
        public static void ShowWindow()
        {
            GetWindow<SceneOpener>("Scene Opener");
        }

        [MenuItem("SKit/SceneList _F2", false, 1)]
        public static void OpenRefSceneOpener()
        {
            SceneList scriptableObject = AssetDatabase.LoadAssetAtPath<SceneList>(path);

            if (scriptableObject == null)
            {
                scriptableObject = CreateInstance<SceneList>();
                AssetDatabase.CreateAsset(scriptableObject, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            Selection.activeObject = scriptableObject;
            EditorGUIUtility.PingObject(scriptableObject);
        }

        private void OnOpen(string path)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            }
        }

        private void OnAdd(string path)
        {
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }

        private void OnRemove(string path)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(path), true);
            }
        }

        private void OnGUI()
        {
            SceneList dataRef = AssetDatabase.LoadAssetAtPath<SceneList>(path);
            if (dataRef == null)
            {
                EditorGUILayout.HelpBox($"File not found at: {path}", MessageType.Warning);
                return;
            }

            if (dataRef.Scenes == null || dataRef.Scenes.Length == 0)
            {
                return;
            }

            foreach (SceneAsset sceneAsset in dataRef.Scenes)
            {
                if (sceneAsset == null)
                {
                    continue;
                }

                GUILayout.BeginHorizontal("Box");
                GUILayout.Label(sceneAsset.name, EditorStyles.boldLabel, GUILayout.MinWidth(100));
                EditorGUIUtility.labelWidth = 500;

                if (GUILayout.Button("Open", GUILayout.Width(50)))
                {
                    OnOpen(AssetDatabase.GetAssetPath(sceneAsset));
                }

                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    OnAdd(AssetDatabase.GetAssetPath(sceneAsset));
                }

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    OnRemove(AssetDatabase.GetAssetPath(sceneAsset));
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}
#endif