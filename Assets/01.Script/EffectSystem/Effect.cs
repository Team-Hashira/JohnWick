using Hashira.Entities;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public static int maxActiveCount = 1;

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
