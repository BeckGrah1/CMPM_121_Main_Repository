using System.Collections.Generic;
using UnityEngine;

public class SpellBuilder
{
    private static List<string> baseSpells = new List<string> { "arcane_bolt"};
    private static List<string> modifierSpells = new List<string> { "damage_amp", "speed_amp", "doubler", "splitter", "chaos", "homing", "geometry", "vampire" };
    public string spellDescription;
    public RewardScreenManager rsm;

    public Spell Build(SpellCaster owner)
    {
        // pick random base spell
        string baseKey = baseSpells[Random.Range(0, baseSpells.Count)];
        Spell spell = new Spell(owner, SpellDataManager.Instance.Get(baseKey));
        spellDescription = baseKey;

        // wrap in 2 random modifiers
        
        for (int i = 0; i < 2; i++)
        {
            string modKey = modifierSpells[Random.Range(0, modifierSpells.Count)];
            SpellData modData = SpellDataManager.Instance.Get(modKey);
            spell = CreateModifierSpell(owner, modData, modKey, spell);
            spellDescription = modKey + " -> " + spellDescription;
        }

        Debug.Log("Built spell: " + spellDescription);
        if (rsm == null)
        {
            rsm = GameObject.FindAnyObjectByType<RewardScreenManager>();
        }
        rsm.changeSpellDescription(spellDescription);
        return spell;
    }

    private Spell CreateModifierSpell(SpellCaster owner, SpellData data, string key, Spell inner)
    {
        switch (key)
        {
            case "doubler": return new DoublerSpell(owner, data, inner);
            case "splitter": return new SplitterSpell(owner, data, inner);
            case "geometry": return new GeometrySpell(owner, data, inner);
            case "vampire": return new VampireSpell(owner, data, inner);
            default: return new ModifierSpell(owner, data, inner);
        }
    }
}
