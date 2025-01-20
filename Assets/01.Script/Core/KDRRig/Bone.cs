using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Hashira.Rigging
{
    public class Bone : MonoBehaviour
    {
        public Bone nextBone;
        public float length;

        private void Awake()
        {
            if (nextBone == null) return;
            length = Vector3.Distance(transform.position, nextBone.transform.position);
        }

        private void Reset()
        {
            bool isFirst = true;
            foreach (Bone bone in transform.GetComponentsInChildren<Bone>())
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                nextBone = bone;
                break;
            }
        }
    }
}
