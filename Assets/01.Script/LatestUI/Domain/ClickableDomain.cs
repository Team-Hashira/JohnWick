using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.LatestUI
{
    public class ClickableDomain : UIManagementDomain
    {
        private IClickableUI _clickedUI;

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (var ui in _uiList)
                {
                    GameObject uiObject;
                    bool isUIUnderCursor = UIManager.UIInteractor.IsUIUnderCursor(out uiObject);
                    if (isUIUnderCursor)
                    {
                        if (uiObject.TryGetComponent(out IClickableUI clickable))
                        {
                            {
                                _clickedUI = clickable;
                                _clickedUI.OnClick();
                            }
                        }
                    }
                }
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (_clickedUI == null)
                    return;
                GameObject uiObject;
                bool isUIUnderCursor = UIManager.UIInteractor.IsUIUnderCursor(out uiObject);
                if (isUIUnderCursor)
                {
                    if (uiObject.TryGetComponent(out IClickableUI clickable))
                    {
                        if (clickable == _clickedUI)
                            _clickedUI.OnClickEnd();
                    }
                }
                _clickedUI = null;
            }
        }
    }
}
