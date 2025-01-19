using UnityEngine;

namespace Hashira
{
    public class AnimatorParamSO : MonoBehaviour
    {
        public string paramName;
        public int hash;

        private void OnValidate()
        {
            hash = Animator.StringToHash(paramName);
        }
    }
}
