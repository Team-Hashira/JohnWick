using Hashira.Core;
using UnityEngine;

namespace Hashira.Stage
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        [SerializeField] private Stage _stage;
        private Stage _currentStage;

        public void GenerateStage()
        {
            _currentStage = Instantiate(_stage, transform);
            _currentStage.OnAllClearEvent.AddListener(() =>
            {
                ClearStage();
            });
        }

        public void ClearStage()
        {
            GameManager.Instance.StartCardSelec();
            Destroy(_currentStage.gameObject);
            Cost.AddCost(20);
        }
    }
}
