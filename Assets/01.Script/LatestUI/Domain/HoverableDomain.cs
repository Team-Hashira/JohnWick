using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class HoverableDomain : UIManagementDomain
    {
        private Dictionary<int, IHoverableUI> _hoverableDict;

        public HoverableDomain()
        {
            _hoverableDict = new Dictionary<int, IHoverableUI>();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            foreach (var ui in _uiList)
            {
                IHoverableUI hoverable = ui as IHoverableUI;
                bool isOverlapped = hoverable.Collider.OverlapPoint(UIManager.MousePosition);
                if (isOverlapped)
                {
                    if (_hoverableDict.TryGetValue(hoverable.GetHashCode(), out IHoverableUI h))
                    {
                        if (h == null)
                        {
                            CallOnMouseEnter(hoverable);
                        }
                    }
                    else
                    {
                        CallOnMouseEnter(hoverable);
                    }
                }
                else
                {
                    if (_hoverableDict.TryGetValue(hoverable.GetHashCode(), out hoverable))
                    {
                        if (hoverable != null)
                        {
                            CallOnMouseExit(hoverable);
                        }
                    }
                }
            }
        }

        public void CallOnMouseEnter(IHoverableUI ui)
        {
            ui.OnCursorEnter();
            if (_hoverableDict.TryGetValue(ui.GetHashCode(), out IHoverableUI h))
                _hoverableDict[ui.GetHashCode()] = ui;
            else
                _hoverableDict.Add(ui.GetHashCode(), ui);
        }

        public void CallOnMouseExit(IHoverableUI ui)
        {
            ui.OnCursorExit();
            _hoverableDict[ui.GetHashCode()] = null;
        }
    }
}
