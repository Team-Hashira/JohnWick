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
        private ToggleDomain _toggleDomain;

        private void Start()
        {
            _toggleDomain = Hashira.LatestUI.UIManager.Instance.GetDomain<ToggleDomain>(typeof(IToggleUI));
            _playerMover = PlayerManager.Instance.Player.GetEntityComponent<EntityMover>(true);
            StartCardSelec();
        }

        public void StartCardUse()
        {
            _toggleDomain.OpenUI("CardUsingUI");
        }
        public void StartCardSelec()
        {
            _inputReader.PlayerActive(false);
            _toggleDomain.OpenUI("RewardCardPanel");
            _playerMover.SetGravity(false);
            PlayerManager.Instance.Player.transform.position = new Vector3(0, 10000, 0);
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
            PlayerManager.Instance.Player.transform.position = new Vector3(0, 10000, 0);
        }
    }
}
