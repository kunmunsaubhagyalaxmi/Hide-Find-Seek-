using System;
using System.Collections.Generic;
using Game.Config;
using Game.Core;
using Game.Level;
using Game.Managers;
using Game.Modules;
using Game.Player;
using Game.UI;
using Game.UI.Hud;
using Game.Unit;
using Injection;
using UnityEngine;

namespace Game.States
{
    public abstract class ModulesState : GameState
    {
        public readonly List<Module> _levelModules;
        [Inject] protected Injector _injector;

        protected ModulesState()
        {
            _levelModules = new List<Module>();
        }

        protected void AddModule<T>(params object[] args) where T : Module
        {
            var result = (T)Activator.CreateInstance(typeof(T), args);
            AddModule(result);
        }

        protected void AddModule(Module result)
        {
            _levelModules.Add(result);
            _injector.Inject(result);
            result.Initialize();
        }

        protected void AddModule<T, T1>(Component component) where T : Module
        {
            var view = component.GetComponent<T1>();
            var result = (T)Activator.CreateInstance(typeof(T), new object[] { view });
            _levelModules.Add(result);
            _injector.Inject(result);
            result.Initialize();
        }

        public void DisposeLevelModules()
        {
            foreach (var levelModule in _levelModules)
            {
                levelModule.Dispose();
            }
            _levelModules.Clear();
        }
    }

    public abstract class BaseGamePlayState : ModulesState
    {
        [Inject] protected GameManager _gameManager;
        [Inject] protected LevelView _levelView;
        [Inject] protected GameView _gameView;
        [Inject] protected Timer _timer;
        [Inject] protected GameStateManager _gameStateManager;
        [Inject] protected GameConfig _config;
        [Inject] protected Context _context;
        [Inject] protected HudManager _hudManager;
        [Inject] protected ResourcesManager _resourcesManager;
        [Inject] protected AdsManager _adsManager;

        private GameObject _fieldOfView;

        public override void Initialize()
        {
            var player = new PlayerController(_gameView.PlayerView, _context);
            _gameManager.Player = player;
        }

        public override void Dispose()
        {
            _gameView.Joystick.SetEnabled(false);
            _gameView.Joystick.JoystickVisibility(_gameManager.Model.JoystickVisibility);

            _gameView.CameraController.SetTarget(null);
            _gameView.CameraController.SetEnabled(false);

            DisposeLevelModules();
            HideHuds();

            _gameManager.Player.Dispose();
            _gameManager.Player = null;

            GameObject.Destroy(_fieldOfView);

            _gameManager.Dispose();
            _context.Uninstall(_gameManager);
        }

        public void SetCameraJoystick(bool isEnable, Transform target, GameParam cameraZoom)
        {
            _gameView.Joystick.SetEnabled(isEnable);

            _gameView.CameraController.ZoomTo(_config.GetValue(cameraZoom), _config.CameraZoomDuration);
            _gameView.CameraController.SetTarget(target);
            _gameView.CameraController.SetEnabled(isEnable);
        }

        public virtual void InitLevelModules()
        {
            AddModule<UnitsModule>();
            AddModule<CashModule>();
        }

        public void ShowHuds()
        {
            _hudManager.ShowAdditional<GamePlayHudMediator>();
        }

        private void HideHuds()
        {
            _hudManager.HideAdditional<GamePlayHudMediator>();
        }

        public void OnGameOver()
        {
            _gameManager.IsGameOver = true;

            SetCameraJoystick(false, null, GameParam.CameraMenuZoom);
            HideHuds();

            _adsManager.ShowInterstitial();
        }

        public FieldOfView FieldOfView(Transform parent)
        {
            var prefab = _resourcesManager.LoadFieldOfView();
            _fieldOfView = GameObject.Instantiate(prefab, parent);
            var result = _fieldOfView.GetComponent<FieldOfView>();
            result.Initialize(_config.SeekerRadius, _config.SeekerAngel);
            return result;
        }
    }
}