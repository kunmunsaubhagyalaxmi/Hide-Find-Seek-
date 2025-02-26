using System;
using Game.Config;
using UnityEngine;
using Core;

namespace Game
{
    [SerializeField]
    public sealed class GameModel : Observable
    {
        private const string _gameModelKey = "model";

        public static GameModel Load(GameConfig config)
        {
            try
            {
                var data = PlayerPrefs.GetString(_gameModelKey);
                //Log.Info("Load. " + data);
                if (string.IsNullOrEmpty(data))
                {
                    var model = new GameModel(config);
                    return model;
                }
                var result = JsonUtility.FromJson<GameModel>(data);
                return result;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                var model = new GameModel(config);
                return model;
            }
        }

        public int Level;
        public int Cash;
        public bool IsVibration;
        public bool IsNoAds;
        public bool IsSeek;
        public bool JoystickVisibility;

        public GameModel()
        {
        }

        public GameModel(GameConfig config) : base()
        {
            Level = config.DefaultLevel;
            IsSeek = config.DefaultIsSeek;
            IsVibration = true;
            IsNoAds = false;
            JoystickVisibility = false;
        }

        public void Save()
        {
            var data = JsonUtility.ToJson(this);
            //Log.Info("Save. " + data);
            PlayerPrefs.SetString(_gameModelKey, data);
            PlayerPrefs.Save();
        }

        public void Remove()
        {
            PlayerPrefs.DeleteKey(_gameModelKey);
            PlayerPrefs.Save();
        }
    }
}