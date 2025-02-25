using Hashira.Items.Modules;
using UnityEngine;

namespace Hashira.UI
{
    public class ModuleSlot : MonoBehaviour
    {
        ModuleSO _baseModuleSO;

        public void Init(ModuleSO baseModule)
        {
            _baseModuleSO = baseModule;
        }
    }
}
