using TMPro;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public sealed class TextInputOverride : InputField
    {
        public TMP_InputField.SelectionEvent onSelect = new TMP_InputField.SelectionEvent();

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            onSelect.Invoke(text);
        }
    }
}