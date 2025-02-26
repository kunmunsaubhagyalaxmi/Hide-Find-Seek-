using Game.Core;
using Game.Level;
using Game.Managers;
using Injection;
using UnityEngine;

namespace Game.Modules
{
    public sealed class UIRadarModule : Module<UIRadarModuleView>
    {
        private const float _offset = 1f;

        [Inject] private Timer _timer;
        [Inject] private GameManager _gameManager;
        [Inject] private GameView _gameView;

        public UIRadarModule(UIRadarModuleView view) : base(view)
        {
        }

        public override void Initialize()
        {
            _view.SeekerRadar.Visibility(false);

            _gameManager.ON_COUNTDOWN_END += OnCountDownEnd;
        }

        public override void Dispose()
        {
            _gameManager.ON_COUNTDOWN_END += OnCountDownEnd;
            _timer.TICK -= OnTICK;

            _view.SeekerRadar.Visibility(false);
        }

        private void OnCountDownEnd()
        {
            _gameManager.ON_COUNTDOWN_END -= OnCountDownEnd;

            var scaleFactor = _gameView.Canvas.scaleFactor;

            _view.SeekerRadar.Initialize(scaleFactor);
            _view.SeekerRadar.Visibility(true);

            _timer.TICK += OnTICK;
        }

        private void OnTICK()
        {
            var input = _gameManager.Seeker.View.Position + Vector3.up * _offset;
            var result = _gameView.CameraController.Camera.WorldToScreenPoint(input);

            _view.SeekerRadar.Follow(result);
        }
    }
}