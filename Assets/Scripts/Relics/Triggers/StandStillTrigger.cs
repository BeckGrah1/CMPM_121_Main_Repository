using UnityEngine;

public class StandStillTrigger : RelicTrigger
{
    float amount;
    float timePassed = 0;
    RelicEffect effect;
    bool effectActive;
    private Vector3 pos;
    public StandStillTrigger(RelicData data, RelicEffect efct)
    {
        description = data.trigger.description;
        amount = float.Parse(data.trigger.amount);
        effect = efct;
        EventBus.Instance.OnPlayerMoved += resetWaitTime;
    }

    private void resetWaitTime()
    {
        timePassed = 0;
        pos = GameManager.Instance.player.transform.position;
        if (effectActive)
        {
            effectActive = false;
            effect.StopEffect();
        }
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > amount && pos == GameManager.Instance.player.transform.position)
        {
            if (!effectActive)
            {
                effectActive = true;
                effect.DoEffect();
            }
        }
    }
}
