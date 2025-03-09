using UnityEngine;

namespace Hashira.Stage
{
    [CreateAssetMenu(menuName = "SO/Floor")]
    public class FloorSO : ScriptableObject
    {
        public StageDataSO[] stages;
        
        public int Length => stages.Length;
        public StageDataSO this[int index] => stages[index];
    }
}
