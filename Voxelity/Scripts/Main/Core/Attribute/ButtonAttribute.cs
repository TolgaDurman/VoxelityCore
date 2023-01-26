using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public string MethodName { get; }
    public ButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }
}