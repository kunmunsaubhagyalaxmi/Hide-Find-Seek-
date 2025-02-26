namespace Game.Unit.States
{
    public sealed class UnitCaughtState : UnitState
    {
        public override void Initialize()
        {
            _unit.View.ResetPath();
            _unit.View.NavMeshStatus(false);
            _unit.View.Idle();
            _unit.View.Visibility(true);
        }

        public override void Dispose()
        {

        }
    }
}