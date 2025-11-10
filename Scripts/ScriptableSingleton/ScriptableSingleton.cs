using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class AssetPath : Attribute
{
    public string Path { get; }

    public AssetPath(string filePath)
    {
        Path = filePath;
    }
}

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                CreateOrLoadInstance();
            }
            return instance;
        }
    }

    private static void CreateOrLoadInstance()
    {
        string filePath = GetResourcePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            instance = Resources.Load<T>(filePath);
        }
    
#if UNITY_EDITOR
        if (instance != null)
        {
            return;
        }
        instance = CreateInstance<T>();
        UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/Resources/{filePath}.asset");
#endif
    }

    private static string GetResourcePath()
    {
        object[] attributes = typeof(T).GetCustomAttributes(true);

        foreach (object attribute in attributes)
        {
            if (attribute is AssetPath pathAttribute)
            {
                return pathAttribute.Path;
            }
        }
        Debug.LogError($"{typeof(T)} does not have {nameof(AssetPath)}.");
        return string.Empty;
    }

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (instance != null && instance != this)
            {
                Debug.LogError($"An instance of {typeof(T)} already exist.");
                DestroyImmediate(this, true);
            }
        }
#endif
    }
}