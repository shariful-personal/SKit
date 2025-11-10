#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class DebugBtn : Attribute
{
    public string Label { get; private set; }

    public DebugBtn(string label)
    {
        Label = label;
    }
}

[CustomEditor(typeof(MonoBehaviour), true)]
public class DebugBtnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetObject = target;
        var methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<DebugBtn>();
            if (attr == null) continue;

            string label = string.IsNullOrEmpty(attr.Label) ? method.Name : attr.Label;

            using (new EditorGUI.DisabledScope(method.GetParameters().Length > 0))
            {
                if (GUILayout.Button(label))
                {
                    method.Invoke(targetObject, null);
                }
            }
        }
    }
}
#endif