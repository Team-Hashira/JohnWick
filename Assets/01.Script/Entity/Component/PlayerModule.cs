using Hashira.Items;
using Hashira.Items.Module;
using Hashira.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class PlayerModule : MonoBehaviour, IEntityComponent
    {
        private Player _player;

        public List<Module> ListModules { get; private set; }

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            ListModules = new List<Module>();
        }

        private void Update()
        {
            for (int i = 0; i < ListModules.Count; i++)
            {
                ListModules[i].ItemUpdate();
            }
        }

        public void AddModule(Module moduleItem)
        {
            ListModules.Add(moduleItem);
            moduleItem.Equip(_player);
        }

        public void RemoveModule<T>(T moduleItem) where T : Module
        {
            Module removeTargetModule
                = ListModules.FirstOrDefault(module => module.GetType() == typeof(T));
            if (removeTargetModule != default)
            {
                ListModules.Remove(removeTargetModule);
                moduleItem.UnEquip();
            }
        }
    }
}
