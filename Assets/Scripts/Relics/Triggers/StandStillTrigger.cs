using UnityEngine;

public class StandStillTrigger : RelicTrigger
{
    float amount;
    float timePassed = 0;
    RelicEffect effect;
    bool effectActive;
    public StandStillTrigger(RelicEffect efct, string amt)
    {
        amount = float.Parse(amt);
        effect = efct;
        EventBus.Instance.OnPlayerMoved += resetWaitTime;
    }

    private void resetWaitTime()
    {
        timePassed = 0;
        if (effectActive)
        {
            effectActive = false;
            effect.StopEffect();
        }
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > amount)
        {
            if (!effectActive)
            {
                effectActive = true;
                effect.DoEffect();
            }
        }
    }
}
