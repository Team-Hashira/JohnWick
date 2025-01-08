using UnityEngine;

namespace Hashira.EffectSystem
{
    [CreateAssetMenu(menuName = "SO/EffectSystem/EffectDataSO")]
    public class EffectDataSO : ScriptableObject
    {
        public Sprite icon;
        [TextArea(5, 10)]
        public string description;
        public float duration;
    }
}
