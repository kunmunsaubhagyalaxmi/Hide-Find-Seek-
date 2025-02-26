﻿using Game.Core;
using Game.Managers;
using Game.States;
using Injection;
using UnityEngine;

namespace Game
{
    public sealed class GameStartBehaviour : MonoBehaviour
    {
        private Timer _timer;
        public Context Context { get; private set; }

        private void Start()
        {
            _timer = new Timer();

            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Application.runInBackground = true;

            var context = new Context();

            context.Install(
                new Injector(context),
                new GameStateManager(),
                new HudManager(),
                new ResourcesManager(),
                new AdsManager(),
                new iOSAuthorizationTrackingManager(),
                new IAPManager(),
                new VibrateManager()
            );

            context.Install(GetComponents<Component>());
            context.Install(_timer);
            context.ApplyInstall();

            context.Get<GameStateManager>().SwitchToState(typeof(GameInitializeState));

            Context = context;
        }

        public void Reload()
        {
            Context.Get<GameStateManager>().Dispose();
            Context.Dispose();
            Start();
        }

        private void Update()
        {
            _timer.Update();
        }

        private void LateUpdate()
        {
            _timer.LateUpdate();
        }

        private void FixedUpdate()
        {
            _timer.FixedUpdate();
        }
    }
}