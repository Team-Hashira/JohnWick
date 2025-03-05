using UnityEngine;

namespace Hashira.LatestUI
{
    public interface IToggleUI : IUserInterface
    {
        public string Key { get; set; }

        public void Open();
        public void Close();
    }
}
