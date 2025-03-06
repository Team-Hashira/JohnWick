using Hashira.Entities;
using Hashira.LatestUI;
using Hashira.Players;
using Hashira.Stage;
using UnityEngine;

namespace Hashira.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private StageGenerator _stageGenerator;
        [SerializeField] private InputReaderSO _inputReader;

        private EntityMover _playerMover;

        private void Start()
        {
            _inputReader.PlayerActive(false);
            ToggleDomain toggleDomain = Hashira.LatestUI.UIManager.Instance.GetDomain<ToggleDomain>(typeof(IToggleUI));
            toggleDomain.OpenUI("SelectingCardPanel");
            _playerMover = PlayerManager.Instance.Player.GetEntityComponent<EntityMover>(true);
            _playerMover.SetGravity(false);
        }

        public void StartCardUse()
        {
            ToggleDomain toggleDomain = Hashira.LatestUI.UIManager.Instance.GetDomain<ToggleDomain>(typeof(IToggleUI));
            toggleDomain.OpenUI("CardUsingUI");
        }

        public void StartStage()
        {
            _inputReader.PlayerActive(true);
            _stageGenerator.GenerateStage();
            _playerMover.SetGravity(true);
        }
        public void ClearStage()
        {
            _inputReader.PlayerActive(false);
            _stageGenerator.ClearStage();
            _playerMover.SetGravity(false);
        }
    }
}
