using UnityEngine;

namespace Hashira.LatestUI
{
    public interface IHoverableUI : IUserInterface
    {
        public Collider2D Collider { get; set; }

        public void OnCursorEnter();
        public void OnCursorExit();
    }
}
