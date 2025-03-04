using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public virtual int MaxActiveCount { get; private set; } = 1;

        public EffectUIDataSO effectUIDataSO;
        public EntityEffector entityEffector;
		public EntityStat entityStat;

        public virtual void Enable()
        {
		}

        public virtual void Update()
        {
        }

        public virtual void Disable()
        {
        }
    }
}
