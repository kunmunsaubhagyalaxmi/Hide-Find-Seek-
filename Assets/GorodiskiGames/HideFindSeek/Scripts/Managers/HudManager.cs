﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.UI;
using Game.UI;
using Game.UI.Hud;
using Injection;

namespace Game.Managers
{
    public sealed class HudManager
    {
        [Inject] private GameView _gameView;
        [Inject] private Injector _injector;

        private Mediator _openedHud;
        private readonly List<Mediator> _additionalHuds;

        public HudManager()
        {
            _additionalHuds = new List<Mediator>();
        }

        public bool IsShowed<T>()
        {
            return _additionalHuds.Any(temp => temp is T) || _openedHud is T;
        }

        public T ShowSingle<T>(params object[] args) where T : Mediator
        {
            if (null != _openedHud)
            {
                HideSingle();
            }

            _openedHud = (Mediator)Activator.CreateInstance(typeof(T), args);
            _injector.Inject(_openedHud);
            var hudType = _openedHud.ViewType;
            var hudView = _gameView.AllHuds().FirstOrDefault(temp => temp.GetType() == hudType);
            _openedHud.Mediate(hudView);
            _openedHud.InternalShow();

            return (T)_openedHud;
        }

        public void HideSingle()
        {
            if (null == _openedHud)
                return;

            _openedHud.InternalHide();
            _openedHud.Unmediate();
            _openedHud = null;
        }

        public T ShowAdditional<T>(params object[] args) where T : Mediator
        {
            var hud = (Mediator)Activator.CreateInstance(typeof(T), args);
            _injector.Inject(hud);
            var hudType = hud.ViewType;
            var hudView = _gameView.AllHuds().FirstOrDefault(temp => temp.GetType() == hudType);
            hud.Mediate(hudView);
            hud.InternalShow();

            _additionalHuds.Add(hud);
            return (T)hud;
        }

        public void HideAdditional<T>()
        {
            for (int i = _additionalHuds.Count - 1; i >= 0; i--)
            {
                var hud = _additionalHuds[i];

                if (!(hud is T))
                    continue;

                hud.InternalHide();
                hud.Unmediate();
                _additionalHuds.RemoveAt(i);
            }
        }
    }
}