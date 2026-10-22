
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text healthText;

    private Health playerHealth;
    private RectTransform healthFillRect;
    private float fullHealthBarWidth;

    private void Start()
    {
        healthFillRect = healthFill.rectTransform;
        fullHealthBarWidth = healthFillRect.rect.width;
    }

    private void Update()
    {
        if (playerHealth == null)
        {
            PlayerInput player = FindAnyObjectByType<PlayerInput>();

            if (player != null)
            {
                playerHealth = player.GetComponentInChildren<Health>();
            }

            if (playerHealth == null)
                return;
        }

        float current = playerHealth.CurrentHealth;
        float max = playerHealth.MaxHealth;


        float healthPercentage = Mathf.Clamp01(current / Mathf.Max(1f, max));


        healthFillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullHealthBarWidth * healthPercentage);


        healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)} HP";
    }
}
