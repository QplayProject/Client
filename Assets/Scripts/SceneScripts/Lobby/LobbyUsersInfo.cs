using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using Util;
using GameInfo;

public class LobbyUsersInfo: MonoBehaviour
{
    public GameObject userInfoPrefab;
    public List<GameObject> usersInfo;
    public Transform content;

    private int count = 100;

    void Start()
    {
        usersInfo = new List<GameObject>();
        CreateUsersInfo();
    }

    void CreateUsersInfo()
    {
        var gameManager = GameManager.Instance;
        var lobbyUsersInfo = gameManager.LobbyUsersInfo;

        for (int i = 0; i < count; i++)
        {
            GameObject userInfo = Instantiate(userInfoPrefab, content);
            usersInfo.Add(userInfo);
            usersInfo[i].SetActive(false);

        }
        for (int i = 0; i < lobbyUsersInfo.Count; i++)
        {
            var lobbyUserInfo = lobbyUsersInfo[i];

            var userInfo = usersInfo[i];
            userInfo.SetActive(true);

            var roomNumber = lobbyUserInfo.RoomNumber;
            var roomNumberText = userInfo.transform.Find("Info").gameObject.transform.Find("RoomNumber").GetComponent<Text>();

            var state = lobbyUserInfo.State;
            var userName = lobbyUserInfo.UserName;
            switch (state)
            {
                case (int)UserState.Lobby:
                    roomNumberText.text = "로비";
                    break;
                case (int)UserState.Room:
                    roomNumberText.text = $"{roomNumber}번방";
                    break;
                case (int)UserState.BeautyRoom:
                    roomNumberText.text = "분장실";
                    break;
                case (int)UserState.Shop:
                    roomNumberText.text = "상점";
                    break;
            }
            var userNameText = userInfo.transform.Find("Info").gameObject.transform.Find("Name").GetComponent<Text>();
            userNameText.text = userName;
        }
    }

    
    private void Update()
    {
        var gameManager = GameManager.Instance;
        var isAddUserInfo = gameManager.IsAddUserInfo;
        if (isAddUserInfo)
        {
            AddUserInfo();
            isAddUserInfo = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int count = 0;
            for(int i=0; i<usersInfo.Count; i++)
            {
                if (usersInfo[i].activeSelf) count++;
            }
            Debug.Log($"UsersInfo Count:{usersInfo.Count} / alive :{count}");
        }
    }

    private void AddUserInfo()
    {
        var gameManager = GameManager.Instance;
        var lobbyUsersInfo = gameManager.LobbyUsersInfo;

        for (int i = 0; i < count; i++)
        {
            if (usersInfo[i].activeSelf)
            {
                usersInfo[i].SetActive(false);
            }
        }

        for (int i = 0; i < lobbyUsersInfo.Count; i++)
        {
            var lobbyUserInfo = lobbyUsersInfo[i];

            var userInfo = usersInfo[i];
            userInfo.SetActive(true);

            var roomNumber = lobbyUserInfo.RoomNumber;
            var roomNumberText = userInfo.transform.Find("Info").gameObject.transform.Find("RoomNumber").GetComponent<Text>();

            var state = lobbyUserInfo.State;
            var userName = lobbyUserInfo.UserName;
            switch (state)
            {
                case (int)UserState.Lobby:
                    roomNumberText.text = "로비";
                    break;
                case (int)UserState.Room:
                    roomNumberText.text = $"{roomNumber}번방";
                    break;
                case (int)UserState.BeautyRoom:
                    roomNumberText.text = "분장실";
                    break;
                case (int)UserState.Shop:
                    roomNumberText.text = "상점";
                    break;
            }
            var userNameText = userInfo.transform.Find("Info").gameObject.transform.Find("Name").GetComponent<Text>();
            userNameText.text = userName;
        }

    }
}

