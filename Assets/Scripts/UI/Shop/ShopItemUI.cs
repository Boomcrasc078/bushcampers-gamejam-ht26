using UnityEngine;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemButtonText;
    [SerializeField] private TMP_Text itemCostText;
    private ShopItem shopItem;

    private Upgrade upgrade;
    private ShopUpgradeManager upgradeManager;

    void Awake()
    {
        upgradeManager = ShopUpgradeManager.instance;
    }

    public void OnButtonPress()
    {
        //Fixa kostnad
        upgradeManager.Upgrade(upgrade);
    }

    public void SetItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;
        itemNameText.text = shopItem.name;
        itemDescriptionText.text = shopItem.itemDescription;
        upgrade = upgradeManager.Upgrades[shopItem.upgradeIndex];
    }

    private float GetCost()
    {
        return Mathf.Round(shopItem.itemBaseCost * Mathf.Pow(shopItem.itemCostMultiplier, upgrade.Level));
    }

    void Update()
    {
        if (upgradeManager.ActivatedUpgrades.Contains(upgrade))
        {
            itemButtonText.text = shopItem.upgradeText;
        }
        else
        {
            itemButtonText.text = shopItem.purchaseText;
        }

        itemCostText.text = $"{TrainingDataDisplay.FormatTrainingData(GetCost())} REQUIRED";
    }
}
