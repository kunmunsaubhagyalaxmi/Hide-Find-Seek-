using Game.Core;
using Injection;

namespace Game.Unit.States
{
    public abstract class UnitState : LookForUnitState
    {
        [Inject] protected UnitController _unit;
        [Inject] protected Timer _timer;
    }
}