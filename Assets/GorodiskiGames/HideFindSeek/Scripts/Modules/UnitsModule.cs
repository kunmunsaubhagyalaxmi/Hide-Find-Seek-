using Game.Level;
using Injection;
using Game.Unit;
using Game.Core;
using Game.Config;
using System.Linq;
using UnityEngine;
using Game.Managers;

namespace Game.Modules
{
    public sealed class UnitsModule : Module
    {
        [Inject] private GameManager _gameManager;
        [Inject] private Context _context;
        [Inject] private GameView _gameView;
        [Inject] private Timer _timer;
        [Inject] private GameConfig _config;
        [Inject] private LevelView _levelView;
        [Inject] private VibrateManager _vibrateManager;

        public override void Initialize()
        {
            SetUnits();

            _gameManager.ON_COUNTDOWN_START += OnCountdownStart;
            _gameManager.ON_COUNTDOWN_END += OnCountDownEnd;
            _gameManager.ON_LEVEL_END += OnGameOver;
            _gameManager.ON_PLAYER_CAUGHT += OnGameOver;
            _gameManager.ON_TRACK_UNITS_FREE += OnTrackUnitsFree;
            _gameManager.ON_HIDE_UNITS += OnHideUnits;
        }

        public override void Dispose()
        {
            _gameManager.ON_COUNTDOWN_START -= OnCountdownStart;
            _gameManager.ON_COUNTDOWN_END -= OnCountDownEnd;
            _gameManager.ON_LEVEL_END -= OnGameOver;
            _gameManager.ON_PLAYER_CAUGHT -= OnGameOver;
            _gameManager.ON_TRACK_UNITS_FREE -= OnTrackUnitsFree;
            _gameManager.ON_HIDE_UNITS -= OnHideUnits;

            _timer.TICK -= TrackUnitsFree;

            foreach (var unit in _gameManager.UnitsMap.Values)
            {
                unit.Dispose();
            }
            _gameManager.UnitsMap.Clear();

            foreach (var unit in _gameManager.CaughtUnitsMap.Keys)
            {
                unit.Dispose();
                ReleaseCage(unit);
            }
            _gameManager.CaughtUnitsMap.Clear();
        }

        private void SetUnits()
        {
            foreach (var view in _levelView.Units)
            {
                var unit = new UnitController(view, _context, _config, false);

                unit.View.SetCollider(true);
                unit.View.SetLayer(GameConstants.UnitLayer);

                _gameManager.UnitsMap.Add(view, unit);
            }
        }

        private void OnCountDownEnd()
        {
            _gameManager.ON_COUNTDOWN_END -= OnCountDownEnd;

            foreach(var view in _gameManager.UnitsMap.Keys)
            {
                view.ON_CAUGHT += OnUnitCaught;
            }
        }

        private void OnTrackUnitsFree()
        {
            _gameManager.ON_TRACK_UNITS_FREE -= OnTrackUnitsFree;

            _timer.TICK += TrackUnitsFree;
        }

        private void OnHideUnits()
        {
            _gameManager.ON_HIDE_UNITS -= OnHideUnits;

            foreach (var view in _gameManager.UnitsMap.Keys)
            {
                view.Visibility(false);
            }
        }

        private void OnCountdownStart()
        {
            _gameManager.ON_COUNTDOWN_START -= OnCountdownStart;
            foreach (var unit in _gameManager.UnitsMap.Values)
            {
                unit.Idle();
            }
        }

        private void TrackUnitsFree()
        {
            foreach (var unit in _gameManager.CaughtUnitsMap.Keys.ToList())
            {
                if(unit == _gameManager.Player)
                    continue;

                var distance = Vector3.Distance(_gameManager.Player.Position, unit.Position);
                if (distance < 1.25f)
                    UnitFree(unit);
            }
        }

        private void OnUnitCaught(UnitView view)
        {
            view.ON_CAUGHT -= OnUnitCaught;

            var unit = _gameManager.UnitsMap[view];
            unit.Caught();

            var cage = _gameView.CagesPool.Get<Transform>();
            cage.position = unit.Position;

            _gameManager.UnitsMap.Remove(view);
            _gameManager.CaughtUnitsMap.Add(unit, cage);

            _vibrateManager.Vibrate();

            if(_gameManager.UnitsMap.Count <= 0)
                _gameManager.FireAllUnitsCaught();
        }

        private void UnitFree(BaseController unit)
        {
            unit.Free();

            ReleaseCage(unit);

            _gameManager.UnitsMap.Add(unit.View, unit);
            _gameManager.CaughtUnitsMap.Remove(unit);

            _vibrateManager.Vibrate();

            unit.View.ON_CAUGHT += OnUnitCaught;
        }

        private void ReleaseCage(BaseController unit)
        {
            var cage = _gameManager.CaughtUnitsMap[unit];
            _gameView.CagesPool.Release(cage);
        }

        private void OnGameOver()
        {
            _timer.TICK -= TrackUnitsFree;

            foreach (var view in _gameManager.UnitsMap.Keys)
            {
                view.ON_CAUGHT -= OnUnitCaught;
                _gameManager.UnitsMap[view].Stop();
            }
        }
    }
}