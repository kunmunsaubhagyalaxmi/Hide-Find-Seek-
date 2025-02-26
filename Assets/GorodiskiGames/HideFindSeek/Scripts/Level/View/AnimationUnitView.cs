using UnityEngine;

namespace Game
{
    public enum AnimationType
    {
        Idle,
        Walk
    }

    public abstract class BaseUnitView : MonoBehaviour
    {
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return transform.localEulerAngles.y;
            }
            set
            {
                transform.localEulerAngles = new Vector3(0, value, 0);
            }
        }

        public Vector3 Euler
        {
            set
            {
                transform.eulerAngles = value;
            }
        }
    }

    public abstract class AnimationUnitView : BaseUnitView
    {
        [SerializeField] private Animator _animator;

        private AnimationType _animationType;
        public AnimationType AnimationType => _animationType;

        public virtual void Idle()
        {
            PlayAnimation(AnimationType.Idle);
        }

        public void Walk()
        {
            PlayAnimation(AnimationType.Walk);
        }

        private void PlayAnimation(AnimationType animationType)
        {
            if (_animationType == animationType)
                return;

            _animationType = animationType;
            var nameHash = Animator.StringToHash(_animationType.ToString());

            _animator.Play(nameHash);
            _animator.Update(0);
        }
    }
}