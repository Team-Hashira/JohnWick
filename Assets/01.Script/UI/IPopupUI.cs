using UnityEngine;

namespace Hashira
{
    public enum EPopupUIName
    {
        ItemDataUI,

    }

    public interface IPopupUI
    {
        public EPopupUIName PopupUIName {  get; }
        public void Show();
        public void Hide();
    }
}
