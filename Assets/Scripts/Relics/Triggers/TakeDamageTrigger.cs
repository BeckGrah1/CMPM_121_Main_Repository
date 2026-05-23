using UnityEngine;

public class TakeDamageTrigger : RelicTrigger
{
    public TakeDamageTrigger(RelicData data, RelicEffect effect)
    {
        description = data.trigger.description;
        EventBus.Instance.OnPlayerDamaged += (dmg) => effect.DoEffect();
    }
}
