using Crogen.CrogenPooling;
using Hashira.Enemies;
using System.Collections;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class MeteorEffect : Effect
    {
        private readonly float _delayTime = 8f;
        private float _curTime = 0f;
        public override int MaxActiveCount => -1;

        public override void Disable()
        {
            base.Disable();
        }

        public override void Enable()
        {
            base.Enable();
        }

        public override void Update()
        {
            base.Update();
            _curTime += Time.deltaTime;
            if (_delayTime < _curTime)
            {
                _curTime = 0;
                CreateMeteor();
                CreateMeteor();
            }
        }

        private void CreateMeteor()
        {
            Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(sortMode: FindObjectsSortMode.None);

            PopCore.Pop(EffectPoolType.LightningVFX, enemies[Random.Range(0, enemies.Length)].transform.position, Quaternion.identity);
        }
    }
}
