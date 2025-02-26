namespace Game.Unit.States
{
    public sealed class UnitFreeState : UnitState
    {
        public override void Initialize()
        {
            _unit.View.NavMeshStatus(true);

            _unit.Idle();
        }

        public override void Dispose()
        {

        }
    }
}