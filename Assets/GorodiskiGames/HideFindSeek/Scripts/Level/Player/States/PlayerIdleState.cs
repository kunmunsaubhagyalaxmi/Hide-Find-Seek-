using Game.Controls;

namespace Game.Player.States
{
    public sealed class PlayerIdleSeekerState : PlayerIdleState
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void OnTICK()
        {
            base.OnTICK();

            LookForUnit(_player.View);
        }

        public override void Walk()
        {
            _player.WalkSeeker();
        }
    }

    public class PlayerIdleState : PlayerState
    {
        private Joystick _joystick;

        public override void Initialize()
        {
            _joystick = _view.Joystick;

            _player.View.Idle();

            _timer.TICK += OnTICK;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTICK;
        }

        public virtual void OnTICK()
        {
            if (!_joystick.IsTouched)
                return;

            Walk();
        }

        public virtual void Walk()
        {
            _player.Walk();
        }
    }
}