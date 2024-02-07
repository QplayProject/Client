using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Util;
using System.Threading.Tasks;

public class Lobby : MonoBehaviour
{
	public Image Background;
	public Image Model;
	public Image Cloth;
	public Image Hair;
	public Image Ears;
	public Text Name;
	public Text Money;
    // Use this for initialization
    void Start()
    {
		LoadUserInfo();
    }
    void LoadUserInfo()
	{
		var gameManager = GameManager.Instance;
		var serverManager = ServerManager.Instance;



		var user = gameManager.User;
		var items = user.Items;
		var itemTable = ServerManager.Instance.ItemTable;

		string modelPath = gameManager.GetModelImagePath(user.Gender, user.Model);
		Model.sprite = Resources.Load<Sprite>(modelPath);

		Cloth.gameObject.SetActive(false);
        Hair.gameObject.SetActive(false);
        Ears.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);

		if (items == null)
		{
			Debug.Log("Items Null");
			//return;
		}
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

		Name.text = gameManager.User.UserName;
		Money.text = gameManager.User.Money.ToString();
    }

	// Update is called once per frame
	void Update()
	{
			
	}
}

