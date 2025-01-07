using System;
using System.Collections;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.SkillSystem.Skills
{
    public class MoveSpeedUpSkill : Skill
    {
        private Player _player;
        private StatElement _statElement;
        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _statElement = _player.GetEntityComponent<EntityStat>().GetElement("Speed");
        }

        public override void UseSkill()
        {
            base.UseSkill();
           Debug.Log("Use Skill");
            _statElement.AddModify("MoveSpeedUpSkill", 100, EModifyMode.Add);
            StartCoroutine(CoroutineEndSkill());
        }

        private IEnumerator CoroutineEndSkill()
        {
            yield return new WaitForSeconds(10);
            _statElement.RemoveModify("MoveSpeedUpSkill", EModifyMode.Add);

            
        }
    }
}