using Game.Config;
using Injection;
using UnityEngine;
using Game.Managers;

namespace Game.States
{
    public sealed class GameInitializeState : GameState
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private Context _context;
        [Inject] private AdsManager _adsManager;
        [Inject] private iOSAuthorizationTrackingManager _iOSAuthorizationTrackingManager;
        [Inject] private IAPManager _IAPManager;
        [Inject] private VibrateManager _vibrateManager;

        public override void Initialize()
        {
            var config = GameConfig.Load();
            var model = GameModel.Load(config);

            _context.Install(config);
            _context.ApplyInstall();

            _iOSAuthorizationTrackingManager.Initialize();
            _IAPManager.Initialize(config);
            _adsManager.Initialize(model.IsNoAds, config);
            _vibrateManager.Initialize(model.IsVibration);

            _gameStateManager.SwitchToState(new GameLoadLevelState());
        }

        public override void Dispose()
        {
        }
    }
}