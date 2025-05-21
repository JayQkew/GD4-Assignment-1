using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public StatName name;
    public float value;
    [SerializeField] private Vector2 clamp;   // x is min, y is max

    public void ApplyModifier(Modifier modifier) {
        value = modifier.type == ModifierType.Add ? 
            Mathf.Clamp(value + modifier.value, clamp.x, clamp.y) : 
            Mathf.Clamp(value * modifier.value, clamp.x, clamp.y);
    }
}
public enum StatName
{
    Fuel,
    MoveSpeed,
    MinRadius,
    MaxRadius,
    MinFrequency,
    MaxFrequency,
    DashCooldown,
    DashForce,
    DashCost,
    MoveCost,
    InflateTime
}
