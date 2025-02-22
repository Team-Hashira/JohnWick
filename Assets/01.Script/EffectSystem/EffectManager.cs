using System.Collections.Generic;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        [SerializeField] private List<EffectUIDataSO> _effectUIDataSOList = new List<EffectUIDataSO>();

        public List<EffectUIDataSO> EffectUIDataSOList
        {
            get => _effectUIDataSOList;
        }
    }
}
