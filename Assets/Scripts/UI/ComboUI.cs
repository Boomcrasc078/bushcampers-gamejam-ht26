
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RunManager runManager;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private TMP_Text killCountText;
    [SerializeField] private Image timerFill;
    [SerializeField] private RectTransform timerBackgroundRect;
    [SerializeField] private RectTransform multiplierRect;
    [SerializeField] private TMP_Text nextTierText;

    [Header("Animation")]
    [SerializeField] private float pulseScale = 1.25f;
    [SerializeField] private float growDuration = 0.08f;
    [SerializeField] private float shrinkDuration = 0.12f;

    private Vector3 originalScale;
    private float previousMultiplier = 1f;
    private Coroutine pulseCoroutine;

    private void Awake()
    {
        originalScale = multiplierRect.localScale;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Update()
    {
        if (runManager == null)
            return;

        int kills = runManager.ComboKills;
        bool showCombo = kills >= 2;

        canvasGroup.alpha = showCombo ? 1f : 0f;

        if (!showCombo)
        {
            previousMultiplier = 1f;

            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
                pulseCoroutine = null;
            }

            multiplierRect.localScale = originalScale;
            return;
        }

        float multiplier = runManager.ComboMultiplier;

        multiplierText.text = $"{multiplier:0.00}x DATA";

        killCountText.text = $"{kills} KILL CHAIN";
        int killsRemaining = runManager.KillsToNextComboTier;

        nextTierText.text = killsRemaining > 0
            ? $"{killsRemaining} KILL{(killsRemaining == 1 ? "" : "S")} TO NEXT LEVEL"
            : "MAXIMUM DATA EXTRACTION";

        float timePercentage = Mathf.Clamp01(runManager.ComboTimeRemaining / Mathf.Max(0.01f, runManager.ComboDuration));
        float fullTimerWidth = timerBackgroundRect.rect.width;

        timerFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,fullTimerWidth * timePercentage
        );


        if (multiplier > previousMultiplier + 0.001f)
        {
            if (pulseCoroutine != null) 
                StopCoroutine(pulseCoroutine);

            multiplierRect.localScale = originalScale;
            pulseCoroutine = StartCoroutine(PulseMultiplier());
        }

        previousMultiplier = multiplier;
    }

    private IEnumerator PulseMultiplier()
    {
        yield return ScaleMultiplier(originalScale * pulseScale, growDuration);
        yield return ScaleMultiplier(originalScale, shrinkDuration);

        pulseCoroutine = null;
    }

    private IEnumerator ScaleMultiplier(Vector3 targetScale, float duration)
    {
        Vector3 startScale = multiplierRect.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / Mathf.Max(0.001f, duration));
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            multiplierRect.localScale = Vector3.Lerp(startScale, targetScale, smoothProgress);

            yield return null;
        }

        multiplierRect.localScale = targetScale;
    }
}
