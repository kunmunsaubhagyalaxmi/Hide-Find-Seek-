using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class SettingsHudView : BaseHud
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restorePurchasesButton;
        [SerializeField] private Toggle _vibrationToggle;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _addCashButton;
        [SerializeField] private Toggle _joystickVisibilityToggle;

        [SerializeField] private Button _youTubeButton;
        [SerializeField] private Button _instagramButton;
        [SerializeField] private Button _twitterButton;
        [SerializeField] private Button _discordButton;

        public Button CloseButton => _closeButton;
        public Button RestorePurchasesButton => _restorePurchasesButton;
        public Toggle VibrationToggle => _vibrationToggle;
        public Toggle JoystickVisibilityToggle => _joystickVisibilityToggle;

        public Button YouTubeButton => _youTubeButton;
        public Button InstagramButton => _instagramButton;
        public Button TwitterButton => _twitterButton;
        public Button DiscordButton => _discordButton;

        public Button ResetButton => _resetButton;
        public Button AddCashButton => _addCashButton;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }
    }
}