#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SKit.Editor
{
    public class SnapToGround
    {
        [MenuItem("SKit/SnapToGround &g", false, 100)]
        private static void SnapSelectedToGround()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                if (obj != null)
                {
                    if (obj.TryGetComponent<Collider>(out var collider))
                    {
                        if (Physics.Raycast(obj.transform.position, Vector3.down, out RaycastHit hit))
                        {
                            obj.transform.position = hit.point + Vector3.up * collider.bounds.extents.y;
                        }
                    }
                }
            }
        }
    }
}
#endif