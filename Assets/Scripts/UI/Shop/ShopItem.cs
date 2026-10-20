using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    public string itemName = "An Item";
    public string itemDescription = "This is an item";
    public float itemBaseCost = 30f;
    public float itemCostMultiplier = 1.2f;
    public int upgradeIndex = 0;
    public string purchaseText = "Learn";
    public string upgradeText = "Improve";
}
