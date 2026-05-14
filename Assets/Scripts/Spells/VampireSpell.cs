using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class VampireSpell : ModifierSpell
{
    private int healAmount = 20;
    public VampireSpell(SpellCaster owner, SpellData data, Spell innerSpell) : base(owner, data, innerSpell)
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
                Hittable playerHp = GameManager.Instance.player.GetComponent<PlayerController>().hp;
                playerHp.hp = Mathf.Min(playerHp.hp + healAmount, playerHp.max_hp);
                Debug.Log(playerHp.hp);
            };
            onHitRegistered = true;
        }
        yield return innerSpell.Cast(where, target, team, spellParams);
    }
}

