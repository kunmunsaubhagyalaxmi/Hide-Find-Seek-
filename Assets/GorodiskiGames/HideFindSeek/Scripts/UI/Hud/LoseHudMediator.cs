using Game.Config;
using Game.Core.UI;
using Game.States;
using Injection;

namespace Game.UI.Hud
{
    public enum LoseType
    {
        NotEnoughtUnits,
        PlayerCaught
    }

    public sealed class LoseHudMediator : Mediator<LoseHudView>
    {
        private const string _notEnoughtUnitsLabel = "CATCH AT LEAST";
        private const string _playerCaughtLabel = "CAUGHT";

        private const string _notEnoughtUnitsPatter = "{0} {1}";
        private const string _playerCaughtPatter = "{0}";

        [Inject] private GameConfig _config;
        [Inject] private GameStateManager _gameStateManager;

        private LoseType _type;

        public LoseHudMediator(LoseType type)
        {
            _type = type;
        }

        protected override void Show()
        {
            var info = string.Format(_notEnoughtUnitsPatter, _notEnoughtUnitsLabel, _config.MinUnitsCaughtToWin);
            if(_type == LoseType.PlayerCaught)
                info = string.Format(_playerCaughtPatter, _playerCaughtLabel);

            _view.InfoText.text = info;

            _view.RestartButton.onClick.AddListener(OnRestartButtonClick);
        }

        protected override void Hide()
        {
            _view.RestartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick()
        {
            InternalHide();
            _gameStateManager.SwitchToState<GameLoadLevelState>();
        }
    }
}