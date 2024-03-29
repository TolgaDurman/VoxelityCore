using System;
using UnityEngine;

namespace Voxelity.Timers
{
    [AddComponentMenu("Voxelity/Time Displayer")]
    public class TimeFromStart : MonoBehaviour
    {
        public int _fontSize = 25;
        private void OnGUI()
        {
            GUIContent content = new GUIContent(Time.timeSinceLevelLoad.ToString("F1"));
            GUIStyle style = new GUIStyle();
            style.fontSize = _fontSize;
            GUILayout.Label(content,style);
        }
    }
}