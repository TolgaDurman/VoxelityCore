using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
using Voxelity.Extensions;
#endif

namespace Voxelity
{
    public static class VDebug
    {
#if UNITY_EDITOR
        public static bool IsEnabled
        {
            get => EditorPrefs.GetBool("v=Logs_Enabled", false);
        }
        public static bool LogEnabled
        {
            get => EditorPrefs.GetBool("v=Logs_Enabled", true);
        }
        public static bool LogWarningEnabled
        {
            get => EditorPrefs.GetBool("v=Logs_Enabled", true);
        }
        public static bool LogErrorEnabled
        {
            get => EditorPrefs.GetBool("v=Logs_Enabled", true);
        }
        public static void Log(object message)
        {
            if (!IsEnabled || !LogEnabled) return;
            Debug.Log(message);
        }
        public static void Log(object message, Object obj)
        {
            if (!IsEnabled || !LogEnabled) return;
            Debug.Log(message, obj);
        }
        public static void LogWarning(object message)
        {
            if (!IsEnabled || !LogWarningEnabled) return;
            Debug.LogWarning(message);
        }
        public static void LogWarning(object message, Object obj)
        {
            if (!IsEnabled || !LogWarningEnabled) return;
            Debug.LogWarning(message, obj);
        }
        public static void LogError(object message)
        {
            if (!IsEnabled || !LogErrorEnabled) return;
            Debug.LogError(message);
        }
        public static void LogError(object message, Object obj)
        {
            if (!IsEnabled || !LogErrorEnabled) return;
            Debug.LogError(message, obj);
        }
        public static void LogPause(object message)
        {
            Debug.Log("Paused by Voxelity: ".Colorize(Color.red) + message);
            Debug.Break();
        }
        public static void LogPause(object message, Object obj)
        {
            Debug.Log("Paused by Voxelity: ".Colorize(Color.red) + message, obj);
            Debug.Break();
        }
#else
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        public static void Log(object message, Object obj)
        {
            Debug.Log(message, obj);
        }
        public static void LogWaning(object message)
        {
            Debug.LogWarning(message);
        }
        public static void LogWaning(object message, Object obj)
        {
            Debug.LogWarning(message, obj);
        }
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        public static void LogError(object message, Object obj)
        {
            Debug.LogError(message, obj);
        }
#endif
    }
}
