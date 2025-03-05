using UnityEngine;

namespace Hashira.LatestUI
{
    public interface IClickableUI : IUserInterface
    {
        public Collider2D Collider { get; set; }

        public void OnClick();
        public void OnClickEnd();
    }
}
