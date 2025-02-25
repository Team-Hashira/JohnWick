using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.Module;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.UI
{
    public class ModuleSlotContainer : MonoBehaviour
    {
        [SerializeField] private ModuleSlot _moduleSlotPrefab;
        [SerializeField] private float _range = 500f;

        private PlayerModule _playerModule;

        private void OnEnable()
        {
            _playerModule ??= GameManager.Instance.Player.GetEntityComponent<PlayerModule>();

            LoadModules();
        }

        private void LoadModules()
        {
            List<Module> list = _playerModule.ListModules;

            for (int i = 0; i < list.Count; i++)
            {
                var moduleSlot = Instantiate(_moduleSlotPrefab, transform);
                float value = i / (float)list.Count * 2 * Mathf.PI;
                (moduleSlot.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Sin(value), Mathf.Cos(value)) * _range;

                moduleSlot.Init(list[i].ItemSO as ModuleSO);
            }
        }
    }
}
