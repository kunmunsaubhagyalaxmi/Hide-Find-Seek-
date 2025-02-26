using Game.Config;
using Game.Core;
using Game.Managers;
using Game.Unit.States;
using Injection;

namespace Game.Unit.States
{
    public abstract class LookForUnitState : State
    {
        public void LookForUnit(UnitView view)
        {
            var target = view.FieldOfView.Target;
            if (target == null)
                return;

            var unit = target.GetComponent<UnitView>();
            if (unit == null)
                return;

            unit.FireCaught();
        }
    }
}

namespace Game.Player.States
{
    public abstract class PlayerState : LookForUnitState
    {
        [Inject] protected Timer _timer;
        [Inject] protected GameView _view;
        [Inject] protected PlayerController _player;
        [Inject] protected GameConfig _config;
        [Inject] protected GameManager _gameManager;
    }
}