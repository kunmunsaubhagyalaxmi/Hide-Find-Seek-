using System;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;
using Utilities;

namespace Game.Config
{
    public enum GameParam
    {
        PlayerWalkSpeed,
        AngleLerpFactor,
        UnitWalkSpeed,
        TimeToHide,
        CoinRadius,
        CameraGameplayZoom,
        CameraMenuZoom,
        CameraSeekZoom,
        CoinsByRescue,
        AudibilityRadius,
        MinDistanceToEnemy,
        RescueReloadDuration,
        SeekerIdleDuration,
        UnitIdleDuration
    }

    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public const string Name = "config";

        private readonly Dictionary<GameParam, object> _paramsMap;
        public readonly Dictionary<string, ShopProductIAPConfig> ShopProductIAPMap;

        public GameConfig()
        {
            ShopProductIAPMap = new Dictionary<string, ShopProductIAPConfig>();
            _paramsMap = new Dictionary<GameParam, object>();

            _paramsMap[GameParam.PlayerWalkSpeed] = 5f;
            _paramsMap[GameParam.AngleLerpFactor] = 10f;
            _paramsMap[GameParam.UnitWalkSpeed] = 5.5f;
            _paramsMap[GameParam.CoinRadius] = 1f;
            _paramsMap[GameParam.CoinsByRescue] = 10f;
            _paramsMap[GameParam.AudibilityRadius] = 10f;
            _paramsMap[GameParam.MinDistanceToEnemy] = 5f;
            _paramsMap[GameParam.RescueReloadDuration] = 3f;
            _paramsMap[GameParam.TimeToHide] = 3f;
            _paramsMap[GameParam.CameraGameplayZoom] = 60;
            _paramsMap[GameParam.CameraMenuZoom] = 88;
            _paramsMap[GameParam.CameraSeekZoom] = 20;
            _paramsMap[GameParam.SeekerIdleDuration] = 0.5f;
            _paramsMap[GameParam.UnitIdleDuration] = .25f;
        }

        public static GameConfig Load()
        {
            var result = Resources.Load<GameConfig>("GameConfig");
            result.Init();
            return result;
        }

        private void Init()
        {
            foreach (var product in _shopProductIAPConfigs)
            {
                ShopProductIAPMap[product.ID] = product;
            }
        }

        public float GetValue(GameParam param)
        {
            if (_paramsMap.TryGetValue(param, out object value))
            {
                return Convert.ToSingle(value);
            }

            throw new KeyNotFoundException(param.ToString());
        }

        public Vector3 GetV3Value(GameParam param)
        {
            if (_paramsMap.TryGetValue(param, out object value))
            {
                if (!(value is Vector3))
                {
                    if (TryParseVector3(value.ToString(), out Vector3 vector))
                    {
                        value = vector;
                        _paramsMap[param] = value;
                    }
                }

                return (Vector3)value;
            }

            throw new KeyNotFoundException(param.ToString());
        }

        public bool TryParseVector3(string value, out Vector3 vector)
        {
            if (value.Contains("("))
            {
                value = value.Replace("(", ":");
                value = value.Replace(",", ",:");
                value = value.Replace(")", string.Empty);
                value = value.Replace(" ", string.Empty);
            }
            value = value.Replace("\r", string.Empty);
            value = value.Replace("\n", string.Empty);
            value = value.Replace("}", string.Empty);
            value = value.Replace("\"", string.Empty);

            var arg = StringUtil.Split(value, ",");
            float x = 0;
            float y = 0;
            float z = 0;

            bool result = float.TryParse(StringUtil.Split(arg[0], ":")[1], out x);
            result = result && float.TryParse(StringUtil.Split(arg[1], ":")[1], out y);
            result = result && float.TryParse(StringUtil.Split(arg[2], ":")[1], out z);

            vector = new Vector3(x, y, z);
            return result;
        }

        [Header("Defaults")]
        [Min(1)] public int DefaultLevel;
        public bool DefaultIsSeek;

        [Header("Seeker")]
        public float SeekerRadius;
        public float SeekerAngel;

        [Header("Level")]
        public float LevelDuration;
        [Min(3)] public int MinUnitsCaughtToWin;

        [Header("Splash Screen")]
        [SerializeField] private float _splashScreenDuration;
        [SerializeField] private float _splashScreenDurationEditor;

        [Header("Ads")]
        public AdsProviderType AdsProviderEditor;
        public AdsProviderType AdsProvider;

        [Header("Shop")]
        [Min(1)] public int AllScenariosDurationHrs;
        [Min(1)] public int NoScenarioDurationMinutes;
        [SerializeField] private ShopProductIAPConfig[] _shopProductIAPConfigs;

        [Header("Camera")]
        public float CameraZoomDuration;

        public float SplashScreenDuration
        {
            get
            {
                var result = _splashScreenDuration;
#if UNITY_EDITOR
                result = _splashScreenDurationEditor;
#endif
                return result;
            }
        }
    }
}