using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira
{
    public class StageGenerator : MonoBehaviour
    {
        [SerializeField] private StageDataSO _stageDataSO;

        [SerializeField] private Transform _startPoint;

        private List<StagePice> _generatedStagePiceList;

        public event Action OnStageClearEvent;

        private void Awake()
        {
            Generate();
        }

        public void Generate()
        {
            _generatedStagePiceList = new List<StagePice>();
            foreach (StagePiceData stagePiceData in _stageDataSO.stagePiceDatas)
            {
                int stageCount = stagePiceData.stagePices.Length;
                int randomStageIndex = Random.Range(0, stageCount);
                StagePice stagePicePrefab = stagePiceData.stagePices[randomStageIndex];
                StagePice stagePice = Instantiate(stagePicePrefab, transform);

                Vector3 startPos;
                if (_generatedStagePiceList.Count != 0)
                    startPos = _generatedStagePiceList[^1].OutPoint;
                else
                    startPos = _startPoint.position;

                stagePice.Init(startPos);
                _generatedStagePiceList.Add(stagePice);
            }
        }

        public void Clear()
        {
            OnStageClearEvent?.Invoke();
        }
    }
}
