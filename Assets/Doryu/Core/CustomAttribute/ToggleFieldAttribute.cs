using System;
using UnityEngine;

namespace Doryu.CustomAttributes
{
    /// <summary>
    /// Visible based on value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToggleFieldAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }
        public int EnumValue { get; private set; }

        /// <summary>
        /// Shown when bool is true.
        /// </summary>
        /// <param name="boolProperty">BoolProperty variable name</param>
        public ToggleFieldAttribute(string boolProperty)
        {
            PropertyName = boolProperty;
        }
        /// <summary>
        /// Shown when enumProperty value equals enumValue.
        /// </summary>
        /// <param name="enumProperty">Enum variable name</param>
        /// <param name="enumValue">Enum value to show</param>
        public ToggleFieldAttribute(string enumProperty, int enumValue)
        {
            PropertyName = enumProperty;
            EnumValue = Convert.ToInt32(enumValue);
        }
    }
}