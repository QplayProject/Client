using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Util;
using System.Collections.Generic;
using Table;
using System;



public class EquipItem : MonoBehaviour
{
    public GameObject Chracter;

    public Image ItemImage;
    public Text ItemNameText;
    [SerializeField]
    public int ItemId;
    private string ImageId;
    private string ItemName;
    private int Category;

    private Action<int> UnEquipItemFunc;
    private Action<int, int> CharacterUnEquipItemFunc;
    private Action<int, int> CharacterEquipItemFunc;

    public void SetEquipItem(Action<int> callback, int category)
    {
        ItemImage.gameObject.SetActive(false);
        ItemNameText.gameObject.SetActive(false);

        UnEquipItemFunc = callback;
        var info = Chracter.GetComponent<Info>();
        CharacterUnEquipItemFunc = info.CharacterUnEquipItemFunc;
        CharacterEquipItemFunc = info.CharacterEquipItemFunc;
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        var itemTable = ServerManager.Instance.ItemTable;
        var items = user.Items;
        foreach (var item in items)
        {
            var Item = itemTable[item.Key];
            var isEquip = item.Value;
            if (Item.Category == category && isEquip)
            {
                ItemId = item.Key;
                ImageId = Item.ImgId;
                ItemName = Item.Name;
                Category = category;

                //-- 인벤토리 아이템 이미지
                var path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, Category, ImageId, true);
                ItemImage.sprite = Resources.Load<Sprite>(path);
                ItemNameText.text = ItemName;

                ItemImage.gameObject.SetActive(true);
                ItemNameText.gameObject.SetActive(true);
                break;
            }
        }

    }

    public void EquipItemFunc(int itemId, string imageId, string itemName, int category)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        var itemTable = ServerManager.Instance.ItemTable;
        var items = user.Items;
        Debug.Log("EquipItem:" + itemId);
        foreach (var item in items)
        {
            var itemCategory = itemTable[item.Key].Category;
            var isEquip = item.Value;
            if (item.Key == itemId) continue;
            if (itemCategory == category && isEquip)
            {
                isEquip = false;
                UnEquipItemFunc?.Invoke(item.Key);
            }
        }

        ItemId = itemId;
        ImageId = imageId;
        ItemName = itemName;
        Category = category;

        //-- 착용 아이템 이미지
        var path = gameManager.GetItemImagePath(user.Model, user.Gender, Category, ImageId, true);
        ItemImage.sprite = Resources.Load<Sprite>(path);
        //-- 아이템명 설정
        ItemNameText.text = ItemName;

        ItemImage.gameObject.SetActive(true);
        ItemNameText.gameObject.SetActive(true);
        CharacterEquipItemFunc?.Invoke(itemId, category);
    }

    //TODO: BeautyRoom에 해제하는 아이템 Id전송시 BeautyRoom에서 해당 키값인 인벤 아이템 셋팅
    public void UnEquipItem()
    {
        ItemImage.gameObject.SetActive(false);
        ItemNameText.gameObject.SetActive(false);

        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        var items = user.Items;
        items[ItemId] = false;

        UnEquipItemFunc?.Invoke(ItemId);

        var itemTable = ServerManager.Instance.ItemTable;
        var category = itemTable[ItemId].Category;
        CharacterUnEquipItemFunc?.Invoke(ItemId, category);
    }
}

