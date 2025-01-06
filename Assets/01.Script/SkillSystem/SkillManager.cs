using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.SkillSystem
{
    public class SkillManager : MonoBehaviour
    {
        public List<Skill> startSkills = new List<Skill>();
        private static Dictionary<Type, Skill> _currentSkills = new Dictionary<Type, Skill>();
        
        public static void AddSkill<T>() where T : Skill, new()
        {
            var skill = new T();
            
            if (_currentSkills.TryAdd(typeof(T), skill))
            {
                Debug.Log($"Add Skill {{{skill.GetType().Name}}}");
            }
            else
            {
                Debug.Log($"Level Up Skill {{{skill.GetType().Name}}}");
            }
        }
        
        public static T GetSkill<T>() where T : Skill
        {
            if (_currentSkills.TryGetValue(typeof(T), out Skill skill))
            {
                return skill as T;
            }
            
            return null;
        }

        private void Update()
        {
            foreach (var skill in _currentSkills)
            {
                if (skill.Value.useCoolTime)
                {
                    
                }
            }
        }
    }
}
