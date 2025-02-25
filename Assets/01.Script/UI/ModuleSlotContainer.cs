using Hashira.Entities.Components;
using Hashira.Items.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.UI
{
    public class ModuleSlotContainer : MonoBehaviour
    {
        [SerializeField] private ModuleSlot _moduleSlotPrefab;
        [SerializeField] private float _range = 500f;
        private List<ModuleSlot> _slotList = new List<ModuleSlot>();

        private PlayerModule _playerModule;

        private void OnEnable()
        {
            LoadModules();
        }

        private void Start()
        {
            _playerModule = GameManager.Instance.Player.GetEntityComponent<PlayerModule>();
        }

        private void OnDisable()
        {
            RemoveModuleSlots();
        }

        private void LoadModules()
        {
            if (_playerModule == null) return;
            List<Module> list = _playerModule.ListModules;
            Dictionary<Type, int> moduleAndCountDict = new Dictionary<Type, int>();

            foreach (var module in list)
                if (moduleAndCountDict.TryAdd(module.GetType(), 1) == false)
                    moduleAndCountDict[module.GetType()]++;

            foreach (var moduleAndCount in moduleAndCountDict)
                Debug.Log(moduleAndCount.Key.ToString() + " " + moduleAndCount.Value.ToString());


                int i = 0;
            foreach (var moduleAndCount in moduleAndCountDict)
            {
                var moduleSlot = Instantiate(_moduleSlotPrefab, transform);
                float value = (i / (float)list.Count - 1) * 2 * Mathf.PI;
                (moduleSlot.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Sin(value), Mathf.Cos(value)) * _range;

                moduleSlot.Init(list[i].ItemSO as ModuleSO, moduleAndCount.Value);
                _slotList.Add(moduleSlot);
                i++;
            }
        }

        private void RemoveModuleSlots()
        {
            for (int i = 0; i < _slotList.Count; i++)
            {
                Destroy(_slotList[i]);
            }

            _slotList.Clear();
        }
    }
}
