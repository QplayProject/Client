using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private int ItemId;
    private string ItemImageId;
    private string ItemName;
    private int Category;

    private int Price;
    private Action<int> BuyItem;
    private Action<string, int> EquipItem;
    private Image ItemImage;
    private Text ItemNameText;
    private Text ItemPriceText;

    public void SetShopItem(Action<int> buyItem, Action<string, int> equipItem, int itemId, int price)
    {
        var buyButton = transform.Find("BuyButton").GetComponent<Button>();
        buyButton.onClick.AddListener(BuyItemFunc);

        var equipButton = gameObject.GetComponent<Button>();
        equipButton.onClick.AddListener(EquipItemFunc);

        BuyItem = buyItem;
        EquipItem = equipItem;
        Init(itemId);
        Price = price;


        ItemImage = transform.Find("Item").transform.Find("Image").GetComponent<Image>();
        var path = GameManager.Instance.GetItemImagePath(Category, ItemImageId, true);
        ItemImage.sprite = Resources.Load<Sprite>(path);

        var info = transform.Find("Info").gameObject.transform;
        ItemNameText = info.Find("ItemName").GetComponent<Text>();
        ItemNameText.text = ItemName;

        ItemPriceText = info.Find("ItemPrice").GetComponent<Text>();
        ItemPriceText.text = Price.ToString();
    }

    private void Init(int itemId)
    {
        var itemTable = ServerManager.Instance.ItemTable;
        var item = itemTable[itemId];
        ItemId = itemId;
        ItemImageId = item.ImgId;
        ItemName = item.Name;
        Category = item.Category;
    }

    public void BuyItemFunc()
    {
        BuyItem?.Invoke(ItemId);
    }

    public void EquipItemFunc()
    {
        EquipItem?.Invoke(ItemImageId, Category);
    }
}

