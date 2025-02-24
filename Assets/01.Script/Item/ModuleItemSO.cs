using UnityEditor;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "Module", menuName = "SO/Item/Module")]
    public class ModuleItemSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}
