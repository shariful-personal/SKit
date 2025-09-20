#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SnapToGround : EditorWindow
{
    [MenuItem("SKit/Snap To Ground &g", false, 100)]
    private static void SnapSelectedToGround()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj != null)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    if (Physics.Raycast(obj.transform.position, Vector3.down, out RaycastHit hit))
                    {
                        obj.transform.position = hit.point + Vector3.up * (collider.bounds.extents.y);
                    }
                }
            }
        }
    }
}
#endif