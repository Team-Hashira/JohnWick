using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(menuName = "SO/Chapter")]
    public class ChapterSO : ScriptableObject
    {
        public StageDataSO[] stages;
        
        public int Length => stages.Length;
        public StageDataSO this[int index] => stages[index];
    }
}