using System;
using UnityEngine;

namespace Hashira.Stage
{
    public class StageClearFlag : MonoBehaviour
    {
        // 일단은 충돌로만 체크
        //[SerializeField] private bool _UseCollision;
        private void OnCollisionEnter2D(Collision2D other)
        {
            StageGenerator.Instance.Clear();
        }
    }
}
