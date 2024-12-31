using Doryu.StatSystem;
using System;
using UnityEngine;

public class HealthCompo : MonoBehaviour, IEntityComponent, IAfterInitComponent
{
    public int Health { get; private set; }

    [SerializeField] private StatElementSO _healthStatSO;

    private Entity _owner;
    private StatElement _maxHealth;
    private bool _isInvincible;
    private bool _isDie;

    public int MaxHealth => _maxHealth.IntValue;
    public event Action<int, int, bool> OnHealthChangedEvent;
    public event Action OnDieEvent;

    public void Initialize(Entity entity)
    {
        _owner = entity;
    }

    public void AfterInit()
    {
        _maxHealth = _owner.GetEntityComponent<StatCompo>().GetElement(_healthStatSO);
        _isInvincible = _maxHealth == null;
        Health = MaxHealth;
    }

    public void ApplyDamage(int damage, bool isChangeVisible = true)
    {
        if (_isDie) return;

        int prev = Health;
        Health -= damage;
        if (Health < 0)
            Health = 0;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);

        if (Health == 0) Die();
    }

    public void ApplyRecovery(int recovery, bool isChangeVisible = true)
    {
        if (_isDie) return;

        int prev = Health;
        Health += recovery;
        if (Health > MaxHealth)
            Health = MaxHealth;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);
    }

    public void Resurrection()
    {
        _isDie = false;
        ApplyRecovery(MaxHealth, false);
    }

    public void Die()
    {
        _isDie = true;
        OnDieEvent?.Invoke();
    }
}
 