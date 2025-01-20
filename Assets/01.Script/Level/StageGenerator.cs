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
        [SerializeField] private ConfineCollider _confineColliderPrefab;
        private ConfineCollider _confineCollider;
        [SerializeField] private Transform _startPoint;

        private List<StagePice> _generatedStagePieceList;

        public event Action OnStageClearEvent;

        private void Awake()
        {
            Generate();
        }

        public void Generate()
        {
            _confineCollider = Instantiate(_confineColliderPrefab);
            _generatedStagePieceList = new List<StagePice>();
            
            foreach (var stagePieceData in _stageDataSO.stagePiceDatas)
            {
                int stageCount = stagePieceData.stagePices.Length;
                int randomStageIndex = Random.Range(0, stageCount);
                StagePice stagePiecePrefab = stagePieceData.stagePices[randomStageIndex];
                StagePice stagePiece = Instantiate(stagePiecePrefab, transform);
                
                Vector3 startPos;
                if (_generatedStagePieceList.Count != 0)
                    startPos = _generatedStagePieceList[^1].OutPoint;
                else
                    startPos = _startPoint.position;

                stagePiece.Init(startPos);
                _generatedStagePieceList.Add(stagePiece);
            }
            
            // Bound 가져오기
            Vector2 min = new Vector2(_generatedStagePieceList[0].InPoint.x, _generatedStagePieceList[0].InPoint.y-100);
            Vector2 max = new Vector2(_generatedStagePieceList[^1].InPoint.x, _generatedStagePieceList[^1].InPoint.y+100);
            
            _confineCollider.SetConfine(min, max);
        }

        public void Clear()
        {
            OnStageClearEvent?.Invoke();
        }
    }
}
