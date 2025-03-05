using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    public enum EModifyCondition
    {
        BulletFire = 1,
        BulletHit = 2,
        Static = 4,
    }
    public enum EDelayType
    {
        Cool = 1,
        Count = 2,
        Custom = 4
    }

    [CreateAssetMenu(fileName = "ProjectileModifier", menuName = "SO/Item/ProjectileModifier")]
    public class ProjectileModifierSO : ScriptableObject
    {
        [Header("==========ProjectileModifier setting==========")]
        public string className;

        protected ProjectileModifier _projectileModifier;

        protected Type _itemType;

        public ProjectileModifier GetItemClass()
        {
            return Activator.CreateInstance(_itemType) as ProjectileModifier;
        }

        protected virtual void OnValidate()
        {
            string thisTag = GetType().ToString();
            int tagStartIdx = thisTag.LastIndexOf(".");                 //Namespace.Class
            string namespaceName = thisTag[..tagStartIdx];              //Namespace
            string typeName = namespaceName + "." + className;          //Namespace.Default

            try
            {
                _itemType = Type.GetType(typeName);
                ProjectileModifier foundModifier = Activator.CreateInstance(_itemType) as ProjectileModifier;
                foundModifier.Init(this);
                _projectileModifier = foundModifier;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} not found.\n" +
                                $"Error : {ex.ToString()}");
                _projectileModifier = null;
            }
        }
    }
}
