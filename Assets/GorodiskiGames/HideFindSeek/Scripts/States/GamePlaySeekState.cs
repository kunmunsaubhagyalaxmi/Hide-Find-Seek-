using Game.Config;
using Game.UI.Hud;
using UnityEngine;

namespace Game.States
{
    public sealed class GamePlaySeekState : BaseGamePlayState
    {
        public override void Initialize()
        {
            base.Initialize();

            _gameManager.Player.View.SetLayer(GameConstants.DefaultLayer);
            _gameManager.Player.View.SetCollider(false);

            _gameManager.Seeker = _gameManager.Player;

            InitLevelModules();
            ShowHuds();

            _gameView.CameraController.ZoomTo(_config.GetValue(GameParam.CameraSeekZoom), _config.CameraZoomDuration);

            _gameManager.FireCountdownStart();

            _gameManager.ON_COUNTDOWN_END += OnCountdownEnd;
            _gameManager.ON_LEVEL_END += OnLevelEnd;
            _gameManager.ON_ALL_UNITS_CAUGHT += OnAllUnitsCaught;
        }

        public override void Dispose()
        {
            base.Dispose();

            _gameManager.ON_COUNTDOWN_END -= OnCountdownEnd;
            _gameManager.ON_LEVEL_END -= OnLevelEnd;
            _gameManager.ON_ALL_UNITS_CAUGHT -= OnAllUnitsCaught;
        }

        private void OnCountdownEnd()
        {
            _gameManager.ON_COUNTDOWN_END -= OnCountdownEnd;

            _gameManager.FireHideUnits();

            var parent = _gameManager.Player.View.transform;
            _gameManager.Player.View.FieldOfView = FieldOfView(parent);
            _gameManager.Player.IdleSeeker();

            SetCameraJoystick(true, _gameManager.Player.View.transform, GameParam.CameraGameplayZoom);
        }

        private void OnLevelEnd()
        {
            _gameManager.ON_LEVEL_END -= OnLevelEnd;

            if(_gameManager.IsGameOver)
                return;

            OnGameOver();

            var caughtUnits = _gameManager.CaughtUnitsMap.Count;
            if(caughtUnits >= 3)
                _hudManager.ShowAdditional<WinHudMediator>();
            else
                _hudManager.ShowAdditional<LoseHudMediator>(LoseType.NotEnoughtUnits);
        }

        private void OnAllUnitsCaught()
        {
            _gameManager.ON_ALL_UNITS_CAUGHT -= OnAllUnitsCaught;

            if (_gameManager.IsGameOver)
                return;

            OnGameOver();

            _hudManager.ShowAdditional<WinHudMediator>();
        }
    }
}