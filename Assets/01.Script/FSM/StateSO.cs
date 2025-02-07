using Hashira.Core.AnimationSystem;
using UnityEngine;

namespace Hashira.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public AnimatorParamSO animatorParam;
    }
}
