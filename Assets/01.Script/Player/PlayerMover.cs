using System;
using System.Collections;
using Hashira.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public void UnderJump(bool isUnderJump)
        {
            StartCoroutine(UnderJumpCoroutine(isUnderJump));
        }

        private IEnumerator UnderJumpCoroutine(bool isUnderJump)
        {
            Debug.Log("Under Jump");
            yield return new WaitForSeconds(0.2f);
            
            foreach (var platformEffector in FindObjectsByType<PlatformEffector2D>(FindObjectsSortMode.None))
            {
                platformEffector.rotationalOffset = isUnderJump ? 180 : 0;
            }
        }
    }
}