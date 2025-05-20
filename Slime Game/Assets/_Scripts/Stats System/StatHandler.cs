using System;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [SerializeField] private PlayerStats baseStats;
    [SerializeField] private Stat[] stats;

    private void Awake() {
        stats = baseStats.stats;
    }
}
