using UnityEngine;

namespace Hashira.Enemies.SiegeEnemy
{
    public class SiegeEnemy : Enemy, IPenetrable
    {
        [field: SerializeField]
        public int Resistivity { get; set; } = 3;
    }
}
