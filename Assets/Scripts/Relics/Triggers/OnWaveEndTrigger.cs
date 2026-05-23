using UnityEngine;

public class OnWaveEndTrigger : RelicTrigger
{
    public OnWaveEndTrigger(RelicData data, RelicEffect effect)
    {
        description = data.trigger.description;
        GameManager.OnWAVEEND += effect.DoEffect;
    }
}
