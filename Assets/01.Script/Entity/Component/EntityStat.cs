using Hashira.Core.StatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private List<StatElement> _overrideStatElementList = new List<StatElement>();
        [SerializeField] private StatBaseSO _baseStat;

        private Dictionary<string, StatElement> _overrideStatDictionary;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _overrideStatDictionary = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in _overrideStatElementList)
            {
                if (statElement.elementSO == null) continue;

                statElement.Initialize();
                _overrideStatDictionary.Add(statElement.elementSO.statName, statElement);
            }
        }

        public StatElement[] GetElements()
        {
            List<StatElement> elementList = new List<StatElement>();

            foreach (StatElement statElement in _overrideStatDictionary.Values)
                elementList.Add(statElement);

            foreach (var statElement in _baseStat.GetStatElements())
                elementList.Add(statElement);
            
            return elementList.ToArray();
        }
        
        public StatElement GetElement(StatElementSO statType)
        {
            if (_overrideStatDictionary.TryGetValue(statType.statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statType.statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }

        public bool TryGetElement(StatElementSO statType, out StatElement statElement)
        {
            if (_overrideStatDictionary.TryGetValue(statType.statName, out statElement)) return true;

            statElement = _baseStat.GetStatElement(statType.statName);
            return statElement != null;
        }

        public StatElement GetElement(string statName)
        {
            if (_overrideStatDictionary.TryGetValue(statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }

        public bool TryGetElement(string statName, out StatElement statElement)
        {
            if (_overrideStatDictionary.TryGetValue(statName, out statElement)) return true;

            statElement = _baseStat.GetStatElement(statName);
            return statElement != null;
        }
    }
}