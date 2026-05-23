using UnityEngine;

public class GainMaxManaEffect : RelicEffect
{
    int amount;
    public GainMaxManaEffect(RelicData data)
    {
        description = data.effect.description;
        amount = int.Parse(data.effect.amount);
        RegisterUntil(data.effect.until);
    }

    public override void DoEffect()
    {
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.spellcaster.max_mana += amount;
        active = true;
    }

    public override void StopEffect()
    {
        if (active)
        {
            PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
            player.spellcaster.max_mana -= amount;
            if (player.spellcaster.mana > player.spellcaster.max_mana)
            {
                player.spellcaster.mana = player.spellcaster.max_mana;
            }
            active = false;
        }
    }
}
