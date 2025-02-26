using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class WinHudView : BaseHud
    {
        [SerializeField] private Button _nextButton;
        public Button NextButton => _nextButton;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }
    }
}