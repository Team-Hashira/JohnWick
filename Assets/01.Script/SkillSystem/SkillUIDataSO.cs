using UnityEngine;

namespace Hashira.SkillSystem
{
    [CreateAssetMenu(menuName = "SO/SkillSystem/SkillUIDataSO")]
    public class SkillUIDataSO : ScriptableObject
    {
        public string skillName;
        public Sprite icon;
        [TextArea(5, 10)]
        public string description;

        private void OnEnable()
        {
            skillName = this.name;
        }
        
        private void OnValidate()
        {
            skillName = this.name;
        }
    }
}
