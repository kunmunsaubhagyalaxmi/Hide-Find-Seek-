using System;
using System.Collections.Generic;
using Game.Config;
using Game.Unit;
using Game.Player;
using UnityEngine;

namespace Game.Managers
{
    public sealed class GameManager : IDisposable
    {
        public Action ON_COUNTDOWN_START;
        public Action ON_COUNTDOWN_END;
        public Action ON_PLAYER_CAUGHT;
        public Action ON_LEVEL_END;
        public Action ON_ALL_UNITS_CAUGHT;
        public Action ON_TRACK_UNITS_FREE;
        public Action ON_HIDE_UNITS;

        public PlayerController Player;
        public BaseController Seeker;
        public readonly Dictionary<UnitView, BaseController> UnitsMap;
        public readonly Dictionary<BaseController, Transform> CaughtUnitsMap;

        public readonly GameModel Model;
        public bool IsGameOver;

        public GameManager(GameConfig config)
        {
            Model = GameModel.Load(config);
            UnitsMap = new Dictionary<UnitView, BaseController>();
            CaughtUnitsMap = new Dictionary<BaseController, Transform>();

            IsGameOver = false;
        }

        public void Dispose()
        {

        }

        public void FireCountdownStart()
        {
            ON_COUNTDOWN_START?.Invoke();
        }

        public void FireCountdownEnd()
        {
            ON_COUNTDOWN_END?.Invoke();
        }

        public void FirePlayerCaught()
        {
            ON_PLAYER_CAUGHT?.Invoke();
        }

        public void FireLevelEnd()
        {
            ON_LEVEL_END?.Invoke();
        }

        public void FireAllUnitsCaught()
        {
            ON_ALL_UNITS_CAUGHT?.Invoke();
        }

        public void FireHideUnits()
        {
            ON_HIDE_UNITS?.Invoke();
        }

        public void FireTrackUnitsFree()
        {
            ON_TRACK_UNITS_FREE?.Invoke();
        }
    }
}