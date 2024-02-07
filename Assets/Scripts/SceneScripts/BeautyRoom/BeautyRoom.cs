using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BeautyRoom: MonoBehaviour
{
    public GameObject ItemPrefab;
    public Transform InventoryContent;

    public GameObject[] EquipItems;
    private Dictionary<int, GameObject> InventoryItems = new Dictionary<int, GameObject>();

    // Use this for initialization
    void Start()
	{
        CreateItems();
    }


    private void CreateItems()
    {
        var itemTable = ServerManager.Instance.ItemTable;
        var gender = GameManager.Instance.User.Gender;
        var items = GameManager.Instance.User.Items;

        for (int category = 0; category < EquipItems.Length; category++)
        {
            var equipItem = EquipItems[category].GetComponent<EquipItem>();
            equipItem.SetEquipItem(UnEquipItemFunc, category);
        }

        foreach (var item in items)
        {
            var itemId = item.Key;
            var isEquip = item.Value;
            var imageId = itemTable[itemId].ImgId;

            var itemName = itemTable[itemId].Name;
            
            var category = itemTable[itemId].Category;
            var equipItem = EquipItems[category].GetComponent<EquipItem>();
            
            GameObject itemObject = Instantiate(ItemPrefab, InventoryContent);
            var inventoryItem = itemObject.GetComponent<InventoryItem>();

            inventoryItem.SetInventoryItem(equipItem.EquipItemFunc, itemId, imageId, itemName, category, isEquip);

            InventoryItems[itemId] = itemObject;

        }
    }

    public void UnEquipItemFunc(int itemId)
    {
        var item = InventoryItems[itemId].transform.Find("IsEquip").gameObject;
        item.SetActive(false);

    }
}

