//using UnityEngine;
//using Vibration;


namespace Game.Managers
{
    public sealed class VibrateManager
    {
        private bool _isVibration;

        public void Initialize(bool isVibration)
        {
            //Vibration.Init();

            SetVibration(isVibration);
        }

        public void SetVibration(bool isVibration)
        {
            _isVibration = isVibration;
        }

        public void Vibrate()
        {
            if (!_isVibration)
                return;

            Log.Info("Vibrate");

#if UNITY_ANDROID && !UNITY_EDITOR
            //Handheld.Vibrate(); // Native Unity vibration for Android
            //Vibration.VibrateAndroid(5);
#elif UNITY_IOS && !UNITY_EDITOR
            //Vibration.VibrateIOS(ImpactFeedbackStyle.Light);
#endif
        }
    }
}

