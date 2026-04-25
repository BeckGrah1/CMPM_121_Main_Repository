using UnityEngine;

// Holds each of the differet attributes from enemy JSON file
[System.Serializable]
public class EnemyData
{
    public string name;
    public int sprite;
    // should change to strings when we implement the RPNEvaluator stuff
    public int hp;
    public int speed;
    public int damage;
}
