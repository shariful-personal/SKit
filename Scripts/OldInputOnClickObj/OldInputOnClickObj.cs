using UnityEngine;

namespace SKit 
{
    public class OldInputOnClickObj : MonoBehaviour
    {
        public bool isClickEnable = true;

        private void Update()
        {
            if (!isClickEnable)
            {
                return;
            }
    #if UNITY_EDITOR
            HandleMouseInput();
    #else
            HandleTouchInput();
    #endif
        }
        
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                BottleSelectAndHighlight();
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    BottleSelectAndHighlight();
                }
            }
        }

        private void BottleSelectAndHighlight()
        {
            Vector3 touchPosition = 
    #if UNITY_EDITOR
                Input.mousePosition;
    #else
                Input.GetTouch(0).position;
    #endif

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Clicked on: " + hit.transform.name);
            }
        }
    }
}