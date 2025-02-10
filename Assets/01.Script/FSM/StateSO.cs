using Doryu.CustomAttributes;
using Hashira.Core.AnimationSystem;
using UnityEngine;

namespace Hashira.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string stateName;

        public bool ifClassNameIsDifferent = false;
        [ToggleField("ifClassNameIsDifferent")]
        public string className;

        public AnimatorParamSO animatorParam;
    }
}
