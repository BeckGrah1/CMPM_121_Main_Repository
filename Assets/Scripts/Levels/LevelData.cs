using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string difficulty;
    public int waveCount;
    public List<SpawnEntry> spawns;
}

[System.Serializable]
public class SpawnEntry
{
    public string enemy;
    public string count;
    public string hp;
    public string delay;
    public List<int> sequence;
    public string location;
    public string damage;
}
