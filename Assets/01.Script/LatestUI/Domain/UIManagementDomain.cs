using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public abstract class UIManagementDomain
    {
        protected List<IUserInterface> _uiList;

        public UIManagementDomain()
        {
            _uiList = new List<IUserInterface>();
        }

        public virtual void UpdateUI() { }

        public virtual void AddUI(IUserInterface uiInterface)
        {
            _uiList.Add(uiInterface);
        }

        public virtual void RemoveUI(IUserInterface uiInterface)
        {
            _uiList.Remove(uiInterface);
        }
    }
}
