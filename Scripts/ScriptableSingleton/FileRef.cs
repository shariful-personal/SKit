using UnityEngine;

[AssetPath("FileRef")]

[CreateAssetMenu(fileName = "FileRef", menuName = "SKit/FileRef", order = 1)]
public class FileRef : ScriptableSingleton<FileRef>
{
    [SerializeField] private Sprite[] itemSprites;

    public Sprite[] GetItems => itemSprites;
}
