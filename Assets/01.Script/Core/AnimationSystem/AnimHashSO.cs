using UnityEngine;

namespace Hashira
{
    public class AnimHashSO : MonoBehaviour
    {
        public string paramName;
        public int hash;

        private void OnValidate()
        {
            hash = Animator.StringToHash(paramName);
        }
    }
}
