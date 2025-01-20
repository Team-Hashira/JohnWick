using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hashira.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        [SerializeField] private ChapterSO[] _chapterSO;
        [SerializeField] private ConfineCollider _confineColliderPrefab;
        private ConfineCollider _confineCollider;
        [SerializeField] private Transform _startPoint;

        private List<StagePice> _generatedStagePieceList;
        private int _currentChapterIndex;
        private int _currentStageIndex;

        public int CurrentChapterIndex
        {
            get => _currentChapterIndex;
            private set
            {
                if (value >= _chapterSO.Length)
                {
                    OnAllChaptersClearEvent?.Invoke();
                    TimeManager.Pause();
                }
                _currentChapterIndex = value;
            }
        }

        public int CurrentStageIndex
        {
            get => _currentStageIndex;
            private set
            {

                if (value >= _chapterSO[CurrentChapterIndex].Length)
                {
                    _currentStageIndex = 0;
                    ++CurrentChapterIndex;
                    return;
                }
                _currentStageIndex = value;
            }
        }

        public event Action OnStageClearEvent;
        public event Action OnAllChaptersClearEvent;
        private void Awake()
        {
            Generate(_chapterSO[CurrentChapterIndex][CurrentStageIndex]);
        }

        private void Generate(StageDataSO stageDataSO)
        {
            _confineCollider = Instantiate(_confineColliderPrefab);
            _generatedStagePieceList = new List<StagePice>();
            
            foreach (var stagePieceData in stageDataSO.stagePiceDatas)
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
            Vector2 max = new Vector2(_generatedStagePieceList[^1].OutPoint.x, _generatedStagePieceList[^1].OutPoint.y+100);
            
            _confineCollider.SetConfine(min, max);
        }

        private void NextStage()
        {
            try
            {
                // 다음 스테이지
                Generate(_chapterSO[CurrentChapterIndex][++CurrentStageIndex]);
            }
            catch (Exception e)
            {
                // 다음 챕터
                CurrentChapterIndex++;
                CurrentStageIndex=0;
                Generate(_chapterSO[CurrentChapterIndex][CurrentStageIndex]);
            }
        }
        
        public async void Clear()
        {
            OnStageClearEvent?.Invoke();
            await Task.Delay(3000);
            NextStage();
        }
    }
}
