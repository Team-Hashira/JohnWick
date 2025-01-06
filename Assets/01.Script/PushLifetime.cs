using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira
{
    public class PushLifetime : Lifetime, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        public override void DelayDie()
        {
            OnPush();
        }

        public void OnPop()
        {
            Spawn();
            _isDead = false;
        }

        public void OnPush()
        {

        }
    }
}
