#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SKit.Editor
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "SKit/SceneList")]
    public class SceneList : ScriptableObject
    {
        [SerializeField] private SceneAsset[] scenes = null;
        public SceneAsset[] Scenes => scenes;
    }
}
#endif
