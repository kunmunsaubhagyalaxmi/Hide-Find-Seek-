using UnityEngine;
using DG.Tweening;

namespace Game
{
    public sealed class CameraController : MonoBehaviour
    {
        private const float _zoomDuration = 1f;

        private const int _shakeVibrato = 20;
        private const float _shakeDuration = 0.3f;
        private const float _shakeStrength = 0.1f;

        [SerializeField] private Camera _camera;
        [SerializeField] private float _distance;
        [SerializeField] private float _sensitivity;

        public Camera Camera => _camera;

        private Transform _target;
        private bool _isShaking;

        private Vector3 _initPosition;

        private void Awake()
        {
            _initPosition = transform.position;
        }

        private void Update()
        {
            Vector3 position = _target.position + _target.forward * _distance;
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * _sensitivity);
        }

        public void Shake()
        {
            if (_isShaking) return;
            _isShaking = true;

            Vector3 strengthVector = new Vector3(_shakeStrength, _shakeStrength, _shakeStrength);
            _camera.DOShakePosition(_shakeDuration, strengthVector, _shakeVibrato).OnComplete(OnComplete);
        }

        public void ResetPosition()
        {
            transform.position = _initPosition;
        }

        public void SetEnabled(bool value)
        {
            enabled = value;
        }

        private void OnComplete()
        {
            _isShaking = false;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ZoomTo(float zoom, float duration)
        {
            DOTween.Kill(this);
            _camera.DOFieldOfView(zoom, duration).SetEase(Ease.OutCirc).SetId(this);
        }
    }
}