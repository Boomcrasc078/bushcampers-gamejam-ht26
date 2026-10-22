using System;

[Serializable]
public class ProgressionSaveData
{
    public int saveVersion = 1;

    public float trainingData = 0f;

    public int[] upgradeLevels = new int[0];
}
