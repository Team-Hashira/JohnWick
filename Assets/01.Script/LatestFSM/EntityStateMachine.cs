using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

namespace Hashira.LatestFSM
{
    public class EntityStateMachine : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
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
        }

        private void Update()
        {
            CurrentState.OnUpdate();
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

        public void AfterInit()
        {
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
                try
                {
                    string className = $"{_entity.GetType().FullName}{state.stateName}State";
                    Type t = Type.GetType(className);
                    EntityState entityState = Activator.CreateInstance(t, _entity, state) as EntityState;
                    _stateDictionary.Add(state.stateName, entityState);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Fail to Create State class({state.stateName}). : {ex.Message}");
                }
            }

            ChangeState(_startState.stateName);
        }
    }
}
