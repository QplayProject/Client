using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Table;
using Util;

public class Shop: MonoBehaviour
{
    public GameObject[] EquipItems;

    public GameObject ShopItemPrefab;
    public Transform ShopContent;

    public GameObject InventoryItemPrefab;
    public Transform InventoryContent;

    public Text Money;

    private Dictionary<int, GameObject> ShopItems = new Dictionary<int, GameObject>();

    void Start()
    {
        CreateShopItems();
        CreateInventoryItems();
        GameManager.Instance.BuyItem = BuyItemFunc;
    }

    private void CreateShopItems()
    {
        var itemTable = ServerManager.Instance.ItemTable;
        var gender = GameManager.Instance.User.Gender;
        foreach(var sellItem in itemTable)
        {
            var item = itemTable[sellItem.Value.Id];
            var price = sellItem.Value.Price;
            if (item.Gender == gender)
            {
                GameObject itemObject = Instantiate(ShopItemPrefab, ShopContent);
                var shopItem = itemObject.GetComponent<ShopItem>();
                shopItem.SetShopItem(BuyItemFunc, EquipItemFunc, item.Id, price);

                ShopItems[item.Id] = itemObject;
            }
        }
    }

    private void CreateInventoryItems()
    {
        var user = GameManager.Instance.User;
        var itemTable = ServerManager.Instance.ItemTable;
        foreach (var itemInfo in user.Items)
        {
            var item = itemTable[itemInfo.Key];
            GameObject itemObject = Instantiate(InventoryItemPrefab, InventoryContent);

            var image = itemObject.transform.Find("Info").Find("Image").GetComponent<Image>();
            var path = GameManager.Instance.GetItemImagePath(item.Category, item.ImgId, true);
            image.sprite = Resources.Load<Sprite>(path);

            var name = itemObject.transform.Find("Name").GetComponent<Text>();
            name.text = itemTable[itemInfo.Key].Name;

            var isEquip = itemObject.transform.Find("IsEquip").gameObject;
            if (!itemInfo.Value) isEquip.SetActive(false);
        }


    }

    //TODO: 서버에 구매 호출 추가해야함
    public void BuyItemFunc(int itemId)
    {
        var user = GameManager.Instance.User;
        var webServer = new ApiServer();
        var buyItem = new ApiRequest.BuyItem();

        buyItem.UserName = user.UserName;
        buyItem.ItemId = itemId;

        StartCoroutine(webServer.ApiRequestBuyItem(buyItem));
       
    }

    public void EquipItemFunc(string itemImageId, int category)
    {
        Debug.Log($"ImageId :{itemImageId} / Category :{category}");
        var image = EquipItems[category].GetComponent<Image>();
        var path = GameManager.Instance.GetItemImagePath(category, itemImageId);
        image.sprite = Resources.Load<Sprite>(path);
        EquipItems[category].SetActive(true);
    }

    private void BuyItemFunc(ApiResponse.BuyItem response)
    {
        if (response.MessageCode == (int)MessageCode.Success)
        {
            Money.text = response.Money.ToString();

            var itemId = response.ItemId;

            var itemTable = ServerManager.Instance.ItemTable;

            var item = itemTable[itemId];
            GameObject itemObject = Instantiate(InventoryItemPrefab, InventoryContent);

            var image = itemObject.transform.Find("Info").Find("Image").GetComponent<Image>();
            var path = GameManager.Instance.GetItemImagePath(item.Category, item.ImgId, true);
            image.sprite = Resources.Load<Sprite>(path);

            var name = itemObject.transform.Find("Name").GetComponent<Text>();
            name.text = itemTable[itemId].Name;

            var isEquip = itemObject.transform.Find("IsEquip").gameObject;
            isEquip.SetActive(false);

            ServerManager.Instance.OpenMessageBox(response.Message);
        }
        
    }
}

