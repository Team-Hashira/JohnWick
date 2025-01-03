using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.FSM
{
    public class StateMachine
    {
        public Entity _owner;

        private Dictionary<Enum, EntityStateBase> _stateDictionary;
        public Enum CurrentStateEnum { get; private set; }

        public StateMachine(Entity owner)
        {
            _owner = owner;

            _stateDictionary = new Dictionary<Enum, EntityStateBase>();

            string unitName = _owner.name;
            Type unitStateEnumType = Type.GetType("E" + unitName + "State");

            if (unitStateEnumType == null)
            {
                Debug.LogError($"[ E{unitName}State ] enum is not found");
                return;
            }

            foreach (Enum stateEnum in Enum.GetValues(unitStateEnumType))
            {
                string enumName = stateEnum.ToString();
                Type unitState = Type.GetType(unitName + enumName + "State");
                try
                {
                    EntityStateBase state = Activator.CreateInstance(unitState, _owner, this, enumName) as EntityStateBase;
                    _stateDictionary.Add(stateEnum, state);
                    if (CurrentStateEnum == null)
                        CurrentStateEnum = stateEnum;
                }
                catch (Exception e)
                {
                    Debug.LogError($"[ {unitName + stateEnum.ToString()}State ] class is not found\n" +
                                    $"Exception : {e.ToString()}");
                }
            }

            _stateDictionary[CurrentStateEnum].Enter();
        }

        public void UpdateMachine()
        {
            _stateDictionary[CurrentStateEnum].Update();
        }

        public void ChangeState(Enum stateEnum)
        {
            _stateDictionary[CurrentStateEnum].Exit();
            CurrentStateEnum = stateEnum;
            _stateDictionary[CurrentStateEnum].Enter();
        }
    }
}