using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Util;
using static UnityEditor.Progress;

public class Info : MonoBehaviour
{
	public Image Background;
	public Image Model;
	public Image Cloth;
	public Image Hair;
	public Image Ears;
	public Text Money;
	// Use this for initialization
	void Start()
	{
		var user = GameManager.Instance.User;
		var items = user.Items;
		var itemTable = ServerManager.Instance.ItemTable;

		//-- 성형 이미지 가져오기
		string modelPath = GameManager.Instance.GetModelImagePath(user.Gender, user.Model);
		Model.sprite = Resources.Load<Sprite>(modelPath);

		Cloth.gameObject.SetActive(false);
        Hair.gameObject.SetActive(false);
        Ears.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);

        foreach (var itemData in items)
		{
			if (!itemData.Value) continue;

			var path = "";

			int category = itemTable[itemData.Key].Category;
			string imageId = itemTable[itemData.Key].ImgId;

			switch(category)
			{
				case (int)Category.Hair:
					path = GameManager.Instance.GetItemImagePath((int)Category.Hair, imageId);
					Hair.sprite = Resources.Load<Sprite>(path);
                    Hair.gameObject.SetActive(true);
                    break;
				case (int)Category.Cloth:
                    path = GameManager.Instance.GetItemImagePath((int)Category.Cloth, imageId);
                    Cloth.sprite = Resources.Load<Sprite>(path);
                    Cloth.gameObject.SetActive(true);
                    break;
				case (int)Category.Ears:
                    path = GameManager.Instance.GetItemImagePath((int)Category.Ears, imageId);
                    Ears.sprite = Resources.Load<Sprite>(path);
					Ears.gameObject.SetActive(true);
                    break;
				case (int)Category.Background:
                    path = GameManager.Instance.GetItemImagePath((int)Category.Background, imageId);
                    Background.sprite = Resources.Load<Sprite>(path);
                    Background.gameObject.SetActive(true);
                    break;
			}

		}

		Money.text = GameManager.Instance.User.Money.ToString();
    }

	public void CharacterEquipItemFunc(int itemId, int category)
	{
        Debug.Log($"CharacterEquipItemFunc:: ItemId : {itemId} / Category : {category}");
        var itemTable = ServerManager.Instance.ItemTable;
        string imageId = itemTable[itemId].ImgId;

        var path = GameManager.Instance.GetItemImagePath(category, imageId);
        switch (category)
        {
            case (int)Category.Hair:
                Hair.sprite = Resources.Load<Sprite>(path);
                Hair.gameObject.SetActive(true);
                break;
            case (int)Category.Cloth:
                Cloth.sprite = Resources.Load<Sprite>(path);
                Cloth.gameObject.SetActive(true);
                break;
            case (int)Category.Ears:
                Ears.sprite = Resources.Load<Sprite>(path);
                Ears.gameObject.SetActive(true);
                break;
            case (int)Category.Background:
                Background.sprite = Resources.Load<Sprite>(path);
                Background.gameObject.SetActive(true);
                break;
        }

        var user = GameManager.Instance.User;
        var items = user.Items;
        items[itemId] = true;
    }
    public void CharacterUnEquipItemFunc(int itemId, int category)
    {
        Debug.Log($"CharacterUnEquipItemFunc:: Category : {category}");
        switch (category)
        {
            case (int)Category.Hair:
                {
                    //var equipItem = Hair.GetComponent<EquipItem>();
                    //itemId = equipItem.ItemId;
                    Hair.gameObject.SetActive(false);
                }
                break;
            case (int)Category.Cloth:
                {
                    //var equipItem = Cloth.GetComponent<EquipItem>();
                    Cloth.gameObject.SetActive(false);
                }
                break;
            case (int)Category.Ears:
                {
                    //var equipItem = Ears.GetComponent<EquipItem>();
                    //itemId = equipItem.ItemId;
                    Ears.gameObject.SetActive(false);
                }
                break;
            case (int)Category.Background:
                {
                    //var equipItem = Background.GetComponent<EquipItem>();
                    //itemId = equipItem.ItemId;
                    Background.gameObject.SetActive(false);
                }
                break;
        }
        

        var user = GameManager.Instance.User;
        var items = user.Items;
        items[itemId] = false;
    }

}

