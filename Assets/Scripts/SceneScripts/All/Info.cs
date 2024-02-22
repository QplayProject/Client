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
		var user = GameManager.Instance.User;
		var items = user.Items;
		var itemTable = ServerManager.Instance.ItemTable;

		//-- 성형 이미지 가져오기
		string modelPath = GameManager.Instance.GetModelImagePath(user.Gender, user.Model);
		Model.sprite = Resources.Load<Sprite>(modelPath);

        for(int i = 0; i < EquipItems.Length; i++)
        {
            EquipItems[i].gameObject.SetActive(false);
        }
        Background.gameObject.SetActive(false);

        foreach (var itemData in items)
		{
			if (!itemData.Value) continue;

			var path = "";

			int category = itemTable[itemData.Key].Category;
			string imageId = itemTable[itemData.Key].ImgId;

            path = GameManager.Instance.GetItemImagePath(category, imageId);
            EquipItems[category].sprite = Resources.Load<Sprite>(path);
            EquipItems[category].gameObject.SetActive(true);

		}

		Money.text = GameManager.Instance.User.Money.ToString();
    }

	public void CharacterEquipItemFunc(int itemId, int category)
	{
        Debug.Log($"CharacterEquipItemFunc:: ItemId : {itemId} / Category : {category}");
        var itemTable = ServerManager.Instance.ItemTable;
        string imageId = itemTable[itemId].ImgId;

        var path = GameManager.Instance.GetItemImagePath(category, imageId);
        path = GameManager.Instance.GetItemImagePath(category, imageId);
        EquipItems[category].sprite = Resources.Load<Sprite>(path);
        EquipItems[category].gameObject.SetActive(true);

        var user = GameManager.Instance.User;
        var items = user.Items;
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

}

