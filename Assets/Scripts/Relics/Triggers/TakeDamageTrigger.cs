using UnityEngine;

public class TakeDamageTrigger : RelicTrigger
{
    public TakeDamageTrigger(RelicEffect effect)
    {
        EventBus.Instance.OnPlayerDamaged += (dmg) => effect.DoEffect();
    }
}
