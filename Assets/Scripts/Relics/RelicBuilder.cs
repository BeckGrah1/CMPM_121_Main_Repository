using NUnit.Framework.Internal;
using UnityEngine;

public class RelicBuilder
{
    public Relic Build(RelicData data)
    {
        Relic relic = new Relic();
        relic.name = data.name;
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
                return new GainManaEffect(data.effect.amount);
            case "gain-spellpower":
                return new GainSpellpowerEffect(data.effect.amount, data.effect.until);
            default:
                throw new System.Exception("Effect type: " + data.effect.type + " is not implemented yet, it needs its own RelicEvent subclass, and needs to be added to the switch for creating events in RelicBuilder");
        }
    }

    public static RelicTrigger TriggerFromString(RelicData data, RelicEffect effect)
    {
        switch (data.trigger.type)
        {
            case "take-damage": 
                return new TakeDamageTrigger(effect);

            case "on-kill": 
                return new OnKillTrigger(effect);

            case "stand-still": 
                return new StandStillTrigger(effect, data.trigger.amount);

            default:
                throw new System.Exception("Trigger type: " + data.trigger.type + " is not implemented yet, it needs its own RelicTrigger subclass, and needs to be added to the switch for creating triggers in RelicBuilder");
        }
    }
}
