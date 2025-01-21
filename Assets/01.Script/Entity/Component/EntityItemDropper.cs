using AYellowpaper.SerializedCollections;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityItemDropper : MonoBehaviour, IEntityComponent, IEntityDisposeComponent
    {
        [SerializeField] private SerializedDictionary<ItemSO, float> _itemPercent;
        private Entity _entity;
        
        private void HandleItemDrop()
        {
            float percent = Random.Range(0, 100.0f);
            float sum = 0;
            float lastSum = 0;
            foreach (var itemPair in _itemPercent)
            {
                sum += itemPair.Value;
                if (lastSum <= percent && percent < sum)
                {
                    ItemDropUtility.DroppedItem(itemPair.Key, _entity.transform.position);
                    return;
                }
                lastSum = sum;
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entity.GetEntityComponent<EntityHealth>().OnDieEvent += HandleItemDrop;
        }

        public void Dispose()
        {
            _entity.GetEntityComponent<EntityHealth>().OnDieEvent -= HandleItemDrop;
        }
    }
}
