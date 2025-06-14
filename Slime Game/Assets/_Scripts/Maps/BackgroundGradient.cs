using UnityEngine;

public class BackgroundGradient : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    void Start() {
        ApplyGradientColors();
    }

    void ApplyGradientColors() {
        if (spriteRenderers == null || spriteRenderers.Length == 0) {
            Debug.LogWarning("No sprite renderers assigned!");
            return;
        }

        if (gradient == null) {
            Debug.LogWarning("No gradient assigned!");
            return;
        }

        for (int i = 0; i < spriteRenderers.Length; i++) {
            if (spriteRenderers[i] != null) {
                float t = spriteRenderers.Length == 1 ? 0f : (float)i / (spriteRenderers.Length - 1);
                Color color = gradient.Evaluate(t);
                spriteRenderers[i].color = color;
            }
        }
    }

    // Call this method to update colors at runtime
    [ContextMenu("Update Gradient Colors")]
    public void UpdateGradientColors() {
        ApplyGradientColors();
    }

    // Optional: Update colors when values change in editor
    void OnValidate() {
        if (Application.isPlaying) {
            ApplyGradientColors();
        }
    }
}