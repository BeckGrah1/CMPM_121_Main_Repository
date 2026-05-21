using NUnit.Framework.Internal;
using UnityEngine;

public class RelicBuilder : MonoBehaviour
{
    public Relic Build(RelicData data)
    {
        Relic relic = new Relic();
        relic.effect = EffectFromString(data);
        relic.trigger = TriggerFromString(data, relic.effect);
        return relic;
    }

    public static RelicEffect EffectFromString(RelicData data)
    {
        switch (data.effectData.type)
        {
            case "gain-mana":
                return new GainManaEffect();
            case "gain-spellpower":
                return new GainSpellpowerEffect();
            default:
                throw new System.Exception("Effect type: " + data.effectData.type + "is not implemented yet, it needs its own RelicEvent subclass, and needs to be added to the switch for creating events in RelicBuilder");
        }
    }

    public static RelicTrigger TriggerFromString(RelicData data, RelicEffect effect)
    {
        switch (data.triggerData.type)
        {
            case "take-damage": 
                return new TakeDamageTrigger(effect);

            case "on-kill": 
                return new OnKillTrigger(effect);

            case "stand-still": 
                return new StandStillTrigger(effect, data.triggerData.amount);

            default:
                throw new System.Exception("Trigger type: " + data.triggerData.type + "is not implemented yet, it needs its own RelicTrigger subclass, and needs to be added to the switch for creating triggers in RelicBuilder");
        }
    }
}
