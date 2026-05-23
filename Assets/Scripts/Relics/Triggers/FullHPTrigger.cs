using UnityEngine;

public class FullHPTrigger : RelicTrigger
{
    public FullHPTrigger(RelicData data, RelicEffect effect)
    {
        description = data.trigger.description;
        EventBus.Instance.OnFullHP += effect.DoEffect;
        if (GameManager.Instance.player.GetComponent<PlayerController>().hp.hp == GameManager.Instance.player.GetComponent<PlayerController>().hp.max_hp)
        {
            effect.DoEffect();
            effect.active = true;
        }
    }
}
