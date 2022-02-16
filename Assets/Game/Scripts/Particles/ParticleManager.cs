using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ParticleManager : MySingleton<ParticleManager>
{
    [SerializeField] private List<ParticleSettings> _particles;
    private ManagerPool _managerPool;

    private void Awake()
    {
        _managerPool = ManagerPool.Instance;
        foreach (var particle in _particles)
        {
            _managerPool.AddPool(PoolType.Fx).PopulateWith(particle.Prefab, particle.PrefabsCount);
        }
    }

    public void PlayParticle(ParticleTypes particleType, Vector3 position, Vector3 rotation, Vector3 scale,
        Transform target, float delay = 1f)
    {
        var particleSettings = _particles.FirstOrDefault(item => item.ParticleType == particleType);
        var particle =
            _managerPool.Spawn<Particle>(PoolType.Fx, particleSettings.Prefab, position, Quaternion.Euler(rotation));
        particle.PlayParticle(scale, target, delay, () => _managerPool.Despawn(PoolType.Fx, particle.gameObject));
    }
}

[Serializable]
public class ParticleSettings
{
    public ParticleTypes ParticleType;
    public GameObject Prefab;
    public int PrefabsCount;
}

public enum ParticleTypes
{
    Shoot,
    Death,
    Explosion,
    Upgrade
}