using DG.Tweening;
using UnityEngine;

namespace Game.Cash
{
    public sealed class CashView : MonoBehaviour
    {
        private const float _shakeScaleDuration = .5f;
        private const float _hideScaleDuration = .2f;

        [SerializeField] private int _amount = 1;

        public int Amount => _amount;

        public void Collect()
        {
            transform.DOShakeScale(_shakeScaleDuration).SetId(this);
            transform.DOScale(Vector3.zero, _hideScaleDuration).SetDelay(_shakeScaleDuration).SetId(this).OnComplete(OnComplete);
        }

        private void Update()
        {
            transform.Rotate(Vector3.one);
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        private void OnComplete()
        {
            DOTween.Kill(this);
        }
    }
}