using System.Collections.Generic;
using System.Linq;
using Game.Cash;
using Game.Core;
using Game.Level;
using Game.Level.Effect;
using Game.Managers;
using Injection;
using UnityEngine;

namespace Game.Modules
{
    public sealed class CashModule : Module
    {
        private const float _cashRadius = 1f;

        [Inject] private Timer _timer;
        [Inject] private LevelView _levelView;
        [Inject] private GameManager _gameManager;
        [Inject] private GameView _gameView;
        [Inject] private VibrateManager _vibrateManager;

        private readonly List<CashView> _cashes;
        private Dictionary<EffectView, float> _effectMap;

        public CashModule()
        {
            _cashes = new List<CashView>();
            _effectMap = new Dictionary<EffectView, float>();
        }

        public override void Initialize()
        {
            foreach(var cash in _levelView.Cashes)
            {
                _cashes.Add(cash);
            }

            _timer.TICK += OnTick;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTick;
        }

        private void OnTick()
        {
            foreach (var cash in _cashes.ToList())
            {
                var cashPosition = cash.Position;
                var playerPosition = _gameManager.Player.Position;
                playerPosition.y = cashPosition.y;

                var distance = Vector3.Distance(playerPosition, cashPosition);
                if(distance > _cashRadius)
                    continue;

                _cashes.Remove(cash);

                var amount = cash.Amount;

                _gameManager.Model.Cash += amount;
                _gameManager.Model.Save();
                _gameManager.Model.SetChanged();

                cash.Collect();

                _vibrateManager.Vibrate();

                var effect = _gameView.SparksEffectPool.Get<EffectView>();
                effect.Position = cashPosition;

                _effectMap.Add(effect, effect.Duration);
            }

            foreach (var effect in _effectMap.Keys.ToList())
            {
                _effectMap[effect] -= Time.deltaTime;

                var duration = _effectMap[effect];
                if(duration > 0f)
                    continue;

                _effectMap.Remove(effect);
                _gameView.SparksEffectPool.Release(effect);
            }
        }
    }
}

