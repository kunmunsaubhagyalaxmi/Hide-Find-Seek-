using Game.Config;
using Game.Unit;
using Game.UI.Hud;
using Game.Modules;

namespace Game.States
{
    public sealed class GamePlayHideState : BaseGamePlayState
    {
        private const int _seekerIndex = 0;

        public override void Initialize()
        {
            base.Initialize();

            var view = _levelView.Units[_seekerIndex];
            view.SetLabel(GameConstants.SeekerLabel);
            _gameManager.Seeker = new UnitController(view, _context, _config, true);

            _levelView.Units.Remove(view);

            _gameManager.Player.View.SetCollider(true);
            _gameManager.Player.View.SetLayer(GameConstants.UnitLayer);
            _gameManager.Player.Idle();

            _gameManager.UnitsMap.Add(_gameManager.Player.View, _gameManager.Player);

            InitLevelModules();
            ShowHuds();

            SetCameraJoystick(true, _gameManager.Player.View.transform, GameParam.CameraGameplayZoom);

            _gameView.Joystick.ON_FIRST_TOUCH += OnFirstTouch;
            _gameManager.ON_COUNTDOWN_END += OnCountdownEnd;
            _gameManager.ON_PLAYER_CAUGHT += OnPlayerCaught;
            _gameManager.ON_LEVEL_END += OnLevelEnd;
        }

        public override void Dispose()
        {
            base.Dispose();

            _gameView.Joystick.ON_FIRST_TOUCH -= OnFirstTouch;
            _gameManager.ON_COUNTDOWN_END -= OnCountdownEnd;
            _gameManager.ON_PLAYER_CAUGHT -= OnPlayerCaught;
            _gameManager.ON_LEVEL_END -= OnLevelEnd;

            _gameManager.Seeker.Dispose();
            _gameManager.Seeker = null;
        }

        private void OnCountdownEnd()
        {
            _gameManager.ON_COUNTDOWN_END -= OnCountdownEnd;

            _gameManager.Seeker.View.NavMeshStatus(true);
            var parent = _gameManager.Seeker.View.transform;
            _gameManager.Seeker.View.FieldOfView = FieldOfView(parent);
            _gameManager.Seeker.IdleSeeker();

            _gameManager.FireTrackUnitsFree();
        }

        private void OnFirstTouch()
        {
            _gameView.Joystick.ON_FIRST_TOUCH -= OnFirstTouch;
            _gameManager.FireCountdownStart();
        }

        private void OnPlayerCaught()
        {
            _gameManager.ON_PLAYER_CAUGHT -= OnPlayerCaught;

            if (_gameManager.IsGameOver)
                return;

            OnGameOver();

            _gameManager.Seeker.Stop();

            _hudManager.ShowAdditional<LoseHudMediator>(LoseType.PlayerCaught);
        }

        private void OnLevelEnd()
        {
            _gameManager.ON_LEVEL_END -= OnLevelEnd;

            if (_gameManager.IsGameOver)
                return;

            OnGameOver();

            _gameManager.Seeker.Stop();

            _hudManager.ShowAdditional<WinHudMediator>();
        }

        public override void InitLevelModules()
        {
            base.InitLevelModules();
            AddModule<UIRadarModule, UIRadarModuleView>(_gameView);
        }
    }
}