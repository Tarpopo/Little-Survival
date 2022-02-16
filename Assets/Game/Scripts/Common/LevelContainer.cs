using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[CreateAssetMenu(menuName = "Configs/Level Container")]
public class LevelContainer : GlobalConfig<LevelContainer>
{
    [SerializeField, BoxGroup("Debug")] 
    private bool isDebug;
    [SerializeField, ShowIf("isDebug"), ValueDropdown("levels"), BoxGroup("Debug")]
    private Level debugLevel;
    [SerializeField] 
    private List<Level> levels;

    public List<Level> Levels => levels;
    public bool IsDebug => isDebug;
    public Level DebugLevel => debugLevel;
}