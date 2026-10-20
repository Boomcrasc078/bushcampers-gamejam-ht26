using UnityEngine;

public static class TrainingDataDisplay
{
    public static string FormatTrainingData(float amount)
    {
        if (amount < 1000f)
        {
            return Mathf.Round(amount) + " Byte";
        }
        else if (amount < 1000000f)
        {
            return Mathf.Round(amount / 1000f) + " kB";
        }
        else if (amount < 1000000000f)
        {
            return Mathf.Round(amount / 1000000f) + " MB";
        }
        else if (amount < 1000000000000f)
        {
            return Mathf.Round(amount / 1000000000f) + " GB";
        }
        else if (amount < 1000000000000000f)
        {
            return Mathf.Round(amount / 1000000000000f) + " TB";
        }
        else
        {
            return Mathf.Round(amount / 1000000000000000f) + " PB";
        }
    }
}