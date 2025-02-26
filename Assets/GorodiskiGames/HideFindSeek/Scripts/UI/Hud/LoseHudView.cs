using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class LoseHudView : BaseHud
    {
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Button _restartButton;

        public TMP_Text InfoText => _infoText;
        public Button RestartButton => _restartButton;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }
    }
}