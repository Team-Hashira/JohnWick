using Hashira.Core.StatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    [CreateAssetMenu(fileName = "Module", menuName = "SO/Item/Module")]
    public class ModuleSO : ItemSO
    {
        [Header("==========Module setting==========")]
        [field: SerializeField] public List<StatElementAdjustment> StatVariationList { get; private set; }
    }
}
