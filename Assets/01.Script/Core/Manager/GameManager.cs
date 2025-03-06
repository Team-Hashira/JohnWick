using Hashira.Stage;
using UnityEngine;

namespace Hashira. Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private StageGenerator _stageGenerator;

        public void StartStage()
        {
            _stageGenerator.GenerateStage();
        }
        public void ClearStage()
        {
            _stageGenerator.ClearStage();
        }
    }
}
