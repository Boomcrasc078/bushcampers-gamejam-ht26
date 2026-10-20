using System;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class ShopUpgradeManager : MonoBehaviour
{
    public static ShopUpgradeManager instance;
    public Upgrade[] Upgrades => upgrades;
    public HashSet<Upgrade> ActivatedUpgrades => activatedUpgrades;
    private Upgrade[] upgrades;
    private HashSet<Upgrade> activatedUpgrades = new HashSet<Upgrade>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        AddUpgrades();
    }

    void AddUpgrades()
    {
        upgrades = new Upgrade[]
        {
            new SpeedUpgrade(1.2f)
        };
    }

    public void Upgrade(Upgrade upgrade)
    {

        if (ActivatedUpgrades.Contains(upgrade))
        {
            upgrade.Levelup();
            return;
        }

        activatedUpgrades.Add(upgrade);
    }

}