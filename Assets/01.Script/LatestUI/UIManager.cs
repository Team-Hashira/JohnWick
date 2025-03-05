using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach (UIBase uiBase in uiBases)
            {
                if (uiBase is IUserInterface ui)
                {
                    foreach (Type interfaceType in uiBase.GetType().GetInterfaces())
                    {
                        if (typeof(IUserInterface).IsAssignableFrom(interfaceType))
                        {
                            if (_uiDomainDict.TryGetValue(interfaceType, out var list))
                            {
                                if (list == null)
                                {
                                    string interfaceName = interfaceType.ToString();
                                    string domainName = interfaceName.Substring(1, interfaceName.IndexOf("UI"));
                                    Type domainType = Type.GetType($"{domainName}Domain");
                                    UIManagementDomain domain = Activator.CreateInstance(domainType) as UIManagementDomain;
                                    _uiDomainDict.Add(interfaceType, domain);
                                }
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

            foreach(var doamin in _uiDomainDict.Values)
            {
                doamin.UpdateUI();
            }
        }

        public T GetDomain<T>(Type interfaceType) where T : UIManagementDomain
        {
            return _uiDomainDict[interfaceType] as T;
        }
    }
}