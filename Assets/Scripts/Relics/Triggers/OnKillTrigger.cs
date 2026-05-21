using UnityEngine;

public class OnKillTrigger : RelicTrigger
{
    public OnKillTrigger(RelicEffect effect)
    {
        EventBus.Instance.OnEnemyKilled += effect.DoEffect;
    }
}
