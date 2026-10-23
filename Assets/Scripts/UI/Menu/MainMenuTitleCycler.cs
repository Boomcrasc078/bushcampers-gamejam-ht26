
using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuTitleCycler : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private float changeInterval = 0.4f;

    [Header("Possible Names")]
    [SerializeField]
    private string[] names =
    {
        "FUEL AI",
        "FEAR AI",
        "FISH AI",
        "FILL AI",
        "FAIL AI",
        "FIRE AI",
        "FILE AI",
        "FUSE AI",
        "FAKE AI",
        "FEED AI",
        "FIZZ AI",
        "FOOL AI"
    };

    [Header("Random Color Settings")]
    [SerializeField, Range(0f, 1f)] private float minSaturation = 0.5f;
    [SerializeField, Range(0f, 1f)] private float maxSaturation = 0.85f;
    [SerializeField, Range(0f, 1f)] private float minBrightness = 0.8f;
    [SerializeField, Range(0f, 1f)] private float maxBrightness = 1f;

    private int previousNameIndex = -1;

    private IEnumerator Start()
    {
        if (titleText == null || names.Length == 0)
            yield break;

        while (true)
        {
            ChangeTitle();
            yield return new WaitForSecondsRealtime(changeInterval);
        }
    }

    private void ChangeTitle()
    {
        int index = Random.Range(0, names.Length);

        if (names.Length > 1)
        {
            while (index == previousNameIndex)
                index = Random.Range(0, names.Length);
        }

        previousNameIndex = index;
        titleText.text = names[index];

        titleText.color = Random.ColorHSV(
            0f, 1f,
            minSaturation, maxSaturation,
            minBrightness, maxBrightness
        );
    }
}
