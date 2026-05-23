using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometrySpell : ModifierSpell
{
    public GeometrySpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data, innerSpell)
    {
    }

    private bool onHitRegistered = false;

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team, SpellParams spellParams)
    {
        ApplyModifiers(spellParams);

        if (!onHitRegistered)
        {
            innerSpell.GetBaseSpell().additionalOnHit += (other, impact) =>
            {
                SpellParams freshParams = new SpellParams
                {
                    wave = GameManager.Instance.waveNum,
                    power = owner.spellpower
                };

                string trajectory = freshParams.trajectoryOverride ?? innerSpell.GetBaseSpell().data.projectile.trajectory;
                Vector3 toEnemy = (other.owner.transform.position - impact).normalized;
                Vector3 dir1 = Quaternion.Euler(0, 0, 90) * toEnemy;
                Vector3 dir2 = Quaternion.Euler(0, 0, -90) * toEnemy;

                Action<Hittable, Vector3> sideOnHit = (sideOther, sideImpact) => {
                    if (sideOther.team != team)
                        sideOther.Damage(new Damage(innerSpell.calculateDamage(freshParams), Damage.TypeFromString(innerSpell.GetBaseSpell().data.damage.type)));
                };

                GameManager.Instance.projectileManager.CreateProjectile(innerSpell.GetBaseSpell().data.projectile.sprite, trajectory, impact, dir1, innerSpell.calculateSpeed(freshParams), sideOnHit);
                GameManager.Instance.projectileManager.CreateProjectile(innerSpell.GetBaseSpell().data.projectile.sprite, trajectory, impact, dir2, innerSpell.calculateSpeed(freshParams), sideOnHit);
            };
            onHitRegistered = true;
        }

        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}

