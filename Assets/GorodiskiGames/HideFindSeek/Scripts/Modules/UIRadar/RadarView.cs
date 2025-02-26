using UnityEngine;
using Utilities;

namespace Game.Modules
{
    public sealed class RadarView : MonoBehaviour
    {
        private const float _delta = 10f;
        private const float _topDistance = 70f;
        private const float _bottomDistance = 200f;

        [SerializeField] private RectTransform _transform;

        private float _minX, _maxX, _minY, _maxY;
        private float kAddX;
        private float kTopAddY;
        private float kBottomAddY;

        public void Initialize(float scaleFactor)
        {
            var halfSizeX = _transform.sizeDelta.x / 2f * scaleFactor;
            var halfSizeY = _transform.sizeDelta.y / 2f * scaleFactor;

            kAddX = _delta * scaleFactor;
            kTopAddY = (_topDistance + _delta) * scaleFactor;
            kBottomAddY = (_bottomDistance + _delta) * scaleFactor;

            _minX = halfSizeX + kAddX;
            _maxX = Screen.width - halfSizeX - kAddX;

            _minY = halfSizeY + kBottomAddY;
            _maxY = Screen.height - halfSizeY - kTopAddY;
        }

        public void Visibility(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Follow(Vector3 screenPoint)
        {
            if (screenPoint.z < 0f)
                screenPoint = new Vector3(-screenPoint.x, -screenPoint.y, screenPoint.z);

            var isOnScreenX = screenPoint.x > 0 && screenPoint.x < Screen.width;
            var isOnScreenY = screenPoint.y > 0 && screenPoint.y < Screen.height;
            var isOnScreen = isOnScreenX && isOnScreenY;

            Visibility(!isOnScreen);

            if (isOnScreen)
                return;

            Vector2 position;
            Vector2 screenCenter = new Vector2(Screen.width * .5f, Screen.height * .5f);

            if (!LineIntersection.FindIntersection(screenCenter, screenPoint,
                    Screen.width, 0, Screen.width, Screen.height, true, out position) &&
                !LineIntersection.FindIntersection(screenCenter, screenPoint,
                    0, 0, 0, Screen.height, true, out position) &&
                !LineIntersection.FindIntersection(screenCenter, screenPoint,
                    0, 0, Screen.width, 0, true, out position) &&
                !LineIntersection.FindIntersection(screenCenter, screenPoint,
                    0, Screen.height, Screen.width, Screen.height, true, out position))
            {
                position = screenPoint;
            }

            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            position.y = Mathf.Clamp(position.y, _minY, _maxY);

            _transform.position = position;

            Vector3 relativePos = screenPoint - _transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, -Vector3.forward);

            rotation.x = 0f;
            rotation.y = 0f;

            _transform.rotation = rotation;
        }
    }
}