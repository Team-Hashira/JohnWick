using Hashira.Entities;
using System.Collections;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        [SerializeField] private LayerMask _oneWayPlatform;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _whatIsGround += _oneWayPlatform;
        }

        public void UnderJump(bool isUnderJump)
        {
            int layerCheck = _whatIsGround & _oneWayPlatform;
            if (layerCheck != 0 && isUnderJump)
                _whatIsGround -= _oneWayPlatform;
            else if (isUnderJump == false)
                _whatIsGround += _oneWayPlatform;

            foreach (var platformEffector in FindObjectsByType<PlatformEffector2D>(FindObjectsSortMode.None))
            {
                platformEffector.rotationalOffset = isUnderJump ? 180 : 0;
            }
        }

        private IEnumerator UnderJumpCoroutine(bool isUnderJump)
        {
            Debug.Log("Under Jump");

            _whatIsGround -= _oneWayPlatform;

            yield return new WaitForSeconds(0.2f);

            _whatIsGround += _oneWayPlatform;

            foreach (var platformEffector in FindObjectsByType<PlatformEffector2D>(FindObjectsSortMode.None))
            {
                platformEffector.rotationalOffset = isUnderJump ? 180 : 0;
            }
        }
    }
}