using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.UI.Scripts.Render_Image_Window
{
    /// <summary>
    /// A UI class that handles zooming and panning for an image. Adapted from
    /// https://gist.github.com/unity3dcollege/f971cee86b6eb09ad4dafc49050f693c.
    /// </summary>
    public class UIZoomImage : MonoBehaviour, IScrollHandler
    {
        private Vector3 initialScale;

        [SerializeField]
        private float zoomSpeed = 0.1f;
        [SerializeField]
        private float maxZoom = 10f;

        private void Awake()
        {
            initialScale = transform.localScale;
        }

        void Update()
        {
            // Zooming for mobile
            if (Input.touchCount == 2 && (
                (Input.touches[0].deltaPosition.y > 0 && Input.touches[1].deltaPosition.y < 0) ||
                (Input.touches[0].deltaPosition.y < 0 && Input.touches[1].deltaPosition.y > 0) ||
                (Input.touches[0].deltaPosition.x > 0 && Input.touches[1].deltaPosition.x < 0) ||
                (Input.touches[0].deltaPosition.x < 0 && Input.touches[1].deltaPosition.x > 0)))
            {
                float xTouch1;
                float xTouch2;
                float yTouch1;
                float yTouch2;
                if (Input.touches[0].position.y > Input.touches[1].position.y)
                {
                    yTouch1 = Input.touches[0].deltaPosition.y;
                    yTouch2 = Input.touches[1].deltaPosition.y;
                }
                else
                {
                    yTouch1 = Input.touches[1].deltaPosition.y;
                    yTouch2 = Input.touches[0].deltaPosition.y;
                }
                if (Input.touches[0].position.x > Input.touches[1].position.x)
                {
                    xTouch1 = Input.touches[0].deltaPosition.x;
                    xTouch2 = Input.touches[1].deltaPosition.x;
                }
                else
                {
                    xTouch1 = Input.touches[1].deltaPosition.x;
                    xTouch2 = Input.touches[0].deltaPosition.x;
                }

                var delta = Vector3.one * (((yTouch1 - yTouch2) + (xTouch1 - xTouch2)) / 1.5f) * zoomSpeed * 0.08f;
                var desiredScale = transform.localScale + delta;

                desiredScale = ClampDesiredScale(desiredScale);

                transform.localScale = desiredScale;
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
            var desiredScale = transform.localScale + delta;

            desiredScale = ClampDesiredScale(desiredScale);

            // Zoom where the cursor is.
            var move = ((Vector3) eventData.position - transform.position) *
                       ((desiredScale - transform.localScale).magnitude / transform.localScale.magnitude);
            transform.position = eventData.scrollDelta.y > 0 ? transform.position - move : transform.position + move;
            
            transform.localScale = desiredScale;
            
        }

        public void ResetZoom()
        {
            transform.localScale = initialScale;
        }

        private Vector3 ClampDesiredScale(Vector3 desiredScale)
        {
            desiredScale = Vector3.Max(initialScale, desiredScale);
            desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
            return desiredScale;
        }
    }
}
