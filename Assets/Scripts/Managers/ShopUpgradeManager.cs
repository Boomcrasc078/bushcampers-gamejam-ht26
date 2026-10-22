using System.Collections.Generic;
using UnityEngine;

public class ShopUpgradeManager : MonoBehaviour
{
    public static ShopUpgradeManager instance;

    [SerializeField] private Upgrade[] upgrades;

    private HashSet<Upgrade> activatedUpgrades = new HashSet<Upgrade>();

    public Upgrade[] Upgrades => upgrades;
    public HashSet<Upgrade> ActivatedUpgrades => activatedUpgrades;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void Upgrade(Upgrade upgrade)
    {

        if (ActivatedUpgrades.Contains(upgrade))
        {
            upgrade.Levelup();
            return;
        }

        activatedUpgrades.Add(upgrade);
        upgrade.Levelup();
    }

    //find by type
    public T GetUpgrade<T>() where T : Upgrade
    {
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade is T matchingUpgrade)
                return matchingUpgrade;
        }
        return null;
    }


    //collect to save
    public int[] GetUpgradeLevels()
    {
        int[] levels = new int[upgrades.Length];

        for(int i = 0; i < upgrades.Length; i++)
        {
            levels[i] = upgrades[i] != null ? upgrades[i].Level : 0;
        }
        return levels;
    }

    //load from save

    public void LoadUpgradeLevels(int[] savedLevels)
    {
        activatedUpgrades.Clear();

        for(int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i] == null)
                continue;

            int level = 0;

            if (savedLevels != null && i < savedLevels.Length)
                level = savedLevels[i];

            upgrades[i].SetLevel(level);

            if (upgrades[i].Level > 0)
                activatedUpgrades.Add(upgrades[i]);
        }
    }

}