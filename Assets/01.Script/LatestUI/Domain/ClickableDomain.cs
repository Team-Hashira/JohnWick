using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.LatestUI
{
    public class ClickableDomain : UIManagementDomain
    {
        public override void UpdateUI()
        {
            base.UpdateUI();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (var ui in _uiList)
                {
                    IClickableUI clickable = ui as IClickableUI;
                    bool isOverlapped = clickable.Collider.OverlapPoint(UIManager.MousePosition);
                    if (isOverlapped)
                    {
                        clickable.OnClick();
                    }
                }
            }
            else if(Mouse.current.leftButton.wasReleasedThisFrame)
            {
                foreach (var ui in _uiList)
                {
                    IClickableUI clickable = ui as IClickableUI;
                    bool isOverlapped = clickable.Collider.OverlapPoint(UIManager.MousePosition);
                    if (isOverlapped)
                    {
                        clickable.OnClickEnd();
                    }
                }
            }
        }
    }
}
