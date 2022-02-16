using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI viewMoneyText;
    [SerializeField] private Transform coinIcon;
    private int _currentMoneyView;
    Sequence _addCoinSequence;
    
    private void Awake()
    {
        _currentMoneyView = MoneyHandler.MoneyData.Value;
        _addCoinSequence = DOTween.Sequence();
        RefreshText();   
    }

    private void OnEnable()
    {
        MoneyHandler.OnValueChanged += RefreshMoneyView;
    }

    private void OnDisable()
    {
        MoneyHandler.OnValueChanged -= RefreshMoneyView;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MoneyHandler.AddMoney(100);
        }
    }
#endif

    private void RefreshMoneyView(int value)
    {
        _currentMoneyView = value;
        
        _addCoinSequence?.Kill();
        coinIcon.transform.localScale = Vector3.one;
        _addCoinSequence.Append(coinIcon.DOScale(1.15f, 0.1f).SetEase(Ease.OutQuint));
        _addCoinSequence.Append(coinIcon.DOScale(1f, 0.3f));
        
        RefreshText();
    }

    private void RefreshText()
    {
        viewMoneyText.text = _currentMoneyView.ToString();
    }
}