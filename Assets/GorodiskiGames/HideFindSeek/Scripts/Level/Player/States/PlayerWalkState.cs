using Game.Config;
using UnityEngine;
using Game.Controls;

namespace Game.Player.States
{
    public sealed class PlayerWalkSeekerState : PlayerWalkState
    {
        public override void OnTICK()
        {
            base.OnTICK();
            LookForUnit(_player.View);
        }

        public override void Idle()
        {
            _player.IdleSeeker();
        }
    }

    public class PlayerWalkState : PlayerState
    {
        private Joystick _joystick;
        private Transform _transform;

        private float _walkSpeed;
        private float _angleLerpFactor;

        public override void Initialize()
        {
            _joystick = _view.Joystick;
            _transform = _player.View.transform;

            _walkSpeed = _config.GetValue(GameParam.PlayerWalkSpeed);
            _angleLerpFactor = _config.GetValue(GameParam.AngleLerpFactor);

            _player.View.Walk();

            _timer.TICK += OnTICK;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTICK;
        }

        public virtual void OnTICK()
        {
            if (!_joystick.IsTouched)
            {
                Idle();
                return;
            }

            var joystickVector = _transform.forward * _joystick.Vertical + _transform.right * _joystick.Horizontal;

            var angle = Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;

            var deltaAngle = Mathf.Abs(Mathf.DeltaAngle(_transform.localEulerAngles.y, angle)) / 90f;
            deltaAngle = 1 - Mathf.Clamp01(deltaAngle);

            angle = Mathf.LerpAngle(_transform.localEulerAngles.y, angle,
                Time.deltaTime * _angleLerpFactor * joystickVector.sqrMagnitude);
            _transform.localEulerAngles = new Vector3(0f, angle, 0f);

            Vector3 direction = _transform.forward;
            var moveSpeed = _walkSpeed * deltaAngle * joystickVector.magnitude;

            var newPosition = _transform.position + direction.normalized * Time.deltaTime * moveSpeed;

            _transform.position = newPosition;
        }

        public virtual void Idle()
        {
            _player.Idle();
        }
    }
}