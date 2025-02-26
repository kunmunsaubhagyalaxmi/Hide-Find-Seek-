using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Hud
{
    public sealed class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float _toScaleIn = .95f;
        private const float _durationIn = .05f;
        private const float _durationOut = .1f;
        private const float _amplitude = 1f;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        private void OnDisable()
        {
            rectTransform?.DOKill();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!rectTransform) return;
            PlayScaleIn();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!rectTransform) return;
            PlayScaleOut();
        }

        public void PlayScaleIn()
        {
            rectTransform?.DOKill();
            rectTransform?.DOScale(Vector3.one * _toScaleIn, _durationIn).SetEase(Ease.InOutQuad);
        }

        public void PlayScaleOut()
        {
            rectTransform?.DOKill();
            rectTransform?.DOScale(Vector3.one, _durationOut).SetEase(Ease.OutBack, _amplitude);
        }
    }
}