using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class EditorButtonAttribute : PropertyAttribute
{
    public string Label;

    public EditorButtonAttribute(string label = null)
    {
        Label = label;
    }
}