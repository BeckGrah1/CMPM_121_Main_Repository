using UnityEngine;
using System;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }

    public event Action<Damage> OnPlayerDamaged;
    public void DoPlayerDamaged(Damage dmg)
    {
        OnPlayerDamaged?.Invoke(dmg);
    }

    public event Action OnEnemyKilled;
    public void DoEnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

    public event Action OnPlayerMoved;
    public void DoPlayerMoved()
    {
        OnPlayerMoved?.Invoke();
    }

    public event Action OnPlayerStoodStill;
    public void DoPlayerStoodStill()
    {
        OnPlayerStoodStill?.Invoke();
    }

    public event Action OnSpellCast;
    public void DoSpellCast()
    {
        OnSpellCast?.Invoke();
    }

}
