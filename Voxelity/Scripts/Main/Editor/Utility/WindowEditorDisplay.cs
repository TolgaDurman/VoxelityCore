using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UEditor = UnityEditor.Editor;
using System;
using System.Linq;

namespace Voxelity.Editor
{
    public class WindowEditorDisplay<T1, T2> where T1 : UnityEngine.Object where T2 : UEditor
    {
        private List<T1> cachedUsers = new List<T1>();
        private List<string> ignoredNames = new List<string>();
        private bool[] foldouts;
        private UEditor[] editors;
        private bool useFoldout = true;
        private bool canBeSelected = true;

        public WindowEditorDisplay(bool useFoldout = true, bool canBeSelected = true, params string[] ignoredNames)
        {
            this.useFoldout = useFoldout;
            this.canBeSelected = canBeSelected;
            this.ignoredNames = ignoredNames.ToList();
        }
        public void Init(T1[] users)
        {
            cachedUsers = users.ToList();
            cachedUsers.RemoveAll(x => ignoredNames.Contains(x.name));
            foldouts = new bool[cachedUsers.Count];
            editors = new UEditor[cachedUsers.Count];
        }
        public void CustomDraw(Action<T1> action)
        {
            var iteratedItems = cachedUsers.ToArray();
            foreach (T1 item in iteratedItems)
            {
                action?.Invoke(item);
            }
        }
        public bool Foldout(T1 item)
        {
            return foldouts[cachedUsers.IndexOf(item)];
        }
        public int IndexOf(T1 item)
        {
            return cachedUsers.IndexOf(item);
        }
        public void DisplayFoldout(int index)
        {
            foldouts[index] = EditorGUILayout.Foldout(foldouts[index], cachedUsers[index].name, toggleOnLabelClick: true);
        }
        public void DisplayItem(int index)
        {
            UnityEditor.Editor.CreateCachedEditor(cachedUsers[index], typeof(T2), ref editors[index]);
            editors[index].OnInspectorGUI();
        }
        public void OnGUI(bool display = true)
        {
            if (!display) return;

            EditorGUILayout.BeginVertical("box");

            for (int i = 0; i < cachedUsers.Count; i++)
            {
                VoxelityGUI.Line();
                if (!canBeSelected)
                {
                    DisplayFoldout(i);
                }
                else if (VoxelityGUI.InLineButton("Ping", () =>
                {
                    DisplayFoldout(i);
                }, false, GUILayout.Width(55), GUILayout.Height(20)))
                {
                    EditorGUIUtility.PingObject(cachedUsers[i]);
                    Selection.activeObject = cachedUsers[i];
                }

                if (!useFoldout)
                {
                    DisplayItem(i);
                }
                else if (foldouts[i])
                {
                    DisplayItem(i);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
