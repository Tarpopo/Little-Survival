using UnityEditor;
using UnityEngine;

namespace SquareDino.Scripts.Editor
{
    [InitializeOnLoad]
    public static class GameObjectIcons
    {
        /// <summary>
        /// Hierarchy window game object icon.
        /// http://diegogiacomelli.com.br/unitytips-hierarchy-window-gameobject-icon/
        /// </summary>

        const string IconsList = "iconSquareDino";
    
        static GameObjectIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }
    
        static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceId), null);

            if (content.image != null && IconsList.Contains(content.image.name))
                GUI.DrawTexture(new Rect(selectionRect.xMax - 16, selectionRect.yMin, 16, 16), content.image);
        }
    }
}