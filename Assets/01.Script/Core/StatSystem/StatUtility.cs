using System.Collections.Generic;
using System.Linq;

namespace Hashira.Core.StatSystem
{
    public class StatDictionary
    {
        public Dictionary<string, StatElement> statElementList;

        public StatDictionary(List<StatElement> overrideStatElementList, StatBaseSO baseStat)
        {
            statElementList = new Dictionary<string, StatElement>();
            //�������� �ִ°� ���� �ֱ�
            foreach (StatElement statElement in overrideStatElementList)
            {
                if (statElement.elementSO == null) continue;

                statElement.Initialize();
                statElementList.Add(statElement.elementSO.statName, statElement);
            }
            //���̽��� �ִ°� �ֱ�
            foreach (StatElement statElement in baseStat.GetStatElements())
            {
                if (statElement.elementSO == null) continue;
                if (statElementList.ContainsKey(statElement.elementSO.statName)) continue;

                statElement.Initialize();
                statElementList.Add(statElement.elementSO.statName, statElement);
            }
        }
        public StatDictionary(List<StatElement> overrideStatElementList)
        {
            statElementList = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in overrideStatElementList)
            {
                if (statElement.elementSO == null) continue;

                statElement.Initialize();
                statElementList.Add(statElement.elementSO.statName, statElement);
            }
        }

        public StatElement[] GetElements()
            => statElementList.Values.ToArray();

        public StatElement this[StatElementSO statType]
        {
            get
            {
                if (statElementList.TryGetValue(statType.statName, out StatElement statElement))
                    return statElement;
                else
                    return null;
            }
            set => statElementList[statType.statName] = value;
        }
        public StatElement this[string statName]
        {
            get
            {
                if (statElementList.TryGetValue(statName, out StatElement statElement))
                    return statElement;
                else
                    return null;
            }
            set => statElementList[statName] = value;
        }

        public bool TryGetElement(StatElementSO statType, out StatElement statElement)
            => statElementList.TryGetValue(statType.statName, out statElement);
        public bool TryGetElement(string statName, out StatElement statElement)
            => statElementList.TryGetValue(statName, out statElement);
    } 
}