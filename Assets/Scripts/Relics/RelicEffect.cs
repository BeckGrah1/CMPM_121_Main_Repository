using UnityEngine;

public class RelicEffect
{
    public string description;
    public bool active = false;
    protected void RegisterUntil(string until)
    {
        if (until == null) return;
        switch (until)
        {
            case "cast-spell":
                EventBus.Instance.OnSpellCast += StopEffect;
                break;
            case "move":
                EventBus.Instance.OnPlayerMoved += StopEffect;
                break;
            case "take-damage":
                EventBus.Instance.OnPlayerDamaged += (dmg) => StopEffect();
                break;
            default:
                Debug.LogWarning("until type: " + until + "not recognized, make sure to add to the switch in RelicEffect");
                break;
        }
    }
    public virtual void DoEffect()
    {

    }

    public virtual void StopEffect()
    {

    }

    public virtual void UpdateRPN()
    {

    }

    // can be overrided to return true
    public virtual bool IsActive()
    {
        return false;
    }
}
