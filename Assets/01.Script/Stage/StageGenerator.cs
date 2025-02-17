using Crogen.CrogenPooling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Stage
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        [SerializeField] private ChapterSO[] _chapterSO;
        [SerializeField] private StageConfineCollider _confineColliderPrefab;
        private StageConfineCollider _confineCollider;
        [SerializeField] private Transform _startPoint;

        private List<StagePiece> _generatedStagePieceList;
        private List<IPoolingObject> _stagePoolingObjectList = new List<IPoolingObject>();

        private int _currentChapterIndex;
        private int _currentStageIndex;
        public bool IsMovingStage { get; private set; }

        public int CurrentChapterIndex
        {
            get => _currentChapterIndex;
            private set
            {
                if (value >= _chapterSO.Length)
                {
                    OnAllChaptersClearEvent?.Invoke();
                    TimeManager.SetTimeScale(0);
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
            _generatedStagePieceList = new List<StagePiece>();
            
            foreach (var stagePieceData in stageDataSO.stagePiceDatas)
            {
                int stageCount = stagePieceData.stagePices.Length;
                int randomStageIndex = Random.Range(0, stageCount);
                StagePiece stagePiecePrefab = stagePieceData.stagePices[randomStageIndex];
                StagePiece stagePiece = Instantiate(stagePiecePrefab, transform);
                
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
            
            _confineCollider = Instantiate(_confineColliderPrefab);
            _confineCollider.SetConfine(min, max);
            _generatedStagePieceList[0].SetPlayerPosToSpawnPoint();
        }

        private void NextStage()
        {
            ++CurrentStageIndex;
            CameraManager.Instance.MoveToPlayerPositionimmediately();
            DestroyAllPoolingObjects();
			Generate(_chapterSO[CurrentChapterIndex][CurrentStageIndex]);
        }

        public void AddPoolingObject(IPoolingObject poolingObject)
        {
            _stagePoolingObjectList.Add(poolingObject);
        }

        private void DestroyAllPoolingObjects()
        {
            foreach (var poolingObject in _stagePoolingObjectList)
            {
                poolingObject.Push();
            }

            _stagePoolingObjectList.Clear();
        }
        
        public async void Clear()
        {
            if (IsMovingStage == true) return;
            TimeManager.SetTimeScale(0);
            FadeController.Fade(true);
            IsMovingStage = true;
            OnStageClearEvent?.Invoke();
            
            // TODO Screen Fade
            await Task.Delay(2000);
            
            // All Destroy
            foreach (var stagePiece in _generatedStagePieceList)
                Destroy(stagePiece.gameObject);
            Destroy(_confineCollider.gameObject);
            
            NextStage();
            await Task.Delay(2000);
            IsMovingStage = false;
            
            FadeController.Fade(false);
            TimeManager.UndoTimeScale();

        }
    }
}
