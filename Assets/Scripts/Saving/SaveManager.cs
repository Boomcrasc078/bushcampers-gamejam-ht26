using UnityEngine;
using System;
using System.IO;

public static class SaveManager
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "progression.json");

    public static void Save(ProgressionSaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log("Progression saaved to " + savePath);
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to save progression: " + e.Message);
        }
    }

    public static ProgressionSaveData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save found, startug new");
            return new ProgressionSaveData();
        }


        try
        {
            string json = File.ReadAllText(savePath);

            ProgressionSaveData data = JsonUtility.FromJson<ProgressionSaveData>(json);

            if (data == null)
                throw new Exception("Save file empty or ivadlid");

            Debug.Log("Progression loaded correct");
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to load progression" + e.Message);
            return new ProgressionSaveData();
        }

    }


    public static void SaveCurrentProgression()
    {
        if(TrainingDataManager.instance == null || ShopUpgradeManager.instance == null)
        {
            Debug.LogWarning("Cannot save: managers missing");
            return;
        }

        ProgressionSaveData data = new ProgressionSaveData
        {
            trainingData = TrainingDataManager.instance.TrainingDataUnformatted,
            upgradeLevels = ShopUpgradeManager.instance.GetUpgradeLevels()
        };

        Save(data);
    }

    
    public static void LoadCurrentProgression()
    {
        if(TrainingDataManager.instance == null || ShopUpgradeManager.instance == null)
        {
            Debug.LogError("Cannot load : managers missing");
            return;
        }

        ProgressionSaveData data = Load();

        TrainingDataManager.instance.SetTrainingData(data.trainingData);
        ShopUpgradeManager.instance.LoadUpgradeLevels(data.upgradeLevels);

    }

}
