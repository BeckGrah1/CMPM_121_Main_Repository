using UnityEngine;

public class GainManaEffect : RelicEffect
{
    int amount;
    public GainManaEffect(RelicData data)
    {
        description = data.effect.description;
        amount = int.Parse(data.effect.amount);
    }

    public override void DoEffect()
    {
        Debug.Log("effect activated");
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.spellcaster.mana = Mathf.Min(player.spellcaster.mana + amount, player.spellcaster.max_mana);
    }
}
