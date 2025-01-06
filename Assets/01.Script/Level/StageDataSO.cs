using UnityEngine;

namespace Hashira
{
    [System.Serializable]
    public struct StagePiceData
    {
        [Header("StageRandomPice")]
        public StagePice[] stagePices;
    }

    [CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
    public class StageDataSO : ScriptableObject
    {
        public string stageName;
        public StagePiceData[] stagePiceDatas;
    }
}
