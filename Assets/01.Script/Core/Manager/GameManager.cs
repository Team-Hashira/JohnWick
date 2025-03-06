using Hashira.Players;
using Hashira.Stage;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira. Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private StageGenerator _stageGenerator;

        public void StartStage()
        {
            _stageGenerator.GenerateStage();
        }
    }
}
