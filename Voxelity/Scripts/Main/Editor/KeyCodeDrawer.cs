using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(KeyCode))]
public class KeyCodeDrawer : PropertyDrawer
{
    private bool isRecording = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the label and get the position of the text field
        Rect labelPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        Rect textFieldPosition = new Rect(labelPosition.x, labelPosition.y, labelPosition.width - 25, labelPosition.height);

        if (isRecording)
        {
            EditorGUI.TextField(textFieldPosition, "Press any key or mouse button...");
        }
        else
        {
            // Draw the default property field
            EditorGUI.PropertyField(textFieldPosition, property, GUIContent.none);
        }

        // Draw the record button to the right of the text field
        Rect buttonPosition = new Rect(textFieldPosition.xMax + 5, textFieldPosition.y, 20, textFieldPosition.height);
        if (GUI.Button(buttonPosition, "R"))
        {
            isRecording = true;
        }

        if (isRecording)
        {
            Event e = Event.current;
            if (e.isKey || e.type == EventType.MouseDown)
            {
                isRecording = false;
                if (e.isKey)
                {
                    property.intValue = (int)e.keyCode;
                }
                else
                {
                    property.intValue = (int)e.button + 323; // Add 323 to the button value to get the corresponding KeyCode value
                }
                GUI.changed = true;
            }
        }

        EditorGUI.EndProperty();
    }
}
