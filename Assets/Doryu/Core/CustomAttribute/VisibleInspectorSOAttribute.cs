using System;
using UnityEngine;

namespace Doryu.CustomAttributes
{
    /// <summary>
    /// Enables SO inspector view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class VisibleInspectorSOAttribute : PropertyAttribute
    {
        public VisibleInspectorSOAttribute() { }
    }
}
