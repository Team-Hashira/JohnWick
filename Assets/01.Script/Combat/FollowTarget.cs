using UnityEngine;

namespace Hashira
{
    public class FollowTarget : MonoBehaviour
    {
        public Vector3 offset;
        public Transform target;

        [Header("Enable Axis")]
        public bool enableX = true;
        public bool enableY = true;
        public bool enableZ = true;

        private void LateUpdate()
        {
            Vector3 targetPos = target.position + offset;

            targetPos = new Vector3(
                enableX ? targetPos.x : transform.position.x, 
                enableY ? targetPos.y : transform.position.y, 
                enableZ ? targetPos.z : transform.position.z);
            
            transform.position = targetPos;
        }
    }
}