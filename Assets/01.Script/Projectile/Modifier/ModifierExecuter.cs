//using Doryu.CustomAttributes;
//using Hashira.Entities;
//using Hashira.Items.Modules;
//using Hashira.Projectiles;
//using System;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//namespace Hashira
//{
//    public enum EExecutionCondition
//    {
//        ByCount,
//        ByCooldown,
//        Custom
//    }
    
//    public enum ECountType
//    {
//        Loop,
//        Stack,
//        Down
//    }
    
//    public enum ECountCondition
//    {
//        Attack,     //맞췄을 때
//        Kill,       //죽였을 때
//        Hit,        //맞았을 때
//        Shoot,      //쏘았을 때
//        Jump,       //뛰었을 때
//        Heal,       //회복할 때
//        Collect,    //수집할 때
//        Object,     //파괴할 때
//    }
    
//    public enum EKillType
//    {
//        Common,     //맞췄을 때
//        Elite,      //죽였을 때
//        Boss,       //맞았을 때
//    }

//    [Serializable]
//    public class ExecutionConditionSetting
//    {
//        public EExecutionCondition condition;

//        //ByCount
//        [Header("=========ByCount=========")]
//        public int count;
//        public ECountType countType;
//        public ECountCondition countCondition;
//        public EKillType killType;

//        //ByCooldown
//        [Header("=========ByCooldown=========")]
//        public float cooldown;

//        //Custom
//        [Header("=========Custom=========")]
//        public string className;
//    }

//    public class ModifierExecuter
//    {
//        private ProjectileModifierSetting _modifierSetting;
//        private int _currentCount;
//        private Card _module;
//        private string _key;

//        public void Init(Attacker attacker, ProjectileModifierSetting modifierSetting, Card module)
//        {
//            _module = module;
//            _modifierSetting = modifierSetting;
//            _key = $"{_module.ItemSO.itemName}_{_modifierSetting.projectileModifierSO.name}";
//            attacker.OnProjectileCreateReadyEvent += HandleProjectileCreateReadyEvent;
//            attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
//            switch (modifierSetting.conditionSetting.condition)
//            {
//                case EExecutionCondition.ByCount:
//                    _currentCount = 0;
//                    break;
//                case EExecutionCondition.ByCooldown:
//                    CooldownUtillity.StartCooldown(_key);
//                    break;
//                case EExecutionCondition.Custom:
//                    break;
//            }
//        }

//        private void HandleProjectileCreateReadyEvent()
//        {
//            if (CheckCondition())
//            {
//                _modifierSetting.projectileModifier.OnProjectileCreateReady();
//            }
//        }

//        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
//        {
//            if (_modifierSetting.conditionSetting.condition == EExecutionCondition.ByCount &&
//                _modifierSetting.conditionSetting.countCondition == ECountCondition.Shoot)
//            {
//                _currentCount++;
//            }
//            if (CheckCondition())
//            {
//                foreach (Projectile projectile in projectileList)
//                {
//                    _modifierSetting.projectileModifier.OnProjectileCreate(projectile);
//                    projectile.OnHitEvent += HandleHitEvent;
//                }
//            }
//        }

//        private void HandleHitEvent(RaycastHit2D hit, IDamageable damageable)
//        {
//            _modifierSetting.projectileModifier.OnProjectileHit(hit, damageable);

//            if (hit.transform.TryGetComponent(out Entity entity) && entity.TryGetEntityComponent(out EntityHealth entityHealth))
//            {
//                switch (_modifierSetting.conditionSetting.countCondition)
//                {
//                    case ECountCondition.Attack:
//                        _currentCount++;
//                        break;
//                    case ECountCondition.Kill:
//                        if (entityHealth.IsDie)
//                            _currentCount++;
//                        break;
//                }
//            }
//        }

//        public bool CheckCondition()
//        {
//            switch (_modifierSetting.conditionSetting.condition)
//            {
//                case EExecutionCondition.ByCount:
//                    return _modifierSetting.conditionSetting.count <= _currentCount;
//                case EExecutionCondition.ByCooldown:
//                    return CooldownUtillity.CheckCooldown(_key, _modifierSetting.conditionSetting.cooldown);
//                case EExecutionCondition.Custom:
//                default:
//                    return true;
//            }
//        }

//        public void Reset()
//        {
//            _currentCount = 0;
//            CooldownUtillity.StartCooldown(_key);
//        }
//    }
//}
