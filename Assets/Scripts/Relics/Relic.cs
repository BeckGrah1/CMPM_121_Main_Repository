using UnityEngine;

[System.Serializable]
public class Relic
{
    public Trigger trigger;
    public Effect effect;
}

[System.Serializable]
public class Trigger
{
    string description;
    string type;
    string amount;
}

[System.Serializable]
public class Effect
{
    string description;
    string type;
    string amount;
    string until;
}
