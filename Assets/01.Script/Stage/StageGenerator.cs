using Hashira.Core;
using System;
using UnityEngine;

namespace Hashira.Stage
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        public static int currentFloor = 0;
        public int CurrentStage { get; private set; } = 0;

        [SerializeField] private FloorSO[] floors;
        private Stage _currentStage;

        public event Action OnFloorUpEvent;
        public event Action OnNextStageEvent;

        public Stage GetCurrentStage() => _currentStage;

        public void GenerateStage()
        {
            _currentStage = Instantiate(floors[currentFloor][CurrentStage].GetRandomStage(), transform);
            _currentStage.OnAllClearEvent.AddListener(() =>
            {
                ClearStage();
            });
        }

        public void ClearStage()
        {
            ++CurrentStage;
            if (CurrentStage >= floors[CurrentStage].stages.Length)
            {
                OnFloorUpEvent?.Invoke();
                CurrentStage = 0;
            }

            OnNextStageEvent?.Invoke();
            GameManager.Instance.StartCardSelec();
            Cost.AddCost(20);
            Destroy(_currentStage.gameObject);
        }
    }
}
