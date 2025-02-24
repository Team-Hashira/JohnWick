using UnityEngine;

namespace Hashira.UI
{
    public class ModuleSlot : MonoBehaviour
    {
        ModuleItemSO _baseModuleSO;

        public void Init(ModuleItemSO baseModule)
        {
            _baseModuleSO = baseModule;
        }
    }
}
