namespace Game.Unit.States
{
    public sealed class UnitDiedState : UnitState
    {
        public override void Initialize()
        {
            _unit.View.Idle();
        }

        public override void Dispose()
        {

        }
    }
}
