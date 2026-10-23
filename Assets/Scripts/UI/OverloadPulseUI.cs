
using TMPro;
using UnityEngine;

public class OverloadPulseUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Color readyColor = new Color(112f / 255f, 223f / 255f, 239f / 255f);
    [SerializeField] private Color emptyColor = new Color(0.70f, 0.77f, 0.85f);

    private OverloadPulseAbility ability;

    private void Update()
    {
        if (ability == null)
        {
            ability = FindAnyObjectByType<OverloadPulseAbility>();

            if (ability == null)
                return;
        }

        if (ability.CurrentCharges > 0)
        {
            statusText.text = "OVERLOAD [Q] READY";
            statusText.color = readyColor;
        }
        else
        {
            statusText.text = "OVERLOAD [Q] EMPTY";
            statusText.color = emptyColor;
        }
    }
}
