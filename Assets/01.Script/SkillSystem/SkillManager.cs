using System;
using System.Collections.Generic;
using Crogen.AttributeExtension;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.SkillSystem
{
    public class SkillManager : MonoBehaviour
    {
        [FormerlySerializedAs("startSkills")] public List<string> startSkillNames;
        private static readonly Dictionary<Type, Skill> _currentSkills = new Dictionary<Type, Skill>();

#if UNITY_EDITOR
        [Space(25)]
        [SerializeField] private string _debugSkillName = "new skill";
        [Button]
        private void AddSkillForDebug()
        {
            AddSkill(Type.GetType(_debugSkillName));
        }
#endif
        private void Awake()
        {
            foreach (var startSkillName in startSkillNames)
            {
                Type t = Type.GetType(startSkillName);
                AddSkill(t);
            }
        }


        public static void AddSkill(Type type) 
        {
            var skill = Activator.CreateInstance(type) as Skill;
            
            if(skill == null)
                Debug.LogError($"Could not create skill {type}");
            
            if (_currentSkills.TryAdd(type, skill))
            {
                Debug.Log($"Add Skill {{{skill.GetType().Name}}}");
            }
            else
            {
                Debug.Log($"Level Up Skill {{{skill.GetType().Name}}}");
            }
        }
        
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
