using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.PerkSystem
{
    [CreateAssetMenu(menuName = "SO/PerkSystem/PerkUIDataSO")]
    public class PerkUIDataSO : ScriptableObject
    {
        public string perkName;
        public Sprite icon;
        [TextArea(5, 10)]
        public string description;

        private void OnEnable()
        {
            perkName = this.name;
        }
        
        private void OnValidate()
        {
            perkName = this.name;
        }
    }
}
