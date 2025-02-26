using System;
using Core;
using Game.Config;
using Game.Core;
using Game.Unit.States;
using Injection;
using UnityEngine;

namespace Game.Unit
{
    public sealed class UnitModel : Observable
    {
        public bool IsCaught;

        public UnitModel()
        {
            IsCaught = false;
        }
    }

    public abstract class BaseController : IDisposable
    {
        public abstract Vector3 Position { get; }
        public abstract UnitView View { get; }

        public abstract void Dispose();
        public abstract void Idle();
        public abstract void Stop();
        public abstract void IdleSeeker();

        public virtual void Caught()
        {
            View.SetCollider(false);

            _model.IsCaught = true;
            _model.SetChanged();
        }
        public virtual void Free()
        {
            View.SetCollider(true);

            _model.IsCaught = false;
            _model.SetChanged();
        }

        private readonly UnitModel _model;
        public UnitModel Model => _model;

        public BaseController()
        {
            _model = new UnitModel();
        }
    }

    public class UnitController : BaseController
    {
        private const float _seekerScale = 1.5f;

        private readonly UnitView _view;
        private readonly StateManager<UnitState> _stateManager;

        public override Vector3 Position => _view.Position;
        public override UnitView View => _view;

        public float WalkSpeed;
        public float IdleDuration;

        public UnitController(UnitView view, Context context, GameConfig config, bool isSeeker)
        {
            _view = view;

            var subContext = new Context(context);
            var injector = new Injector(subContext);

            subContext.Install(this);
            subContext.Install(injector);

            _stateManager = new StateManager<UnitState>();
            _stateManager.IsLogEnabled = false;

            injector.Inject(_stateManager);

            WalkSpeed = config.GetValue(GameParam.UnitWalkSpeed);
            IdleDuration = config.GetValue(GameParam.UnitIdleDuration);

            if(isSeeker)
            {
                IdleDuration = config.GetValue(GameParam.SeekerIdleDuration);
                View.SetScale(_seekerScale);
            }

            _view.SetOutline(null);

            if(!isSeeker)
                _view.NavMeshStatus(true);

            _view.SetNavMesh(WalkSpeed, 1000, 10000);
        }

        public override void Dispose()
        {
            _view.NavMeshStatus(false);
            _stateManager.Dispose();
        }

        public override void Idle()
        {
            _stateManager.SwitchToState(new UnitIdleState());
        }

        public override void IdleSeeker()
        {
            _stateManager.SwitchToState(new UnitIdleSeekerState());
        }

        public override void Caught()
        {
            base.Caught();
            _stateManager.SwitchToState(new UnitCaughtState());
        }

        public override void Free()
        {
            base.Free();
            _stateManager.SwitchToState(new UnitFreeState());
        }

        public void Walk(Vector3 destination)
        {
            _stateManager.SwitchToState(new UnitWalkState(destination));
        }

        public void WalkSeeker(Vector3 destination)
        {
            _stateManager.SwitchToState(new UnitWalkSeekerState(destination));
        }

        public override void Stop()
        {
            _stateManager.SwitchToState(new UnitStopState());
        }
    }
}