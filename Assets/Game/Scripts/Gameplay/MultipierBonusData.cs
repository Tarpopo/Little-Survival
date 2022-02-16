using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class MultipierBonusData
{   
    [SerializeField] private float multiplyScoreValue;
    [SerializeField, MinMaxSlider("@globalRange.x", "@globalRange.y", true), OnValueChanged(nameof(Recalculate))] 
    private Vector2 minMaxOffset;
    [SerializeField, HideInInspector] 
    private Vector2 globalRange;
    [SerializeField, HideInInspector] 
    private MultiplierBonusPanel multiplierBonusPanel;
    
    public Vector2 MinMaxOffset => minMaxOffset;
    public float MultiplyScoreValue => multiplyScoreValue;

    public void Init(MultiplierBonusPanel multiplierBonusPanel)
    {
        this.multiplierBonusPanel = multiplierBonusPanel;
        globalRange = multiplierBonusPanel.MoveRangePointer;
    }
    
    public void ChangeOffset(Vector2 newOffset)
    {
        minMaxOffset = newOffset;
    }

    public void ChangeMinOffsetValue(float value)
    {
        minMaxOffset = new Vector2(value, minMaxOffset.y);
    }
    
    public void ChangeMaxOffsetValue(float value)
    {
        minMaxOffset = new Vector2(minMaxOffset.x,value);
    }

    private void Recalculate()
    {
        multiplierBonusPanel.RecalculateRanges(this);
    }
}