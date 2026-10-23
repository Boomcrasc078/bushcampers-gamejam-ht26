
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIJitter : MonoBehaviour
{
    [Header("Normal Jitter")]
    [SerializeField] private float jitterStrength = 1f;
    [SerializeField] private float jitterInterval = 0.06f;

    [Header("Overload Glitches")]
    [SerializeField] private float glitchChance = 0.02f;
    [SerializeField] private float glitchStrength = 4f;
    [SerializeField] private float glitchDuration = 0.12f;

    private RectTransform rect;
    private Vector2 originalPosition;

    private float nextJitterTime;
    private float glitchEndTime;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPosition = rect.anchoredPosition;
    }

    private void Update()
    {
        if (Time.unscaledTime < nextJitterTime)
            return;

        nextJitterTime = Time.unscaledTime + Mathf.Max(0.01f, jitterInterval);

        if (Random.value < glitchChance)
            glitchEndTime = Time.unscaledTime + glitchDuration;

        float strength = Time.unscaledTime < glitchEndTime ? glitchStrength : jitterStrength;

        Vector2 offset = new Vector2(
            Mathf.Round(Random.Range(-strength, strength)),
            Mathf.Round(Random.Range(-strength, strength))
        );

        rect.anchoredPosition = originalPosition + offset;
    }

    private void OnDisable()
    {
        if (rect != null)
            rect.anchoredPosition = originalPosition;
    }
}
