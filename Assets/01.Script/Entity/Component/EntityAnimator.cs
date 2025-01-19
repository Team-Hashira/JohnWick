using UnityEngine;

namespace Hashira
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }

        
    }
}
