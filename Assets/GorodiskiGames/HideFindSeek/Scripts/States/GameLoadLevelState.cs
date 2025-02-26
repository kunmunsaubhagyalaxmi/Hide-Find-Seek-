using Game.Config;
using Game.Level;
using Game.Managers;
using Game.UI.Hud;
using Injection;
using UnityEngine.SceneManagement;

namespace Game.States
{
    public class GameLoadLevelState : GameState
    {
        [Inject] private GameConfig _config;
        [Inject] private Context _context;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private HudManager _hudManager;

        private int _level;

        public override void Initialize()
        {
            _hudManager.ShowAdditional<SplashScreenHudMediator>();

            var model = GameModel.Load(_config);
            _level = model.Level;

            var sceneCount = SceneManager.sceneCountInBuildSettings;
            if (_level >= sceneCount)
            {
                _level = 1;
                model.Level = _level;
                model.Save();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                {
                    SceneManager.UnloadSceneAsync(i);
                }
            }
            LoadScene();
        }

        public override void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode arg)
        {
            LevelView level = null;
            var sceneObjects = scene.GetRootGameObjects();
            foreach (var sceneObject in sceneObjects)
            {
                level = sceneObject.GetComponent<LevelView>();
                if (null != level)
                    break;
            }

            _context.Install(level);

            _gameStateManager.SwitchToState<GameMenuState>();
        }

        public virtual void LoadScene()
        {
            SceneManager.LoadScene(_level, LoadSceneMode.Additive);
        }
    }
}