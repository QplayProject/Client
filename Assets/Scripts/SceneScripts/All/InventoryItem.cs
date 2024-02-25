using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryItem : MonoBehaviour
{
    public Button Button;
    private Action<int, string, string, int> EquipItemFunc;

    private GameObject Equip;
    private Text ItemNameText;
    private Image ItemImage;
    [SerializeField]
    private int ItemId;
    [SerializeField]
    private string ImageId;
    [SerializeField]
    private string ItemName;
    [SerializeField]
    private int Category;
    [SerializeField]
    private bool IsEquip;


    public void SetInventoryItem(Action<int, string, string, int> callback, int itemId, string imageId, string itemName, int category, bool isEquip)
    {
        Button.onClick.AddListener(EuipItem);

        var item = gameObject.transform;
        ItemNameText = item.Find("Name").GetComponent<Text>();
        Equip = item.Find("IsEquip").gameObject;
        ItemImage = item.Find("Info").transform.Find("Image").GetComponent<Image>();

        EquipItemFunc = callback;
        ItemId = itemId;
        ImageId = imageId;
        ItemName = itemName;
        Category = category;
        IsEquip = isEquip;

        var user = GameManager.Instance.User;
        //-- 인벤토리 아이템 이미지
        var path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, Category, ImageId, true);
        ItemImage.sprite = Resources.Load<Sprite>(path);

        //-- 아이템명 설정
        ItemNameText.text = ItemName;

        Equip.SetActive(IsEquip);
    }

    private void EuipItem()
    {
        Equip.SetActive(true);
        EquipItemFunc?.Invoke(ItemId, ImageId, ItemName, Category);
    }


    public void UnEquipItem()
    {

    }
}

