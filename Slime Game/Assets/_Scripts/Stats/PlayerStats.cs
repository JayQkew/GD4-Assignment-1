using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private Stat[] stats;
    
    private void Awake() {
        // Create deep copies of the stats instead of just copying references
        stats = new Stat[baseStats.stats.Length];
        for (int i = 0; i < baseStats.stats.Length; i++) {
            stats[i] = baseStats.stats[i].Copy();
        }
    }

    public float GetStatValue(StatName stat) {
        foreach (Stat s in stats) {
            if (s.name == stat) return s.value;
        }
        return 0;
    }

    public Stat GetStat(StatName stat) {
        foreach (Stat s in stats) {
            if (s.name == stat) return s;
        }
        return null;
    }

    /// <summary>
    /// Call this function when adding a card to the players deck
    /// </summary>
    /// <param name="modifier">the modifier of the card</param>
    public void ModifyStat(Modifier modifier) {
        Stat stat = GetStat(modifier.affectedStat);
        stat.ApplyModifier(modifier);
    }

    public void ResetStats() {
        // Create fresh copies from the base stats again
        stats = new Stat[baseStats.stats.Length];
        for (int i = 0; i < baseStats.stats.Length; i++) {
            stats[i] = baseStats.stats[i].Copy();
        }
    }
}