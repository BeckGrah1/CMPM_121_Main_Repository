using UnityEngine;
using RPNEvaluator;
using System.Collections.Generic;

public class GainSpellpowerEffect : RelicEffect
{
    int amount;
    string rpnAmount;
    public GainSpellpowerEffect(RelicData data)
    {
        rpnAmount = data.effect.amount;
        Dictionary<string, int> dict = new Dictionary<string, int>
            {
                {"wave", GameManager.Instance.waveNum }
            };
        amount = RPNEvaluator.RPNEvaluator.Evaluate(rpnAmount, dict);
        if (data.effect.until != null)
        {
            RegisterUntil(data.effect.until);
        }
    }

    public override void DoEffect()
    {
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.spellcaster.spellpower += amount;
        active = true;
    }

    public override void StopEffect()
    {
        if (active)
        {
            PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
            player.spellcaster.spellpower -= amount;
            active = false;
        }
    }

    public override void UpdateRPN()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
            {
                {"wave", GameManager.Instance.waveNum }
            };
        amount = RPNEvaluator.RPNEvaluator.Evaluate(rpnAmount, dict);
    }

    public override bool IsActive()
    {
        return active;
    }
}
