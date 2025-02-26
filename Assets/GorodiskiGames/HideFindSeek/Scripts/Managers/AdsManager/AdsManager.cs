using System;
using Game.Config;
using Game.Core;
using Injection;

namespace Game.Managers
{
    public enum AdsProviderType
    {
        Fake,
        AdMob
    }

    public enum AdsPlacement
    {
        Default,
        CashForAds,
        CashForAdsEquipmentHud,
        EquipmentForAds
    }

    public sealed class AdsManager : IDisposable
    {
        public Action ON_INTERSTITIAL_SHOW;
        public Action ON_INTERSTITIAL_WATCHED;
        public Action ON_INTERSTITIAL_LOADED;
        public Action ON_INTERSTITIAL_FAILED_TO_LOAD;

        public Action ON_REWARDED_WATCHED;

        [Inject] private Timer _timer;

        private BaseAdsProxy _adsProxy;
        private GameConfig _config;

        private bool _isNoAds;

        public void Initialize(bool isNoAds, GameConfig config)
        {
            _isNoAds = isNoAds;
            _config = config;

#if UNITY_EDITOR
            if (_config.AdsProviderEditor == AdsProviderType.Fake)
                _adsProxy = new FakeAdsProxy(_timer);
            else
                _adsProxy = new GoogleAdMobProxy(_timer);
#else
            if (_config.AdsProvider == AdsProviderType.AdMob)
                _adsProxy = new GoogleAdMobProxy(_timer);
            else
                _adsProxy = new FakeAdsProxy(_timer);
#endif
            _adsProxy.ON_REWARDED_WATCHED += OnRewardedWatched;

            _adsProxy.ON_INTERSTITIAL_WATCHED += OnInterstitialWatched;
            _adsProxy.ON_INTERSTITIAL_SHOW += OnInterstitialShow;

            _adsProxy.INITIALIZED += OnInitialized;
            _adsProxy.Initialize();
        }

        public void Dispose()
        {
            _adsProxy.UnloadBanner();

            _adsProxy.ON_REWARDED_WATCHED -= OnRewardedWatched;

            _adsProxy.ON_INTERSTITIAL_WATCHED -= OnInterstitialWatched;
            _adsProxy.ON_INTERSTITIAL_SHOW -= OnInterstitialShow;

            _adsProxy.INITIALIZED -= OnInitialized;
            _adsProxy.Dispose();
        }

        private void OnInitialized()
        {
            _adsProxy.LoadRewarded();
            _adsProxy.LoadInterstitial();
            _adsProxy.LoadBanner();

            ShowBanner();
        }

        public void ShowInterstitial()
        {
            if (_isNoAds)
                return;

            try
            {
                _adsProxy.ShowInterstitial();
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
        }

        public void ShowRewarded()
        {
            try
            {
                _adsProxy.ShowRewarded();
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
        }

        public void ShowBanner()
        {
            if (_isNoAds)
                return;

            try
            {
                _adsProxy.ShowBanner();
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
        }

        public void HideBanner()
        {
            try
            {
                _adsProxy.HideBanner();
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
        }

        private void OnRewardedWatched()
        {
            Log.Info($"Rewarded watched");
            ON_REWARDED_WATCHED?.Invoke();
        }

        private void OnInterstitialWatched()
        {
            Log.Info($"Interstitial watched");
            ON_INTERSTITIAL_WATCHED?.Invoke();
        }

        private void OnInterstitialShow()
        {
            Log.Info($"Interstitial show");
            ON_INTERSTITIAL_SHOW?.Invoke();
        }

        public void SetNoAds()
        {
            _isNoAds = true;
            HideBanner();
        }
    }
}

