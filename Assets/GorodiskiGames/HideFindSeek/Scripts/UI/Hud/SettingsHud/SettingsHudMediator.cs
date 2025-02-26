using Game.Config;
using Game.Core.UI;
using Game.Managers;
using Injection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI.Hud
{
    public sealed class SettingsHudMediator : Mediator<SettingsHudView>
    {
        private const int _cashAmount = 10000;

        private const string _instagramLink = "https://www.instagram.com/gorodiski.games/";
        private const string _instagramPersonalLink = "https://www.instagram.com/sasha.gorodiski/";
        private const string _discordLink = "https://discord.gg/ADNhwCz3Dw";
        private const string _youTubeLink = "https://www.youtube.com/channel/UCOpCPQiFQK6VBZb_Hq3Hf6Q";
        private const string _twitterLink = "https://x.com/sasha_gorodiski";

        [Inject] private GameConfig _config;
        [Inject] private GameManager _gameManager;
        [Inject] private AdsManager _adsManager;
        [Inject] private IAPManager _IAPManager;
        [Inject] private GameView _gameView;
        [Inject] private VibrateManager _vibrateManager;

        protected override void Show()
        {
            var isDebugBuild = GameConstants.IsDebugBuild();
            _view.ResetButton.gameObject.SetActive(isDebugBuild);
            _view.AddCashButton.gameObject.SetActive(isDebugBuild);

#if UNITY_ANDROID
			_view.RestorePurchasesButton.gameObject.SetActive(false);
#endif

            VibrationToggleVisibility();
            JoystickVisibilityToggleVisibility();

            _view.CloseButton.onClick.AddListener(OnCloseButtonClick);
            _view.RestorePurchasesButton.onClick.AddListener(OnRestoreClick);
            _view.VibrationToggle.onValueChanged.AddListener(OnVibrationToggleClick);
            _view.JoystickVisibilityToggle.onValueChanged.AddListener(OnJoystickVisibilityToggleClick);

            _view.ResetButton.onClick.AddListener(ResetButtonClick);
            _view.AddCashButton.onClick.AddListener(AddCashButtonClick);

            _view.YouTubeButton.onClick.AddListener(OnYouTubeButtonClick);
            _view.InstagramButton.onClick.AddListener(OnInstagramButtonClick);
            _view.TwitterButton.onClick.AddListener(OnTwitterButtonClick);
            _view.DiscordButton.onClick.AddListener(OnDiscordButtonClick);

            _IAPManager.ON_PRODUCT_PURCHASED += OnProductPurchased;
        }

        protected override void Hide()
        {
            _view.CloseButton.onClick.RemoveListener(OnCloseButtonClick);
            _view.RestorePurchasesButton.onClick.RemoveListener(OnRestoreClick);
            _view.VibrationToggle.onValueChanged.RemoveListener(OnVibrationToggleClick);
            _view.JoystickVisibilityToggle.onValueChanged.RemoveListener(OnJoystickVisibilityToggleClick);

            _view.ResetButton.onClick.RemoveListener(ResetButtonClick);
            _view.AddCashButton.onClick.RemoveListener(AddCashButtonClick);

            _view.YouTubeButton.onClick.RemoveListener(OnYouTubeButtonClick);
            _view.InstagramButton.onClick.RemoveListener(OnInstagramButtonClick);
            _view.TwitterButton.onClick.RemoveListener(OnTwitterButtonClick);
            _view.DiscordButton.onClick.RemoveListener(OnDiscordButtonClick);

            _IAPManager.ON_PRODUCT_PURCHASED -= OnProductPurchased;
        }

        private void AddCashButtonClick()
        {
            _gameManager.Model.Cash += _cashAmount;
            _gameManager.Model.Save();
            _gameManager.Model.SetChanged();
        }

        private void OnCloseButtonClick()
        {
            InternalHide();
        }

        private void OnRestoreClick()
        {
            _IAPManager.RestorePurchases();
        }

        private void OnProductPurchased(string productID)
        {
            var config = _config.ShopProductIAPMap[productID];
            var reward = config.Reward;

            if (reward == ShopProductReward.NoAds)
            {
                _gameManager.Model.IsNoAds = true;
                _gameManager.Model.Save();
                _adsManager.SetNoAds();
            }
        }

        private void VibrationToggleVisibility()
        {
            var isVibration = _gameManager.Model.IsVibration;
            _view.VibrationToggle.isOn = isVibration;
        }

        private void OnVibrationToggleClick(bool value)
        {
            _gameManager.Model.IsVibration = value;
            _gameManager.Model.Save();

            _vibrateManager.SetVibration(value);
            if (value == true)
                _vibrateManager.Vibrate();

            VibrationToggleVisibility();
        }

        private void ResetButtonClick()
        {
            _gameManager.Model.Remove();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void OnYouTubeButtonClick()
        {
            Application.OpenURL(_youTubeLink);
        }

        private void OnInstagramButtonClick()
        {
            Application.OpenURL(_instagramLink);
        }

        private void OnDiscordButtonClick()
        {
            Application.OpenURL(_discordLink);
        }

        private void OnTwitterButtonClick()
        {
            Application.OpenURL(_twitterLink);
        }

        private void OnJoystickVisibilityToggleClick(bool value)
        {
            _gameManager.Model.JoystickVisibility = value;
            _gameManager.Model.Save();

            _gameView.Joystick.JoystickVisibility(value);

            JoystickVisibilityToggleVisibility();
        }

        private void JoystickVisibilityToggleVisibility()
        {
            _view.JoystickVisibilityToggle.isOn = _gameManager.Model.JoystickVisibility;
        }
    }
}

