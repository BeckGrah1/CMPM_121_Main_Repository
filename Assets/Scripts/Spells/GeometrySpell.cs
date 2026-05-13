using System.Collections.Generic;
using System.Collections;
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

            // make two new projectiles at 90 degree angles from the first
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, data.projectile.trajectory, impact, dir1, calculateSpeed(spellParams), null);
            GameManager.Instance.projectileManager.CreateProjectile(data.projectile.sprite, data.projectile.trajectory, impact, dir2, calculateSpeed(spellParams), null);
        };
        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}

