using Doryu.CustomAttributes;
using Hashira.Core.StatSystem;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    [Serializable]
    public class ProjectileModifierSetting
    {
        public ProjectileModifierSO projectileModifierSO;
        [SerializeReference] public ProjectileModifier projectileModifier;
        public ExecutionConditionSetting conditionSetting;
    }

    [CreateAssetMenu(fileName = "Module", menuName = "SO/Item/Module")]
    public class ModuleSO : ItemSO
    {
        [Header("==========Module setting==========")]

        [field: SerializeField] public List<ProjectileModifierSetting> ModifierList { get; private set; }
        [field: SerializeField] public List<StatElementAdjustment> StatVariationList { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();
            for (int i = 0; i < ModifierList.Count; i++)
            {
                ProjectileModifierSetting projectileModifierSetting = ModifierList[i];
                if (ModifierList[i].projectileModifierSO == null)
                {
                    projectileModifierSetting.projectileModifier = null;
                    continue;
                }
                if (projectileModifierSetting.projectileModifier != null) continue;
                projectileModifierSetting.projectileModifier = projectileModifierSetting.projectileModifierSO?.GetItemClass();
            }
        }
    }
}
