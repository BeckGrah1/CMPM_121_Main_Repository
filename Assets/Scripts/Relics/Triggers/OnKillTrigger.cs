using UnityEngine;

public class OnKillTrigger : RelicTrigger
{
    public OnKillTrigger(RelicData data, RelicEffect effect)
    {
        description = data.trigger.description;
        EventBus.Instance.OnEnemyKilled += effect.DoEffect;
    }
}
