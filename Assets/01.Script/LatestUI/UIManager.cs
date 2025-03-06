using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.LatestUI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private List<UIBase> _uiBaseList;

        private Dictionary<Type, UIManagementDomain> _uiDomainDict;

        public static Vector2 MousePosition;
        public static Vector2 WorldMousePosition;

        private void Awake()
        {
            _uiBaseList = new List<UIBase>();
            _uiDomainDict = new Dictionary<Type, UIManagementDomain>();

            UIBase[] uiBases = GameObject.FindObjectsByType<UIBase>(FindObjectsSortMode.None);
            _uiBaseList = uiBases.ToList();

            foreach (UIBase uiBase in _uiBaseList)
            {
                if (uiBase is IUserInterface ui)
                {
                    foreach (Type interfaceType in uiBase.GetType().GetInterfaces())
                    {
                        if (typeof(IUserInterface).IsAssignableFrom(interfaceType))
                        {
                            if (interfaceType == typeof(IUserInterface))
                                continue;
                            if (_uiDomainDict.TryGetValue(interfaceType, out var list))
                            {
                                _uiDomainDict[interfaceType].AddUI(ui);
                            }
                            else
                            {
                                string interfaceName = interfaceType.ToString();
                                int startIndex = interfaceName.IndexOf('I', 16);

                                string domainName = 
                                    interfaceName.Substring(startIndex + 1, interfaceName.IndexOf("UI", startIndex) - startIndex - 1);
                                Type domainType = Type.GetType($"{GetType().Namespace}.{domainName}Domain");
                                UIManagementDomain domain = Activator.CreateInstance(domainType) as UIManagementDomain;
                                _uiDomainDict.Add(interfaceType, domain);
                                _uiDomainDict[interfaceType].AddUI(ui);
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            MousePosition = Mouse.current.position.value;
            WorldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            foreach (var doamin in _uiDomainDict.Values)
            {
                doamin.UpdateUI();
            }
        }

        public T GetDomain<T>(Type interfaceType) where T : UIManagementDomain
        {
            return _uiDomainDict[interfaceType] as T;
        }

        public void AddUI(UIBase uiBase)
        {
            if (uiBase is IUserInterface ui)
            {
                foreach (Type interfaceType in uiBase.GetType().GetInterfaces())
                {
                    if (typeof(IUserInterface).IsAssignableFrom(interfaceType))
                    {
                        if (interfaceType == typeof(IUserInterface))
                            continue;
                        if (_uiDomainDict.TryGetValue(interfaceType, out var list))
                        {
                            _uiDomainDict[interfaceType].AddUI(ui);
                        }
                    }
                }
            }
        }
    }
}