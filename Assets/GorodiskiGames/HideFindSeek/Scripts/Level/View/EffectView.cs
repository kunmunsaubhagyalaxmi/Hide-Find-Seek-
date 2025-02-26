using UnityEngine;

namespace Game.Level.Effect
{
    public sealed class EffectView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public float Duration
        {
            get
            {
                var main = _particleSystem.main;
                return main.duration;
            }
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 Scale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }

        public void Play()
        {
            if (_particleSystem.isPlaying)
                return;

            _particleSystem.Play();
        }
    }
}

