using UnityEngine;

namespace SKit
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("SKit/SafeArea")]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform panel;

        private void Awake()
        {
            panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}
