using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Game.UI.Hud
{
    public sealed class GameMenuHudView : BaseHudWithModel<GameModel>
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TMP_Text _cashText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private TMP_Text _levelLabetText;

        public Button SettingsButton => _settingsButton;
        public Button PlayButton => _playButton;
        public Button ShopButton => _shopButton;
        public TMP_Text LevelLabetText => _levelLabetText;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {
            
        }

        protected override void OnModelChanged(GameModel model)
        {
            _cashText.text = MathUtil.NiceCash(model.Cash);
        }
    }
}