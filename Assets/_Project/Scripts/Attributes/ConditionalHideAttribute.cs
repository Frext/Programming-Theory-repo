using System;
using UnityEngine;

namespace Attributes
{
    // The main purpose of this class is to provide additional data that will be used within the PropertyDrawer.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        // The name of the bool field that will be in control
        public string ConditionalSourceField;

        public ConditionalHideAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
    }
}