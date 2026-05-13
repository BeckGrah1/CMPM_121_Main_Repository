using Newtonsoft.Json.Linq;
using RPNEvaluator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    public float last_cast;
    public SpellCaster owner;
    public SpellData data;
    public Hittable.Team team;
    public Spell(SpellCaster owner, SpellData data)
    {
        this.owner = owner;
        this.data = data;
    }

    public string GetName()
    {
        return data.name;
    }

    public virtual int GetManaCost()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.waveNum},
            {"power", owner.spellpower }
        };
        return RPNEvaluator.RPNEvaluator.Evaluate(data.mana_cost, dict);
    }

    public int GetDamage()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.waveNum},
            {"power", owner.spellpower }
        };
        return RPNEvaluator.RPNEvaluator.Evaluate(data.damage.amount, dict);
    }

    public virtual float GetCooldown()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", GameManager.Instance.waveNum},
            {"power", owner.spellpower }
        };
        return RPNEvaluator.RPNEvaluator.Evaluatef(data.cooldown, dict);
    }

    public virtual int GetIcon()
    {
        return data.icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        last_cast = Time.time;

        SpellParams spellParams = new SpellParams();
        spellParams.wave = GameManager.Instance.waveNum;
        spellParams.power = owner.spellpower;

        float speed = calculateSpeed(spellParams);

        // moved onHit here so I can pass in suff like spellParams
        Action<Hittable, Vector3> onHit = (other, impact) => {
            if (other.team != team)
                other.Damage(new Damage(calculateDamage(spellParams), Damage.TypeFromString(data.damage.type)));
        };

        if (spellParams.trajectoryOverride != null)
        {
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, spellParams.trajectoryOverride, where, target - where, speed, onHit);
        }
        else
        {
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, data.projectile.trajectory, where, target - where, speed, onHit);
        }
        yield return new WaitForEndOfFrame();
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, SpellParams spellParams)
    {
        this.team = team;
        last_cast = Time.time;

        float speed = calculateSpeed(spellParams);

        // moved onHit here so I can pass in suff like spellParams
        Action<Hittable, Vector3> onHit = (other, impact) => {
            if (other.team != team)
                other.Damage(new Damage(calculateDamage(spellParams), Damage.TypeFromString(data.damage.type)));
        };

        if (spellParams.trajectoryOverride != null)
        {
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, spellParams.trajectoryOverride, where, target - where, speed, onHit);
        }
        else
        {
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, data.projectile.trajectory, where, target - where, speed, onHit);
        }
        yield return new WaitForEndOfFrame();
    }

    public float calculateSpeed(SpellParams spellParams)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", spellParams.wave },
            { "power", spellParams.power }
        };
        float baseSpeed = RPNEvaluator.RPNEvaluator.Evaluatef(data.projectile.speed, dict);
        float speed = ValueModifier.Apply(spellParams.speedModifiers, baseSpeed);
        return speed;
    }

    public int calculateDamage(SpellParams spellParams)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            { "wave", spellParams.wave },
            { "power", spellParams.power }
        };
        float baseDamage = RPNEvaluator.RPNEvaluator.Evaluatef(data.damage.amount, dict);
        int damage = (int)ValueModifier.Apply(spellParams.damageModifiers, baseDamage);
        return damage;
    }

}
