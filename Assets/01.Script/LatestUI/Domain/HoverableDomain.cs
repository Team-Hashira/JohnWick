using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class HoverableDomain : UIManagementDomain
    {
        private IHoverableUI _hoveredUI;

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (_hoveredUI == null)
            {
                foreach (var ui in _uiList)
                {
                    GameObject uiObject;
                    bool isUIUnderCursor = UIManager.UIInteractor.IsUIUnderCursor(out uiObject);
                    if (isUIUnderCursor)
                    {
                        if (uiObject.TryGetComponent(out IHoverableUI hoverable))
                        {
                            _hoveredUI = hoverable;
                            _hoveredUI.OnCursorEnter();
                            break;
                        }
                    }
                }
            }
            else
            {
                GameObject uiObject;
                bool isUIUnderCursor = UIManager.UIInteractor.IsUIUnderCursor(out uiObject);
                if (!isUIUnderCursor)
                {
                    CallOnCursorExit();
                }
                else
                {
                    if (uiObject.TryGetComponent(out IHoverableUI hoverable))
                    {
                        if (_hoveredUI != hoverable)
                        {
                            CallOnCursorExit();
                        }
                    }
                    else
                    {
                        CallOnCursorExit();
                    }
                }
            }
        }
        private void CallOnCursorExit()
        {
            _hoveredUI.OnCursorExit();
            _hoveredUI = null;
        }
    }
}