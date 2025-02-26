using Game.Core.UI;
using Game.Managers;
using Game.States;
using Injection;

namespace Game.UI.Hud
{
    public sealed class WinHudMediator : Mediator<WinHudView>
    {
        [Inject] private GameManager _gameManager;
        [Inject] private GameStateManager _gameStateManager;

        protected override void Show()
        {
            var isSeek = _gameManager.Model.IsSeek;
            _gameManager.Model.IsSeek = !isSeek;

            _gameManager.Model.Level++;
            _gameManager.Model.Save();

            _view.NextButton.onClick.AddListener(OnNextButtonClick);
        }

        protected override void Hide()
        {
            _view.NextButton.onClick.RemoveListener(OnNextButtonClick);
        }

        private void OnNextButtonClick()
        {
            InternalHide();
            _gameStateManager.SwitchToState<GameLoadLevelState>();
        }
    }
}