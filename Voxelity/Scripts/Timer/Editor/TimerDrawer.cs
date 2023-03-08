using UnityEditor;
using UnityEngine;

namespace Voxelity.Timers.Editor
{
    [CustomPropertyDrawer(typeof(Timer))]
    public class TimerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the label width
            float labelWidth = EditorGUIUtility.labelWidth;

            // Draw the default property field with the original label
            EditorGUI.PropertyField(position, property, label);

            // Get the Timer object from the serialized property
            Timer timer = fieldInfo.GetValue(property.serializedObject.targetObject) as Timer;

            // If the Timer object is not null, display the remaining time
            if (timer != null)
            {
                // Calculate the position of the remaining time label
                Rect remainingTimeRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

                // Draw the remaining time label
                EditorGUI.LabelField(remainingTimeRect, "Remaining Time: " + timer.RemainingTime.ToString());

                // Mark the object as dirty and trigger a repaint
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}