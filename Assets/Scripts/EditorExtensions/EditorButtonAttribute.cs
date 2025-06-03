using System;
using UnityEngine;

namespace EditorExtensions
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class EditorButtonAttribute : PropertyAttribute
    {
        public string Label;

        public EditorButtonAttribute(string label = null)
        {
            Label = label;
        }
    }
}