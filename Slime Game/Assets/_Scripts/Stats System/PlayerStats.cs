using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private Stat[] stats;
    private void Awake() {
        stats = baseStats.stats;
    }

    public float GetStat(StatName stat) {
        foreach (Stat s in stats) {
            if (s.name == stat) return s.value;
        }
        return 0;
    }
}
