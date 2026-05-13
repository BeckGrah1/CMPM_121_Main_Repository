using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySpell : ModifierSpell
{
    public GeometrySpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data, innerSpell)
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
        innerSpell.additionalOnHit = (other, impact) =>
        {
            Vector3 dir1 = Quaternion.Euler(0, 0, 90) * (target - where);
            Vector3 dir2 = Quaternion.Euler(0, 0, -90) * (target - where);

            Action<Hittable, Vector3> sideOnHit = (other, impact) => {
                if (other.team != team)
                    other.Damage(new Damage(innerSpell.calculateDamage(spellParams), Damage.TypeFromString(innerSpell.data.damage.type)));
            };

            // make two new projectiles at 90 degree angles from the first
            GameManager.Instance.projectileManager.CreateProjectile(innerSpell.data.projectile.sprite, innerSpell.data.projectile.trajectory, impact, dir1, calculateSpeed(spellParams), sideOnHit);
            GameManager.Instance.projectileManager.CreateProjectile(innerSpell.data.projectile.sprite, innerSpell.data.projectile.trajectory, impact, dir2, calculateSpeed(spellParams), sideOnHit);
        };
        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}

