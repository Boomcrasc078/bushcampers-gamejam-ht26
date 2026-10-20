using UnityEngine;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemButtonText;
    private string purchaseText;
    private string upgradeText;
    private Upgrade upgrade;
    private ShopUpgradeManager upgradeManager;

    void Awake()
    {
        upgradeManager = ShopUpgradeManager.instance;
    }

    public void OnButtonPress()
    {
        upgradeManager.Upgrade(upgrade);
    }

    public void SetItem(string name, string description, int upgradeIndex, string purchaseText, string upgradeText)
    {
        itemNameText.text = name;
        itemDescriptionText.text = description;
        upgrade = upgradeManager.Upgrades[upgradeIndex];
        this.purchaseText = purchaseText;
        this.upgradeText = upgradeText;
    }

    void Update()
    {
        if (upgradeManager.ActivatedUpgrades.Contains(upgrade))
        {
            itemButtonText.text = upgradeText;
        }
        else
        {
            itemButtonText.text = purchaseText;
        }
    }
}
