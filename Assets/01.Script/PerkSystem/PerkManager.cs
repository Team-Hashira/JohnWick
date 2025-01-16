using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.PerkSystem.Perks;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.PerkSystem
{
    public class PerkManager : MonoBehaviour
    {
        private static readonly Dictionary<Type, Perk> _perks = new Dictionary<Type, Perk>();
        private static readonly Dictionary<Type, Perk> _currentPerks = new Dictionary<Type, Perk>();

        public static event Action<Perk> OnAddedNewPerkEvent; 
        
        private void Awake()
        {
            var perks = GetComponentsInChildren<Perk>();

            foreach (var perk in perks)
            {
                _perks.Add(perk.GetType(), perk);
            }
        }

        private void OnDestroy()
        {
            OnAddedNewPerkEvent = null;
        }

        public static void RemovePerk(Perk perk)
        {
            if (_perks.Remove(perk.GetType()))
            {
                
            }
        }
        
        public static void AddPerk<T>() where T : Perk
        {
            Type type = typeof(T);
            AddPerk(type);
        }
        
        public static void AddPerk(Type type) 
        {
            if (_perks.TryGetValue(type, out var perk))
            {
                perk.currentCoolTime = perk.coolTime;
                _currentPerks.Add(type, perk);
                OnAddedNewPerkEvent?.Invoke(perk);
            }
        }
        
        public static Perk GetPerk(int index)
        {
            return _currentPerks.ElementAt(index).Value;
        }
        
        public static T GetPerk<T>() where T : Perk
        {
            if (_currentPerks.TryGetValue(typeof(T), out Perk perk))
            {
                return perk as T;
            }
            
            return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddPerk<MoveSpeedUp>();
            }
            
            foreach (var perkPair in _currentPerks)
            {
                Perk perk = perkPair.Value;
                if (perk.useCoolTime == false) return;
                
                if (perk.currentCoolTime >= perk.coolTime)
                {
                    //조건 이벤트를 달고 있을 스킬이 아니라면 
                    if (perk.useConditionalEvent == false)
                        perk.UsePerk(); //스킬 사용
                    //만약 조건 이벤트를 달고 있을 스킬이라면
                    else
                    //조건만 충조한다면 바로 실행할 수 있도록 준비
                        perk.IsReadyCoolTime = true;
                }
                else
                    perk.currentCoolTime += Time.deltaTime;
            }
        }
    }
}
