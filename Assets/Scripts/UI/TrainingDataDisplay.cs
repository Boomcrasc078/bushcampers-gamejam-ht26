
using UnityEngine;

public static class TrainingDataDisplay
{
    public static string FormatTrainingData(float amount)
    {
        if (amount < 1000f)
            return $"{amount:0} Byte";

        if (amount < 1000000f)
            return $"{amount / 1000f:0.###} kB";

        if (amount < 1000000000f)
            return $"{amount / 1000000f:0.######} MB";

        if (amount < 1000000000000f)
            return $"{amount / 1000000000f:0.#########} GB";

        if (amount < 1000000000000000f)
            return $"{amount / 1000000000000f:0.############} TB";

        return $"{amount / 1000000000000000f:0.###############} PB";
    }
}
