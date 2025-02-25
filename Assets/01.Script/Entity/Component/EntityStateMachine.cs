using Hashira.Entities;
using Hashira.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityStateMachine : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private Entity _entity;

        [SerializeField]
        private List<StateSO> _stateList;
        [SerializeField]
        private StateSO _startState;

        private Dictionary<string, EntityState> _stateDictionary;
        private Dictionary<string, object> _shareVariableDict;

        public EntityState CurrentState { get; private set; }
        public string CurrentStateName { get; private set; }

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
                CurrentStateName = newState;
                CurrentState.OnEnter();
            }
            else
            {
                Debug.LogError($"Fail to find {newState}.");
                return;
            }
        }

        /// <summary>
        /// 한프레임 쉬고 ChangeState를 호출해주는 함수입니다.
        /// </summary>
        /// <param name="newState"></param>
        public void DelayedChangeState(string newState)
        {
            IEnumerator DelayCoroutine()
            {
                yield return null;
                ChangeState(newState);
            }
            StartCoroutine(DelayCoroutine());
        }


        public T GetShareVariable<T>(string key)
        {
            if (_shareVariableDict.ContainsKey(key))
            {
                return (T)_shareVariableDict[key];
            }
            return default(T);
        }

        public bool TryGetShareVariable<T>(string key, out T variable)
        {
            variable = default(T);
            if (_shareVariableDict.ContainsKey(key))
            {
                variable = (T)_shareVariableDict[key];
                return true;
            }
            return false;
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
                string stateName = state.ifClassNameIsDifferent ? state.className : state.stateName;
                string className = $"{_entity.GetType().FullName}{stateName}State";
                try
                {
                    Type t = Type.GetType(className);
                    EntityState entityState = Activator.CreateInstance(t, _entity, state) as EntityState;
                    _stateDictionary.Add(state.stateName, entityState);
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                        Debug.LogError($"Fail to Create State class({state.stateName}, {className}). : {ex.InnerException.Message}\nStackTrace : {ex.InnerException.StackTrace}");
                    else
                        Debug.LogError($"Fail to Create State class({state.stateName}, {className}). : {ex.Message}");
                }
            }
            ChangeState(_startState.stateName);
        }
    }
}
