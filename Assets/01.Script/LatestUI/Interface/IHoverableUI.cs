using System;
using UnityEngine;

namespace Hashira.LatestUI
{
    public interface IHoverableUI : IUserInterface
    {
        [Obsolete]
        public Collider2D Collider { get; set; }

        public void OnCursorEnter();
        public void OnCursorExit();
    }
}
