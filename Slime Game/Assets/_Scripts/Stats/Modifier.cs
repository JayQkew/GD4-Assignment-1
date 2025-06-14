using System;
using UnityEngine;

[Serializable]
public class Modifier
{
    public StatName affectedStat;
    public ModifierType type;
    public float value;
}

public enum ModifierType
{
    Mult,
    Add
}