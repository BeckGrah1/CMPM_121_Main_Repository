using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string name;
    public int waves;
    public SpawnEntry[] spawns;
}

[System.Serializable]
public class SpawnEntry
{
    public string enemy;
    public string count;
    public string hp;
    public string delay;
    public int[] sequence;
    public string location;
    public string damage;
    public string speed;
}
