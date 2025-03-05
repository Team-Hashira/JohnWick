using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class ToggleDomain : UIManagementDomain
    {
        public Dictionary<string, IToggleUI> _uiDictionary;

        public ToggleDomain()
        {
            _uiDictionary = new Dictionary<string, IToggleUI>();
        }

        public void OpenUI(string key)
            => _uiDictionary[key].Open();

        public void CloseUI(string key)
            => _uiDictionary[key].Close();

        public override void AddUI(IUserInterface uiInterface)
        {
            base.AddUI(uiInterface);
            IToggleUI toggle = uiInterface as IToggleUI;
            _uiDictionary.Add(toggle.Key, toggle);
        }

        public override void RemoveUI(IUserInterface uiInterface)
        {
            base.RemoveUI(uiInterface);
            IToggleUI toggle = uiInterface as IToggleUI;
            _uiDictionary.Remove(toggle.Key);
        }
    }
}
