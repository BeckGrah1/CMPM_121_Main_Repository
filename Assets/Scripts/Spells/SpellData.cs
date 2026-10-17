using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpellData
{
    public string name;
    public string description;
    public int icon;
    public DamageData damage;
    public string mana_cost;
    public string cooldown;
    public ProjectileData projectile;

    // Currently unimplemented spell variants
    public string N;
    public string spray;
    public string secondary_damage;
    public ProjectileData secondary_projectile;

    // modifier spells
    public string damage_multiplier;
    public string mana_multiplier;
    public string cooldown_multiplier;
    public string mana_adder;
    public string speed_multiplier;
    public string delay;
    public string angle;
    public string projectile_trajectory;
}

[System.Serializable]
public class DamageData
{
    public string amount;
    public string type;
}

[System.Serializable]
public class ProjectileData
{
    public string trajectory;
    public string speed;
    public int sprite;
    public string lifetime;
}

