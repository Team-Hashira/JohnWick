using UnityEngine;

namespace Hashira.Projectile
{
    public abstract class ProjectileCollider2D : MonoBehaviour
    {
        public abstract bool CheckCollision(LayerMask whatIsTarget, out RaycastHit2D[] hits, Vector2 moveTo = default);
    }
}