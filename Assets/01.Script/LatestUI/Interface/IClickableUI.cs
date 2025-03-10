using System;
using UnityEngine;

namespace Hashira.LatestUI
{
    public delegate void OnClickEvent();

    public interface IClickableUI : IUserInterface
    {
        [Obsolete]
        public Collider2D Collider { get; set; }

        public void OnClick();
        public void OnClickEnd();
    }
}
