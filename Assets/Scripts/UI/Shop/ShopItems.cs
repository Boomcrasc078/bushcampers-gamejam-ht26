using System.Data;
using UnityEngine;

public class ShopItems : MonoBehaviour
{
    [SerializeField] private ShopItem[] items;
    [SerializeField] private GameObject shopItemPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var item in items)
        {
            var currentItemGameobject = Instantiate(shopItemPrefab, transform);
            var shopItemUI = currentItemGameobject.GetComponent<ShopItemUI>();
            shopItemUI.SetItem(item.itemName, item.itemDescription, item.upgradeIndex, item.purchaseText, item.upgradeText);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
