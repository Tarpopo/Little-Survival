using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AnimationComponent<T> where T : Enum
{
    [SerializeField] private List<AnimationItem<T>> _animationItems;
    [SerializeField] private AnimationClip[] _randomAnimations;
    [SerializeField] private Animator _animator;
    private Dictionary<Enum, string> _animations = new Dictionary<Enum, string>();
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int Speed = Animator.StringToHash("Speed");
    public void SetParameters() => UpdateDictionary();

    public void PlayAnimation(T animationType, bool onlyState = false)
    {
        _animator.SetInteger(State, Convert.ToInt32(animationType));
        if (onlyState) return;
        if (_animations.TryGetValue(animationType, out var name) == false) return;
        _animator.Play(name);
    }
    public void SetSpeed(float speed) => _animator.SetFloat(Speed, speed);
    public void PlayRandomAnimation() =>
        _animator.Play(_randomAnimations[Random.Range(0, _randomAnimations.Length)].name);

    private void UpdateDictionary()
    {
        _animations.Clear();
        foreach (var animationItem in _animationItems)
            _animations.Add(animationItem.AnimationType, animationItem.AnimationClip.name);
    }
}

[Serializable]
public struct AnimationItem<T> where T : Enum
{
    public AnimationClip AnimationClip;
    public T AnimationType;
}

public enum UnitAnimations
{
    Run,
    Walk,
    Idle,
    MeleeAttack,
    Dance,
    SearchIdle,
    Upgrade,
    Death,
    TakeDamage,
    BikeSit,
    BikeAttack,
    BikeLeft,
    BikeRight,
    BikeWait
}

public enum WeaponAnimation
{
    Idle,
    Reload,
    ZoomIn,
    ZoomOut
}