using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class Info : MonoBehaviour
{
	public Image Background;
	public Image Model;
	public Image[] EquipItems;
    public Text Money;
	// Use this for initialization

	void Start()
	{
        GameManager.Instance.ChangeModel = LoadCharacter;
        Debug.Log("LoadCharacter");
        LoadCharacter();
    }

	public void CharacterEquipItemFunc(int itemId, int category)
	{
        Debug.Log($"CharacterEquipItemFunc:: ItemId : {itemId} / Category : {category}");
        
        var itemTable = ServerManager.Instance.ItemTable;
        var user = GameManager.Instance.User;
        
        string imageId = itemTable[itemId].ImgId;
        
        var path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, category, imageId);
        path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, category, imageId);
        EquipItems[category].sprite = Resources.Load<Sprite>(path);
        EquipItems[category].gameObject.SetActive(true);

        var items = user.Items;

        foreach (var oldItem in items)
        {
            var itemCategory = itemTable[oldItem.Key].Category;
            if (itemCategory == category && oldItem.Value)
            {
                items[oldItem.Key] = false;
                Debug.Log("AlreadyItemId:"+ oldItem.Key);
                break;
            }
        }

        items[itemId] = true;
    }
    public void CharacterUnEquipItemFunc(int itemId, int category)
    {
        Debug.Log($"CharacterUnEquipItemFunc:: Category : {category}");
        EquipItems[category].gameObject.SetActive(false);

        var user = GameManager.Instance.User;
        var items = user.Items;
        items[itemId] = false;
    }

    public void LoadCharacter()
    {
        var user = GameManager.Instance.User;
        var items = user.Items;
        var itemTable = ServerManager.Instance.ItemTable;

        //-- 성형 이미지 가져오기
        string modelPath = GameManager.Instance.GetModelImagePath(user.Gender, user.Model);
        Model.sprite = Resources.Load<Sprite>(modelPath);

        for (int i = 0; i < EquipItems.Length; i++)
        {
            EquipItems[i].gameObject.SetActive(false);
        }
        Background.gameObject.SetActive(false);

        foreach (var itemData in items)
        {
            if (!itemData.Value) continue;

            int category = itemTable[itemData.Key].Category;
            string imageId = itemTable[itemData.Key].ImgId;

            var path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, category, imageId);
            EquipItems[category].sprite = Resources.Load<Sprite>(path);
            EquipItems[category].gameObject.SetActive(true);

        }

        Money.text = user.Money.ToString();
    }

}

