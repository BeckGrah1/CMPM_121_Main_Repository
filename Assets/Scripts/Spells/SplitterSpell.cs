using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SplitterSpell : ModifierSpell
{
    public SplitterSpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data, innerSpell)
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

        // calculate random shot in angle defined in spell data
        float angle = RPNEvaluator.RPNEvaluator.Evaluatef(data.angle, dict);
        Vector3 dir1 = Quaternion.Euler(0, 0, Random.Range(0, angle)) * (target - where);
        Vector3 dir2 = Quaternion.Euler(0, 0, -Random.Range(0, angle)) * (target - where);

        yield return innerSpell.Cast(where, where + dir1, team, spellParams);
        yield return innerSpell.Cast(where, where + dir2, team, spellParams);
    }
}
