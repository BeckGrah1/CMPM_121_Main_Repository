using UnityEngine;
using System.Collections;

public class ModifierSpell : Spell
{
    Spell innerSpell;

    public ModifierSpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data)
    {
        this.innerSpell = innerSpell;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        SpellParams spellParams = new SpellParams
        {
            wave = GameManager.Instance.waveNum,
            power = owner.spellpower
        };
        yield return Cast(where, target, team, spellParams);
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, SpellParams spellParams)
    {
        if (data.damage_multiplier != null)
            spellParams.damageModifiers.Add(new ValueModifier { // need more here
            });

        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}
