using Core;
using UnityEngine;
using Game.Unit;

namespace Game.UI.Hud
{
    public sealed class UnitMarkerView : BehaviourWithModel<UnitModel>
    {
        [SerializeField] private GameObject _seekIcon;
        [SerializeField] private GameObject _hideIcon;

        private GameObject _icon;

        public void Initialize(bool isSeek)
        {
            _icon = _seekIcon;
            if(!isSeek)
                _icon = _hideIcon;
        }

        protected override void OnModelChanged(UnitModel model)
        {
            _icon.SetActive(model.IsCaught);
        }
    }
}

