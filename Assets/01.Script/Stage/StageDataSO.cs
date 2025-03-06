using UnityEngine;

namespace Hashira.Stage
{
    [System.Serializable]
    public struct StagePiceData
    {
        [Header("StageRandomPice")]
        public Stage[] stagePices;
    }

    [CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
    public class StageDataSO : ScriptableObject
    {
        public string stageName;
        public StagePiceData[] stagePiceDatas;
    }
}
