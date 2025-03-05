using UnityEngine;

namespace Hashira.LatestUI
{
    public class InGameUI : UIBase, IToggleUI
    {
        [field:SerializeField]
        public string Key { get; set; }

        public void Open()
        {
        }

        public void Close()
        {
        }
    }
}
