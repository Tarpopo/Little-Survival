using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class GameSettingsWindow : OdinMenuEditorWindow
{
    [MenuItem("Window/Game Settings Window %L")]
    public static void OpenWindow()
    {
        GetWindow(typeof(GameSettingsWindow)).Show();    
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(supportsMultiSelect: true)
        {
            { "Home",                              this,                               EditorIcons.House             },
            { "GameSettings",                      null,                               EditorIcons.SettingsCog       },
            { "GameSettings/Level Settings",                    LevelContainer.Instance,            EditorIcons.Tree              },                                                        
        };
        
        return tree;
    }
}