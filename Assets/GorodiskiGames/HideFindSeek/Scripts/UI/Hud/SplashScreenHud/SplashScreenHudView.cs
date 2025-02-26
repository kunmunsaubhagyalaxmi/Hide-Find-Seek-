using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class SplashScreenHudView : BaseHud
    {
        [SerializeField] private Image _fillBarImage;
        [SerializeField] private TMP_Text _deviceIDText;

        public Image FillBarImage => _fillBarImage;

        protected override void OnEnable()
        {
            string deviceID = SystemInfo.deviceUniqueIdentifier;
            _deviceIDText.text = deviceID;
        }

        protected override void OnDisable()
        {
            
        }
    }
}