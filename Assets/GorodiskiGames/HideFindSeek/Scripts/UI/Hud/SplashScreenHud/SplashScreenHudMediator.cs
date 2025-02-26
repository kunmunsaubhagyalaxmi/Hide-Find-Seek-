using System;
using Game.Config;
using Game.Core;
using Game.Core.UI;
using Game.Managers;
using Injection;
using UnityEngine;

namespace Game.UI.Hud
{
    public sealed class SplashScreenHudMediator : Mediator<SplashScreenHudView>
    {
        [Inject] private GameConfig _config;
        [Inject] private Timer _timer;

        private float _duration;
        private float _elapsed;

        protected override void Show()
        {
            _duration = _config.SplashScreenDuration;

            _timer.TICK += OnTICK;
        }

        protected override void Hide()
        {
            _timer.TICK -= OnTICK;
        }

        private void OnTICK()
        {
            UpdateBar();
            _elapsed += Time.deltaTime;
            if (_elapsed >= _duration)
            {
                _timer.TICK -= OnTICK;
                InternalHide();
            }
        }

        private void UpdateBar()
        {
            float value = _elapsed / _duration;
            _view.FillBarImage.fillAmount = value;
        }
    }
}