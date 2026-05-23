using NUnit.Framework.Internal;
using UnityEngine;

public class RelicBuilder
{
    public static Relic Build(RelicData data)
    {
        Relic relic = new Relic();
        relic.name = data.name;
        relic.sprite = data.sprite;
        relic.effect = EffectFromString(data);
        relic.trigger = TriggerFromString(data, relic.effect);
        return relic;
    }

    public static RelicEffect EffectFromString(RelicData data)
    {
        switch (data.effect.type)
        {
            case "gain-mana":
                if (data.effect.until != null)
                {
                    Debug.LogWarning("effect type 'gain-mana' should not have an until property");
                }
                return new GainManaEffect(data);
            case "gain-spellpower":
                return new GainSpellpowerEffect(data);
            case "gain-max-mana":
                return new GainMaxManaEffect(data);
            case "gain-hp":
                return new GainHPEffect(data);
            default:
                throw new System.Exception("Effect type: " + data.effect.type + " is not implemented yet, it needs its own RelicEvent subclass, and needs to be added to the switch for creating events in RelicBuilder");
        }
    }

    public static RelicTrigger TriggerFromString(RelicData data, RelicEffect effect)
    {
        switch (data.trigger.type)
        {
            case "take-damage": 
                return new TakeDamageTrigger(data, effect);

            case "on-kill": 
                return new OnKillTrigger(data, effect);

            case "stand-still": 
                return new StandStillTrigger(data, effect);

            case "full-hp":
                return new FullHPTrigger(data, effect);
            case "on-wave-end":
                return new OnWaveEndTrigger(data, effect);

            default:
                throw new System.Exception("Trigger type: " + data.trigger.type + " is not implemented yet, it needs its own RelicTrigger subclass, and needs to be added to the switch for creating triggers in RelicBuilder");
        }
    }
}
