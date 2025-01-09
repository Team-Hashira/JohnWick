using System;
using UnityEngine;

namespace Hashira.EffectSystem
{
    [CreateAssetMenu(menuName = "SO/EffectSystem/EffectUIDataSO")]
    public class EffectUIDataSO : ScriptableObject
    {
        public string effectName;
        public Sprite icon;
        [TextArea(5, 10)]
        public string description;

        private void OnEnable()
        {
            effectName = this.name;
        }
        
        private void OnValidate()
        {
            effectName = this.name;
        }
    }
}
