#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonAttributeEditor : Editor
{
    private MonoBehaviour targetObject;

    public void OnEnable()
    {
        targetObject = (MonoBehaviour) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MethodInfo[] methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            ButtonAttribute attribute = Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

            if (attribute != null)
            {
                if (GUILayout.Button(attribute.MethodName))
                {
                    method.Invoke(targetObject, null);
                }
            }
        }
    }
}
#endif
