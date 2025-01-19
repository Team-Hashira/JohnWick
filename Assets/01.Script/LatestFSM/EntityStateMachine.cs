using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Hashira.LatestFSM
{
    public class EntityStateMachine : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;

        [SerializeField]
        private string _namespace;
        [SerializeField]
        private List<StateSO> _stateList;
        [SerializeField]
        private StateSO _startState;

        private Dictionary<string, EntityState> _stateDictionary;
        private Dictionary<string, object> _shareVariableDict;

        public EntityState CurrentState { get; private set; }

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _stateDictionary = new Dictionary<string, EntityState>();
            _shareVariableDict = new Dictionary<string, object>();
            //StringBuilder보다 $""이 가벼움.
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach (var state in _stateList)
            //{
            //    stringBuilder.Append(_namespace);
            //    stringBuilder.Append(state.ToString());
            //    Type t = Type.GetType(stringBuilder.ToString());
            //    stringBuilder.Clear();
            //}

            foreach (var state in _stateList)
            {
                string className = $"{_namespace}{state.stateName}State";
                Type t = Type.GetType(className);
                if (t == null)
                {
                    t = Type.GetType($"{_namespace}.{state.stateName}State");
                    if (t == null)
                    {
                        Debug.LogError($"There is no State Class. {className}");
                        continue;
                    }
                    else
                    {
                        Debug.LogWarning($"{gameObject.name} : Add a '.' next to the last letter of the namespace.");
                    }

                    EntityState entityState = Activator.CreateInstance(t, entity, state) as EntityState;
                    _stateDictionary.Add(state.stateName, entityState);
                }
            }

            ChangeState(_startState.stateName);
        }

        public void ChangeState(string newState)
        {
            if (_stateDictionary.TryGetValue(newState, out EntityState entityState))
            {
                CurrentState?.OnExit();
                CurrentState = entityState;
                CurrentState.OnEnter();
            }
            else
            {
                Debug.LogError($"Fail to find {newState}.");
                return;
            }
        }

        public T GetShareVariable<T>(string key)
        {
            if (_shareVariableDict.ContainsKey(key))
            {
                return (T)_shareVariableDict[key];
            }
            return default(T);
        }

        public void SetShareVariable(string key, object value)
        {
            if (_shareVariableDict.ContainsKey(key))
            {
                _shareVariableDict[key] = value;
            }
            else
            {
                _shareVariableDict.Add(key, value);
            }
        }
    }
}
