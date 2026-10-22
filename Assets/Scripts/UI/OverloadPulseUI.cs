
using TMPro;
using UnityEngine;

public class OverloadPulseUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;

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
            statusText.color = Color.cyan;
        }
        else
        {
            statusText.text = "OVERLOAD [Q] EMPTY";
            statusText.color = Color.gray;
        }
    }
}
