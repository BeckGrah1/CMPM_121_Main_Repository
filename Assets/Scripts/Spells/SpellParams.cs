using UnityEngine;
using System.Collections.Generic;

public class SpellParams
{
    public List<ValueModifier> damageModifiers = new();
    public List<ValueModifier> manaModifiers = new();
    public List<ValueModifier> speedmodifiers = new();
    public List<ValueModifier> cooldownModifiers = new();
    public string trajectoryOverride = null;
    public int wave;
    public int power;
}
