using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class MultiplierBonusPanel : MonoBehaviour
{
    public event Action<float> OnChangeMultiplyValue;
    public event Action<float> OnGetFinalMultiplyValue;
    
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private bool isInverseMove = false;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.Linear(0,0, 1, 1);
    [SerializeField] private MoveType moveType;
    [SerializeField, OnValueChanged(nameof(RecalculateAllRanges))] private Vector2 moveRangePointer;
    [SerializeField] private Transform pointer;
    [SerializeField, OnValueChanged(nameof(InitRanges))]
    
    private List<MultipierBonusData> _multiplierBonusDatas = new List<MultipierBonusData>();
    private float _currentMultiplyValue;
    private Sequence _moveSequance;
    private MultipierBonusData _currentMultipierBonusData;
    private Coroutine _checkerChangeMultiply;
    
    public Vector2 MoveRangePointer => moveRangePointer;

    private void Start()
    {
        StartMovePointer();
    }

    private void OnValidate()
    {
        if (_multiplierBonusDatas.Count == 0)
        {
            var bonusScoreData = new MultipierBonusData();
            _multiplierBonusDatas.Add(bonusScoreData);
            bonusScoreData.Init(this);
        }
    }

    public void StartMovePointer()
    {
        _moveSequance = DOTween.Sequence();

        switch (moveType)
        {
            case MoveType.LocalRotate:
                StartLocalRotate(_moveSequance);
                break;
            case MoveType.LocalMove:
                StartLocalMove(_moveSequance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _moveSequance.SetLoops(-1, LoopType.Restart);

        _checkerChangeMultiply = StartCoroutine(CheckerChangeMultiply());
    }

    private void StartLocalMove(Sequence sequence)
    {
        var startMovePosition = new Vector3(moveRangePointer.x, pointer.localPosition.y, pointer.localPosition.z);
        var endMovePoint = new Vector3(moveRangePointer.y, pointer.localPosition.y, pointer.localPosition.z);
        
        pointer.transform.localPosition = startMovePosition;
        sequence.Join(pointer.DOLocalMove(endMovePoint, moveDuration).SetEase(moveCurve));
        sequence.Append(pointer.DOLocalMove(startMovePosition, moveDuration).SetEase(moveCurve));
    }

    private void StartLocalRotate(Sequence sequence)
    {
        var startRotatePosition = new Vector3(0, 0, moveRangePointer.x);
        var endRotatePoint = new Vector3(0, 0, moveRangePointer.y);

        if (isInverseMove)
        {
            pointer.transform.localRotation = Quaternion.Euler(endRotatePoint);
            sequence.Join(pointer.DOLocalRotate(startRotatePosition, moveDuration).SetEase(moveCurve));
            sequence.Append(pointer.DOLocalRotate(endRotatePoint, moveDuration).SetEase(moveCurve));
        }
        else
        {
            pointer.transform.localRotation = Quaternion.Euler(startRotatePosition);
            sequence.Join(pointer.DOLocalRotate(endRotatePoint, moveDuration).SetEase(moveCurve));
            sequence.Append(pointer.DOLocalRotate(startRotatePosition, moveDuration).SetEase(moveCurve));
        }

        
    }
    
    public void StopMovePointer()
    {
        if(_checkerChangeMultiply != null)
            StopCoroutine(_checkerChangeMultiply);
        
        _moveSequance?.Kill();

        OnGetFinalMultiplyValue?.Invoke(_currentMultiplyValue);
    }

    private IEnumerator CheckerChangeMultiply()
    {
        while (true)
        {
            CheckCurrentMultiply();
            yield return null;
        }
    }

    private void CheckCurrentMultiply()
    {
        float pointerValue;
        
        switch (moveType)
        {
            case MoveType.LocalRotate:
                pointerValue = pointer.localEulerAngles.z;
                pointerValue = pointerValue > 180 ? pointerValue - 360 : pointerValue;
                break;
            case MoveType.LocalMove:
                pointerValue = pointer.localPosition.x;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        foreach (var bonusScoreData in _multiplierBonusDatas)
        {
            if (pointerValue <= bonusScoreData.MinMaxOffset.y && pointerValue >= bonusScoreData.MinMaxOffset.x)
            {
                if (_currentMultipierBonusData == bonusScoreData) continue;
                
                _currentMultipierBonusData = bonusScoreData;
                _currentMultiplyValue = bonusScoreData.MultiplyScoreValue;
                OnChangeMultiplyValue?.Invoke(_currentMultiplyValue);
            }
        }
    }

    private void RecalculateAllRanges()
    {
        InitRanges();
        foreach (var bonusScoreData in _multiplierBonusDatas)
        {
            bonusScoreData.Init(this);
            RecalculateRanges(bonusScoreData);
        }
    }
    
    public void RecalculateRanges(MultipierBonusData multipierBonusData)
    {
        for (int i = 0; i < _multiplierBonusDatas.Count; i++)
        {
            if (_multiplierBonusDatas[i] != multipierBonusData) continue;
            
            if (i - 1 >= 0)
            {
                if (multipierBonusData.MinMaxOffset.x < _multiplierBonusDatas[i - 1].MinMaxOffset.x)
                {
                    multipierBonusData.ChangeMinOffsetValue(_multiplierBonusDatas[i - 1].MinMaxOffset.x);
                    continue;
                }
                    
                _multiplierBonusDatas[i - 1].ChangeMaxOffsetValue(multipierBonusData.MinMaxOffset.x);
            }
            else
            {
                multipierBonusData.ChangeMinOffsetValue(moveRangePointer.x);
            }

            if (i + 1 < _multiplierBonusDatas.Count)
            {
                if (multipierBonusData.MinMaxOffset.y > _multiplierBonusDatas[i + 1].MinMaxOffset.y)
                {
                    multipierBonusData.ChangeMaxOffsetValue(_multiplierBonusDatas[i + 1].MinMaxOffset.x);
                    continue;
                }
                    
                _multiplierBonusDatas[i + 1].ChangeMinOffsetValue(multipierBonusData.MinMaxOffset.y);
            }
            else
            {
                multipierBonusData.ChangeMaxOffsetValue(moveRangePointer.y);
            }
        }
    }

    private void InitRanges()
    {
        if(_multiplierBonusDatas.Count == 0)
            _multiplierBonusDatas.Add(new MultipierBonusData());
        
        _multiplierBonusDatas[_multiplierBonusDatas.Count - 1].Init(this);
        AverageValues();
    }

    [Button]
    private void AverageValues()
    {
        var lineLength = Mathf.Abs(moveRangePointer.x) + Mathf.Abs(moveRangePointer.y);
        var partLength = lineLength / _multiplierBonusDatas.Count;
        
        for (int i = 0; i < _multiplierBonusDatas.Count; i++)
        {
            _multiplierBonusDatas[i].Init(this);
            _multiplierBonusDatas[i].ChangeOffset(new Vector2(Mathf.Clamp(moveRangePointer.x + partLength * i, moveRangePointer.x, moveRangePointer.y),
                Mathf.Clamp(moveRangePointer.x + partLength * (i + 1), moveRangePointer.x, moveRangePointer.y)));
        }
    }

    public enum MoveType
    {
        LocalRotate,
        LocalMove
    }
}