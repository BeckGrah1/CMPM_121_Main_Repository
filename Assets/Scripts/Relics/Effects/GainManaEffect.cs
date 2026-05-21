using UnityEngine;

public class GainManaEffect : RelicEffect
{
    int amount;
    public GainManaEffect(string amt)
    {
        amount = int.Parse(amt);
    }

    public override void DoEffect()
    {
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.spellcaster.mana = Mathf.Min(player.spellcaster.mana + amount, player.spellcaster.max_mana);
    }
}
