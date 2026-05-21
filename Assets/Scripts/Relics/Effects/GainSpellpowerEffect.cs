using UnityEngine;

public class GainSpellpowerEffect : RelicEffect
{
    int amount;
    bool active = false;
    public GainSpellpowerEffect(string amnt, string until)
    {
        amount = int.Parse(amnt);
        RegisterUntil(until);
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
}
