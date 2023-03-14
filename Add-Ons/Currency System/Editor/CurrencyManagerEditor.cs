using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voxelity.Currency.Editor
{
    [CustomEditor(typeof(CurrencyManager))]
    public class CurrencyManagerEditor : UnityEditor.Editor 
    {
        CurrencyManager targetObject;
        private void OnEnable() 
        {
            targetObject = (CurrencyManager)target;
        }
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            targetObject.name = targetObject.currencyName + " Manager";
        }
    }
}
