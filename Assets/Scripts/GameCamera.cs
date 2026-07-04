using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class GameCamera : MonoBehaviour
    {
        [SerializeField] private Transform backgroundTransform;
        [SerializeField] private RectTransform scoresRectTransform;

        private Camera mainCamera;

        private Rect viewFrameRect;
        private Rect viewRect;

        private float hBoard;

        private void Awake()
        {
            Assert.IsNotNull(backgroundTransform);
            Assert.IsNotNull(scoresRectTransform);

            mainCamera = gameObject.GetComponent<Camera>();
        }

        public void ViewFrame(Rect rect)
        {
            this.viewFrameRect = rect;

            Apply();
        }

        public void View(Rect rect, float hBoard)
        {
            this.viewRect = rect;
            this.hBoard = hBoard;

            Apply();
        }

        public void Apply()
        {
            if (mainCamera == null)
                return;

            //var center = viewFrameRect.center;
            var size = viewRect.size / viewFrameRect.size;
            var height = Mathf.Max(size.x / mainCamera.aspect, size.y);
            var orthographicSize = height * 0.5f;
            mainCamera.orthographicSize = orthographicSize;

            // transform.position = new Vector3(
            //     viewRect.center.x,
            //     viewRect.center.y - (center.y - 0.5f) * height,
            //     transform.position.z
            // );

            //xử lý background theo camera
            backgroundTransform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                0.0f
            );
            var scaleFactor =
                Mathf.Max(
                    // chiều rộng camera, xử lý khi người dùng xoay ngang màn hình, màn hình to hơn với kích thước mặc định của background
                    height * mainCamera.aspect / 1080.0f,
                    //
                    height / 1920.0f
                ) * 100.0f;

            backgroundTransform.localScale =
                new Vector3(scaleFactor, scaleFactor, scaleFactor);
            //vị trí của điểm đạt được
            var screenPoint =
                mainCamera.WorldToScreenPoint(
                    new Vector3(
                        0,
                        hBoard / 2 + 1.5f,
                        0.0f
                    )
                );

            if (
                float.IsNaN(screenPoint.x) == false &&
                float.IsNaN(screenPoint.y) == false &&
                float.IsNaN(screenPoint.z) == false &&
                float.IsInfinity(screenPoint.x) == false &&
                float.IsInfinity(screenPoint.y) == false &&
                float.IsInfinity(screenPoint.z) == false
            )
            {
                Vector2 localPoint;

                if (
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        scoresRectTransform.parent.GetComponent<RectTransform>(),
                        screenPoint,
                        null,
                        out localPoint
                    )
                )
                {
                    scoresRectTransform.localPosition = localPoint;
                }
            }
        }
    }
}