using System.Collections.Generic;
using UnityEngine;
using RPNEvaluator;

public class GainHPEffect : RelicEffect
{
    int amount;
    public GainHPEffect(RelicData data)
    {
        description = data.effect.description;
        amount = int.Parse(data.effect.amount);
    }

    public override void DoEffect()
    {
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.HealPlayer(amount);
    }
}
