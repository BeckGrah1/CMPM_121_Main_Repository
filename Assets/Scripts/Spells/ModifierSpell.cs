using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ModifierSpell : Spell
{
    public Spell innerSpell;

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
        ApplyModifiers(spellParams);
        yield return innerSpell.Cast(where, target, team, spellParams);
    }

    protected void ApplyModifiers(SpellParams spellParams)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", spellParams.wave },
            { "power", spellParams.power }
        };
        if (data.damage_multiplier != null)
        {
            spellParams.damageModifiers.Add(new ValueModifier
            {
                modifierType = ValueModifier.type.MULTIPLY,
                value = RPNEvaluator.RPNEvaluator.Evaluatef(data.damage_multiplier, dict)
            });
        }
        if (data.mana_multiplier != null)
        {
            spellParams.manaModifiers.Add(new ValueModifier
            {
                modifierType = ValueModifier.type.MULTIPLY,
                value = RPNEvaluator.RPNEvaluator.Evaluatef(data.mana_multiplier, dict)
            });
        }
        if (data.cooldown_multiplier != null)
        {
            spellParams.cooldownModifiers.Add(new ValueModifier
            {
                modifierType = ValueModifier.type.MULTIPLY,
                value = RPNEvaluator.RPNEvaluator.Evaluatef(data.cooldown_multiplier, dict)
            });
        }
        if (data.speed_multiplier != null)
        {
            spellParams.speedModifiers.Add(new ValueModifier
            {
                modifierType = ValueModifier.type.MULTIPLY,
                value = RPNEvaluator.RPNEvaluator.Evaluatef(data.speed_multiplier, dict)
            });
        }
        if (data.mana_adder != null)
        {
            spellParams.manaModifiers.Add(new ValueModifier
            {
                modifierType = ValueModifier.type.ADD,
                value = RPNEvaluator.RPNEvaluator.Evaluatef(data.mana_adder, dict)
            });
        }
        if (data.projectile_trajectory != null)
        {
            spellParams.trajectoryOverride = data.projectile_trajectory;
        }
    }

    public override int GetManaCost()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.waveNum },
            { "power", owner.spellpower }
        };

        float ret = innerSpell.GetManaCost();
        if (data.mana_multiplier != null)
        {
            ret *= RPNEvaluator.RPNEvaluator.Evaluatef(data.mana_multiplier, dict);
        }
        if (data.mana_adder != null)
        {
            ret += RPNEvaluator.RPNEvaluator.Evaluatef(data.mana_adder, dict);
        }
        return Mathf.RoundToInt(ret);
    }

    public override float GetCooldown()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.waveNum },
            { "power", owner.spellpower }
        };

        float cooldown = innerSpell.GetCooldown();
        if (data.cooldown_multiplier != null)
        {
            return cooldown * RPNEvaluator.RPNEvaluator.Evaluatef(data.cooldown_multiplier, dict);
        }
        else
        {
            return cooldown;
        }
    }

    public override int GetDamage()
    {
        return innerSpell.GetDamage();
    }

    public override float calculateSpeed(SpellParams spellParams)
    {
        if (data.projectile == null)
            return innerSpell.calculateSpeed(spellParams);
        return base.calculateSpeed(spellParams);
    }
}
