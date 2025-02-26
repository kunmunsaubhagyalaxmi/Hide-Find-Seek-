namespace Game.Unit.States
{
    public sealed class UnitStopState : UnitState
    {
        public override void Initialize()
        {
            _unit.View.ResetPath();
            _unit.View.NavMeshStatus(false);
            _unit.View.Idle();
        }

        public override void Dispose()
        {

        }
    }
}