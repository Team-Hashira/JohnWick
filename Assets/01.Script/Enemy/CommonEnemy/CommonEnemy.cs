using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemy : Enemy, IPenetrable
    {
        [field: SerializeField] public int Resistivity { get; set; } = 3; // KDR
    }
}
