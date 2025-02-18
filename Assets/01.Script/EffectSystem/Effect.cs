using Hashira.Entities;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public int level;

        public EntityEffector baseEffector;
        public EffectUIDataSO effectUIDataSO;
        public Entity entity;
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
