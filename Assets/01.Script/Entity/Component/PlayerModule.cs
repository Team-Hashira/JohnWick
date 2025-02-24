using Hashira.Entities;
using Hashira.Items;
using Hashira.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class PlayerModule : MonoBehaviour, IEntityComponent
    {
        private Player _player;

        public List<ModuleItem> ListModules { get; private set; }

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        public void AddModule(ModuleItem moduleItem)
        { 
            ListModules.Add(moduleItem);
        }

        public void RemoveModule<T>(T moduleItem) where T : ModuleItem
        {
            ModuleItem removeTargetModule 
                = ListModules.FirstOrDefault(module => module.GetType() == typeof(T));
            if (removeTargetModule != default)
                ListModules.Remove(removeTargetModule);
        }
    }
}
