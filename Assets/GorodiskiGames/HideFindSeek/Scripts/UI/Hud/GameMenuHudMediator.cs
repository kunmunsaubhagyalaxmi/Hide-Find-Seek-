using Game.Core.UI;
using Game.Managers;
using Game.States;
using Injection;

namespace Game.UI.Hud
{
    public sealed class GameMenuHudMediator : Mediator<GameMenuHudView>
    {
        private const string _levelPattern = "{0} {1}";
        private const string _arenaWord = "ARENA";

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameManager _gameManager;
        [Inject] private HudManager _hudManager;

        protected override void Show()
        {
            _view.LevelLabetText.text = string.Format(_levelPattern, _arenaWord, _gameManager.Model.Level);

            _view.Model = _gameManager.Model;

            _view.SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _view.PlayButton.onClick.AddListener(OnPlayButtonClicked);
            _view.ShopButton.onClick.AddListener(OnShopButtonClicked);
        }

        protected override void Hide()
        {
            _view.SettingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _view.PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
            _view.ShopButton.onClick.RemoveListener(OnShopButtonClicked);
        }

        private void OnShopButtonClicked()
        {
            _hudManager.ShowAdditional<ShopHudMediator>();
        }

        private void OnPlayButtonClicked()
        {
            if(_gameManager.Model.IsSeek)
                _gameStateManager.SwitchToState(typeof(GamePlaySeekState));
            else
                _gameStateManager.SwitchToState(typeof(GamePlayHideState));
        }

        private void OnSettingsButtonClicked()
        {
            _hudManager.ShowAdditional<SettingsHudMediator>();
        }
    }
}