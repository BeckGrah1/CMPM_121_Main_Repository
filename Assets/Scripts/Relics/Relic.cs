using UnityEngine;

public class Relic
{
    public string name;
    public int sprite;
    public RelicTrigger trigger;
    public RelicEffect effect;

    public string GetLabel()
    {
        return (name + ":\n" + trigger.description + " " + effect.description);
    }
}
