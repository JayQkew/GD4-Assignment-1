using UnityEngine;
using TMPro;
using System.Collections;

public class SmoothWeightAnimator : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    private Material fontMaterial;
    
    [Header("Sin Wave Animation Settings")]
    public float animationSpeed = 1f;
    public bool playOnStart = true;
    
    [Header("Weight Range")]
    public float minWeight = 0f; // Thin
    public float maxWeight = 1f; // Black/Bold
    
    private float currentWeight = 1f; // Start at black/bold weight
    private Coroutine sinAnimationCoroutine;
    
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        fontMaterial = new Material(textComponent.fontMaterial);
        textComponent.fontMaterial = fontMaterial;
        
        if (playOnStart)
            StartSinAnimation();
    }
    
    public void StartSinAnimation()
    {
        if (sinAnimationCoroutine != null)
            StopCoroutine(sinAnimationCoroutine);
            
        sinAnimationCoroutine = StartCoroutine(SinWeightAnimation());
    }
    
    public void StopSinAnimation()
    {
        if (sinAnimationCoroutine != null)
        {
            StopCoroutine(sinAnimationCoroutine);
            sinAnimationCoroutine = null;
        }
    }
    
    private IEnumerator SinWeightAnimation()
    {
        float time = 0f;
        
        while (true)
        {
            // Calculate sin wave value (-1 to 1)
            float sinValue = Mathf.Sin(time * animationSpeed);
            
            // Convert sin wave to weight range (0 to 1)
            // Sin wave goes from -1 to 1, we want 0 to 1
            float normalizedSin = (sinValue + 1f) / 2f;
            
            // Map to our weight range
            currentWeight = Mathf.Lerp(minWeight, maxWeight, normalizedSin);
            
            ApplyWeight(currentWeight);
            
            time += Time.deltaTime;
            yield return null;
        }
    }
    
    private void ApplyWeight(float weight)
    {
        // Map weight (0-1) to dilate range for better visual effect
        float dilate = Mathf.Lerp(-0.7f, 0.7f, weight);
        fontMaterial.SetFloat("_FaceDilate", dilate);
    }
    
    // Manual control methods (will stop sin animation)
    public void SetWeightManual(float weight)
    {
        StopSinAnimation();
        currentWeight = weight;
        ApplyWeight(weight);
    }
    
    public void SetThin() => SetWeightManual(0f);
    public void SetBold() => SetWeightManual(1f);
    
    void OnDestroy()
    {
        if (fontMaterial != null)
            DestroyImmediate(fontMaterial);
    }
}