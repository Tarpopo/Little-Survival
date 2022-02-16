using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using SquareDino;
using TMPro;
using UI;
using UnityEngine;

public class FlyMoneyView : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI viewMoneyText;
    [SerializeField] private Transform coinEndFlyPoint;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform spawnCoinParent;

    [SerializeField] private bool activateWithWindow;

    [SerializeField, ShowIf("activateWithWindow")]
    private CanvasGroupBased canvasGroupBased;
    
    [Header("Settings")]
    [SerializeField] private int maxSpawnCoinCount = 15;
    [SerializeField] private int minAddedMoneyValuePerCoin = 10;
    [SerializeField] private Vector2 randomSpawnXOffset = new Vector2(-100f, 100f);
    [SerializeField] private Vector2 randomSpawnYOffset = new Vector2(-100f, 100f);
    
    private readonly Stack<Transform> _coins = new Stack<Transform>();
    private int _currentMoneyViewValue;
    
    private void Awake()
    {
        InitPool();

        if (activateWithWindow)
        {
            canvasGroupBased.OnEnableAll += SetActive;
            gameObject.SetActive(false);
        }
    }

    private void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    private void OnEnable()
    {
        _currentMoneyViewValue = MoneyHandler.MoneyData.Value;
        RefreshMoneyViewText();
        
        MoneyHandler.OnValueAdded += AnimatedFlyCoin;
    }

    private void OnDisable()
    {
        MoneyHandler.OnValueAdded -= AnimatedFlyCoin;
    }

    private void InitPool()
    {
        for (int i = 0; i < maxSpawnCoinCount; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        var coin = Instantiate(coinPrefab, spawnCoinParent);
        coin.gameObject.SetActive(false);
        _coins.Push(coin);
    }

    private Transform GetFlyCoin()
    {
        if(_coins.Count == 0)
            SpawnCoin();

        return _coins.Pop();
    }

    private void AnimatedFlyCoin(int value)
    {   
        var coinCount = Mathf.CeilToInt((float)value / minAddedMoneyValuePerCoin);

        if (coinCount > maxSpawnCoinCount)
            coinCount = maxSpawnCoinCount;
        
        var defaultIncreaseValue = Mathf.FloorToInt((float) value / coinCount);
        var lastIncreaseValue = value - (defaultIncreaseValue * coinCount) + defaultIncreaseValue;

        var endFlySequence = DOTween.Sequence();
        var globalFlySequence = DOTween.Sequence();
        
        var flyCoinsTemp = new List<Transform>();
        
        for (int i = 0; i < coinCount; i++)
        {
            var index = i;
            var flyCoin = GetFlyCoin();
            var flyCoinPosition = flyCoin.position;
            
            flyCoinsTemp.Add(flyCoin);
            
            flyCoin.gameObject.SetActive(true);
            flyCoin.localScale = Vector3.zero;
            flyCoin.localPosition = Vector3.zero;
            flyCoin.position = new Vector3(
                flyCoinPosition.x + Random.Range(randomSpawnXOffset.x, randomSpawnXOffset.y),
                flyCoinPosition.y + Random.Range(randomSpawnYOffset.x, randomSpawnYOffset.y),
                   flyCoinPosition.z
            );
            
            var flySequence = DOTween.Sequence();

            flySequence.Append(flyCoin.transform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc).SetDelay(Random.Range(0.1f, 0.4f)));
            flySequence.Join(flyCoin.transform.DOMove(coinEndFlyPoint.position, 0.6f).SetEase(Ease.InBack));
            flySequence.OnComplete(delegate
            {
                flyCoin.transform.DOScale(0f, 0.2f).SetEase(Ease.InCirc)
                    .OnComplete(delegate
                    {
                        flyCoin.gameObject.SetActive(false);
                    });

                if (index == coinCount - 1)
                {
                    _currentMoneyViewValue += lastIncreaseValue;
                    RefreshMoneyViewText();
                }
                else
                {
                    _currentMoneyViewValue += defaultIncreaseValue;
                    RefreshMoneyViewText();
                }
                
                endFlySequence?.Kill();
                coinEndFlyPoint.localScale = Vector3.one;
                endFlySequence.Append(coinEndFlyPoint.DOScale(1.15f, 0.1f).SetEase(Ease.OutQuint));
                endFlySequence.Append(coinEndFlyPoint.DOScale(1f, 0.3f));
                MyVibration.Haptic(MyHapticTypes.LightImpact);
            });
            
            globalFlySequence.Join(flySequence);
            globalFlySequence.OnComplete(delegate
            {
                foreach (var flyCoinTemp in flyCoinsTemp)
                {
                    flyCoinTemp.gameObject.SetActive(false);
                    flyCoinTemp.localPosition = Vector3.zero;
                    _coins.Push(flyCoinTemp);
                }
            });
        }
    }

    private void RefreshMoneyViewText()
    {
        viewMoneyText.text = _currentMoneyViewValue.ToString();
    }
}