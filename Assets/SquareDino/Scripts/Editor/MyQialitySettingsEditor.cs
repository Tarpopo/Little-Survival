using SquareDino.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace SquareDino.Scripts.Editor
{
    [CustomEditor(typeof(MyQualitySettings))]
    public class MyQialitySettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var settings = target as MyQualitySettings;
            if (settings == null) return;
            
            if (GUILayout.Button("Apply Settings"))
            {
                settings.ApplySettings();
            }
        }
    }
}