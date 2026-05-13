using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DoublerSpell : ModifierSpell
{
    public DoublerSpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data, innerSpell)
    {
        
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, SpellParams spellParams)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", spellParams.wave },
            { "power", spellParams.power }
        };

        ApplyModifiers(spellParams);
        yield return innerSpell.Cast(where, target, team, spellParams);
        yield return new WaitForSeconds(RPNEvaluator.RPNEvaluator.Evaluatef(data.delay, dict));
        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}
