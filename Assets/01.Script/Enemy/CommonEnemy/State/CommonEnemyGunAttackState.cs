using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyGunAttackState : EntityState
    {
        public CommonEnemyGunAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }
        
    }
}
