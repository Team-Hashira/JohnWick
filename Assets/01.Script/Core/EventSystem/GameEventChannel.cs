using System.Collections.Generic;
using System;
using UnityEngine;

namespace Hashira.Core.EventSystem
{
    public class GameEvent
    {
    }

    [CreateAssetMenu(fileName = "GameEventChannelSO", menuName = "SO/Events/GameEventChannelSO")]
    public class GameEventChannelSO : ScriptableObject
    {
        private Dictionary<Type, Action<GameEvent>> _eventDictionary = new();
        private Dictionary<Delegate, Action<GameEvent>> _lookUpTable = new();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUpTable.ContainsKey(handler) == false)
            {
                Action<GameEvent> castHandler = (evt) => handler(evt as T);
                _lookUpTable[handler] = castHandler;

                Type evtType = typeof(T);
                if (_eventDictionary.ContainsKey(evtType))
                {
                    _eventDictionary[evtType] += castHandler;
                }
                else
                {
                    _eventDictionary[evtType] = castHandler;
                }
            }
            else
            {
                Debug.LogError("존재하지 않는 Event.");
            }
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lookUpTable.TryGetValue(handler, out Action<GameEvent> action))
            {
                if (_eventDictionary.TryGetValue(evtType, out Action<GameEvent> internalAction))
                {
                    internalAction -= action;
                    if (internalAction == null)
                        _eventDictionary.Remove(evtType);
                    else
                        _eventDictionary[evtType] = internalAction;
                }
                _lookUpTable.Remove(handler);
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_eventDictionary.TryGetValue(evt.GetType(), out Action<GameEvent> handlers))
            {
                handlers?.Invoke(evt);
            }
        }

        public void Clear()
        {
            _eventDictionary.Clear();
            _lookUpTable.Clear();
        }
    }
}
