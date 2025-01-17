using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using TMPro;
using UnityEngine;

namespace Hashira.UI.StatusWindow.StatusPanel
{
    public class StatusContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusNameText;
        [SerializeField] private TextMeshProUGUI _statusValueText;
        
        private Player _player;
        private EntityStat _entityStat;
        
        private StatElement[] _statElement;

        private bool _isInit;
        
        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _entityStat = _player.GetEntityComponent<EntityStat>();
            _isInit = true;
        }

        private void OnEnable()
        {
            if (_isInit == false) return;
            StatElement[] statElements = _entityStat.StatDictionary.GetElements();
            _statusNameText.text = string.Empty;
            _statusValueText.text = string.Empty;
            
            foreach (var statElement in statElements)
            {
                _statusNameText.text += $"{statElement.elementSO.displayName}\n";
                _statusValueText.text += $"{statElement.Value}\n";
            }
        }
    }
}
