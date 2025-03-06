using System;
using UnityEngine;

namespace Hashira.Stage
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        [SerializeField] private GameObject _stage;
        private GameObject _currentStage;

        public void GenerateStage()
        {
            _currentStage = Instantiate(_stage, transform);
        }

        public void ClearStage()
        {
            Destroy(_currentStage);
        }
    }
}
