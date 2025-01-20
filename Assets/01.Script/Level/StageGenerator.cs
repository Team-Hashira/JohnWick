using System;
using System.Collections.Generic;
using Hashira.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira
{
    public class StageGenerator : MonoBehaviour
    {
        [SerializeField] private StageDataSO _stageDataSO;
        [SerializeField] private ConfineCollider confineColliderPrefab;
        [SerializeField] private Transform _startPoint;

        private List<StagePice> _generatedStagePieceList;

        public event Action OnStageClearEvent;

        private void Awake()
        {
            Generate();
        }

        public void Generate()
        {
            _generatedStagePieceList = new List<StagePice>();
            foreach (StagePiceData stagePiceData in _stageDataSO.stagePiceDatas)
            {
                int stageCount = stagePiceData.stagePices.Length;
                int randomStageIndex = Random.Range(0, stageCount);
                StagePice stagePiecePrefab = stagePiceData.stagePices[randomStageIndex];
                StagePice stagePiece = Instantiate(stagePiecePrefab, transform);

                Vector3 startPos;
                if (_generatedStagePieceList.Count != 0)
                    startPos = _generatedStagePieceList[^1].OutPoint;
                else
                    startPos = _startPoint.position;

                stagePiece.Init(startPos);
                _generatedStagePieceList.Add(stagePiece);
            }
        }

        public void Clear()
        {
            OnStageClearEvent?.Invoke();
        }
    }
}
