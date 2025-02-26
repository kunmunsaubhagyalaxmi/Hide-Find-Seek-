using Game.Config;
using Game.Core;
using Game.Core.UI;
using Game.Utilities;
using Injection;
using UnityEngine;
using Game.Managers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Game.UI.Hud
{
    public sealed class GamePlayHudMediator : Mediator<GamePlayHudView>
    {
        private const string _levelDurationFormat = "{0} {1:D1}:{2:D2}";
        private const string _textFormat = "{0} {1}";

        private const string _startingInWord = "STARTING IN";
        private const string _timeToHideWord = "TIME TO HIDE";

        private const float _hideActionTextDelay = 0.25f;
        private const float _levelEndDelay = 0.25f;

        [Inject] private GameConfig _config;
        [Inject] private Timer _timer;
        [Inject] private GameManager _gameManager;
        [Inject] private ResourcesManager _resourcesManager;

        private float _hideDuration;
        private float _levelDuration;

        private string _actionText;
        private string _clockIcon;

        private readonly TimerDelayer _timerDelayer;
        private readonly List<UnitMarkerView> _markers;

        public GamePlayHudMediator()
        {
            _timerDelayer = new TimerDelayer();
            _markers = new List<UnitMarkerView>();
        }

        protected override void Show()
        {
            var isSeek = _gameManager.Model.IsSeek;
            _actionText = isSeek ? _startingInWord : _timeToHideWord;
            _clockIcon = GameConstants.ClockIcon;

            ActionTextVisibility(true);

            _hideDuration = _config.GetValue(GameParam.TimeToHide);
            SetActionText(_hideDuration);

            _levelDuration = _config.LevelDuration;
            SetLevelDurationText(_levelDuration);
            SetLevelDurationTextColor(Color.white);

            _view.Model = _gameManager.Model;

            CreateUnitMarkers();

            _gameManager.ON_COUNTDOWN_START += OnCountdownStart;
            _gameManager.ON_PLAYER_CAUGHT += OnPlayerCaught;

            _timer.TICK += OnTick;
            _timer.ONE_SECOND_TICK += OnSecontTick;
        }

        protected override void Hide()
        {
            _gameManager.ON_COUNTDOWN_START -= OnCountdownStart;
            _gameManager.ON_PLAYER_CAUGHT -= OnPlayerCaught;

            _timer.POST_TICK -= CalculateLevelDuration;
            _timer.TICK -= OnTick;
            _timer.ONE_SECOND_TICK -= OnSecontTick;

            foreach (var view in _markers.ToList())
            {
                GameObject.Destroy(view.gameObject);
            }
        }

        private void OnTick()
        {
            _timerDelayer.Tick();
        }

        private void OnSecontTick()
        {
            if(_levelDuration > 5)
                return;

            _timer.ONE_SECOND_TICK -= OnSecontTick;

            var color = _view.HideColor;
            if(_gameManager.Model.IsSeek)
                color = _view.SeekColor;

            SetLevelDurationTextColor(color);
        }

        private void CreateUnitMarkers()
        {
            var isSeek = _gameManager.Model.IsSeek;
            var prefab = _resourcesManager.LoadUnitMarker();

            foreach (var unit in _gameManager.UnitsMap.Values)
            {
                var view = GameObject.Instantiate(prefab, _view.Content).GetComponent<UnitMarkerView>();
                view.Initialize(isSeek);
                view.Model = unit.Model;

                _markers.Add(view);
            }
        }

        private void OnCountdownStart()
        {
            _gameManager.ON_COUNTDOWN_START -= OnCountdownStart;

            _timer.POST_TICK += CalculateHideDuration;
        }

        private void CalculateHideDuration()
        {
            _hideDuration -= Time.deltaTime;

            SetActionText(_hideDuration);

            if (_hideDuration > 0f)
                return;

            _gameManager.FireCountdownEnd();

            _timerDelayer.DelayAction<bool>(_hideActionTextDelay, ActionTextVisibility, false);

            _timer.POST_TICK -= CalculateHideDuration;
            _timer.POST_TICK += CalculateLevelDuration;
        }

        private void CalculateLevelDuration()
        {
            _levelDuration -= Time.deltaTime;

            SetLevelDurationText(_levelDuration);

            if(_levelDuration > 0f)
                return;

            _timer.POST_TICK -= CalculateLevelDuration;

            _timerDelayer.DelayAction(_levelEndDelay, _gameManager.FireLevelEnd);
        }

        private void SetActionText(float duration)
        {
            var rounded = Mathf.CeilToInt(duration);
            _view.CountdownText.text = string.Format(_textFormat, _actionText, rounded);
        }

        private void SetLevelDurationText(float duration)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
            string timeText = string.Format(_levelDurationFormat, _clockIcon, timeSpan.Minutes, timeSpan.Seconds);
            _view.LevelDurationText.text = timeText;
        }

        private void SetLevelDurationTextColor(Color color)
        {
            _view.LevelDurationText.color = color;
        }

        private void ActionTextVisibility(bool value)
        {
            _view.CountdownText.gameObject.SetActive(value);
        }

        private void OnPlayerCaught()
        {
            _gameManager.ON_PLAYER_CAUGHT -= OnPlayerCaught;
            _timer.POST_TICK -= CalculateLevelDuration;
        }
    }
}