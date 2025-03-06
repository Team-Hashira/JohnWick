using Hashira.Core;
using System;
using UnityEngine;

namespace Hashira.Stage
{
    public class StageGenerator : MonoSingleton<StageGenerator>
    {
        [SerializeField] private StageEventer _stage;
        private StageEventer _currentStage;

        public void GenerateStage()
        {
            _currentStage = Instantiate(_stage, transform);
            _currentStage.AllClearEvent.AddListener(GameManager.Instance.)
        }

        public void ClearStage()
        {
            Destroy(_currentStage);
        }
    }
}
