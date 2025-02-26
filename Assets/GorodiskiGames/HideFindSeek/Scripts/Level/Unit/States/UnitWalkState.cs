using UnityEngine;

namespace Game.Unit.States
{
    public sealed class UnitWalkSeekerState : UnitWalkState
    {
        public UnitWalkSeekerState(Vector3 destination) : base(destination)
        {

        }

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

            LookForUnit(_unit.View);
        }

        public override void Idle()
        {
            _unit.IdleSeeker();
        }
    }

    public class UnitWalkState : UnitState
    {
        private Vector3 _destination;

        public UnitWalkState(Vector3 destination)
        {
            _destination = destination;
        }

        public override void Initialize()
        {
            _unit.View.SetDestination(_destination);
            _unit.View.Walk();

            _timer.TICK += OnTICK;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTICK;
        }

        public virtual void OnTICK()
        {
            var distance = Vector3.Distance(_unit.View.Position, _destination);
            if (distance < 0.05f)
            {
                Idle();
            }
        }

        public virtual void Idle()
        {
            _unit.Idle();
        }
    }
}