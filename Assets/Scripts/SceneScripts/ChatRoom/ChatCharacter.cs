using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using GameInfo;

public class ChatCharacter : MonoBehaviour
{
    public Image Model;
    public Image[] Images;
    public Text UserName;

    public void SetChatCharacter(Character info)
    {
        if (info.UserName == null)
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        var test = "";
        for (int i = 0; i < info.Items.Count; i++)
        {
            test += $"[{info.Items[i]}]";
        }
        Debug.Log($"[{info.UserName}] Gender[{info.Gender}] Model[{info.Model}]\n착용 아이템 {test}");

        for (int i= 0; i< Images.Length; i++)
        {
            if (Images[i] == null) continue;
            Images[i].gameObject.SetActive(false);
        }
        
        var gameManager = GameManager.Instance;
        var itemTable = ServerManager.Instance.ItemTable;

        var modelPath = gameManager.GetModelImagePath(info.Gender, info.Model);
        Debug.Log(modelPath);
        Model.sprite = Resources.Load<Sprite>(modelPath);
            

        var equipItems = info.Items;
        if (equipItems == null) return;
        if (equipItems.Count <= 0) return;
        for (int i = 0; i < equipItems.Count; i++)
        {
            var itemId = equipItems[i];
            var category = itemTable[itemId].Category;
            var imageId = itemTable[itemId].ImgId;
            var path = gameManager.GetItemImagePath(info.Model, info.Gender, category, imageId);
            Images[category].sprite = Resources.Load<Sprite>(path);
            Images[category].gameObject.SetActive(true);
        }

        UserName.text = info.UserName;
    }
}

