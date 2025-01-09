using System;
using System.Collections.Generic;
using Crogen.AttributeExtension;
using Hashira.SkillSystem.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.SkillSystem
{
    public class SkillManager : MonoBehaviour
    {
        private static readonly Dictionary<Type, Skill> _skills = new Dictionary<Type, Skill>();
        private static readonly Dictionary<Type, Skill> _currentSkills = new Dictionary<Type, Skill>();

        private void Awake()
        {
            var skills = GetComponentsInChildren<Skill>();

            foreach (var skill in skills)
            {
                _skills.Add(skill.GetType(), skill);
            }
        }

        public static void AddSkill<T>() where T : Skill
        {
            Type type = typeof(T);
            AddSkill(type);
        }
        
        public static void AddSkill(Type type) 
        {
            if (_skills.TryGetValue(type, out var skill))
            {
                skill.currentCoolTime = skill.coolTime;
                _currentSkills.Add(type, skill);
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddSkill<MoveSpeedUp>();
            }
            
            foreach (var skillKeyValue in _currentSkills)
            {
                Skill skill = skillKeyValue.Value;
                if (skill.useCoolTime == false) return;
                
                if (skill.currentCoolTime >= skill.coolTime)
                {
                    //조건 이벤트를 달고 있을 스킬이 아니라면 
                    if (skill.useConditionalEvent == false)
                        skill.UseSkill(); //스킬 사용
                    //만약 조건 이벤트를 달고 있을 스킬이라면
                    else
                    //조건만 충조한다면 바로 실행할 수 있도록 준비
                        skill.IsReadyCoolTime = true;
                }
                else
                    skill.currentCoolTime += Time.deltaTime;
            }
        }
    }
}
