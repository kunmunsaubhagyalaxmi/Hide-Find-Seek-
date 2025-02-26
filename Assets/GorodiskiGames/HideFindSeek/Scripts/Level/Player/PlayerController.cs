using Game.Core;
using Game.Player.States;
using Game.Unit;
using Injection;
using UnityEngine;

namespace Game.Player
{
    public sealed class PlayerController : BaseController
    {
        private readonly UnitView _view;
        private readonly StateManager<PlayerState> _stateManager;

        public override UnitView View => _view;

        public override Vector3 Position => _view.Position;

        public PlayerController(UnitView view, Context context)
        {
            _view = view;

            var subContext = new Context(context);
            var injector = new Injector(subContext);

            subContext.Install(this);
            subContext.Install(injector);

            _stateManager = new StateManager<PlayerState>();
            _stateManager.IsLogEnabled = false;

            injector.Inject(_stateManager);

            _view.SetOutline(context.Get<GameView>().OutlineMaterials);
            _view.NavMeshStatus(true);
        }

        public override void Dispose()
        {
            _view.NavMeshStatus(false);
            _stateManager.Dispose();
        }

        public override void Idle()
        {
            _stateManager.SwitchToState(new PlayerIdleState());
        }

        public override void IdleSeeker()
        {
            _stateManager.SwitchToState(new PlayerIdleSeekerState());
        }

        public void Walk()
        {
            _stateManager.SwitchToState(new PlayerWalkState());
        }

        public void WalkSeeker()
        {
            _stateManager.SwitchToState(new PlayerWalkSeekerState());
        }

        public override void Caught()
        {
            base.Caught();
            _stateManager.SwitchToState(new PlayerCaughtState());
        }

        public override void Free()
        {
            base.Free();
        }

        public override void Stop()
        {

        }
    }
}