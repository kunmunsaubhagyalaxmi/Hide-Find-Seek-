using Game.Config;
using Game.Managers;
using Game.UI.Hud;
using Injection;
using UnityEngine;

namespace Game.States
{
    public sealed class GameMenuState : ModulesState
    {
        [Inject] private GameView _gameView;
        [Inject] private GameConfig _config;
        [Inject] private HudManager _hudManager;
        [Inject] private Context _context;

        private GameManager _gameManager;

        public override void Initialize()
        {
            _gameView.PlayerView.Position = Vector3.zero;
            _gameView.PlayerView.Euler = Vector3.up * 180f;

            _gameView.CameraController.ResetPosition();
            _gameView.CameraController.ZoomTo(_config.GetValue(GameParam.CameraMenuZoom), 0);

            _gameManager = new GameManager(_config);

            _context.Install(_gameManager);
            _context.ApplyInstall();

            _hudManager.ShowAdditional<GameMenuHudMediator>();
        }
        
        public override void Dispose()
        {
            _hudManager.HideAdditional<GameMenuHudMediator>();
        }
    }
}