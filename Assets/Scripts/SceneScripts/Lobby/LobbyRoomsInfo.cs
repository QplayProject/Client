using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyRoomsInfo: MonoBehaviour
{
    public GameObject roomInfoPrefab;
    public Dictionary<int, GameObject> roomsInfo;
    public Transform content;

    private int count = 100;
    void Start()
    {
        roomsInfo = new Dictionary<int, GameObject>();
        CreateRoomsInfo();
    }

    void CreateRoomsInfo()
    {
        var gameManager = GameManager.Instance;
        var createdRoomsInfo = gameManager.CreatedRoomsInfo;
        for (int i = 0; i < count; i++)
        {
            GameObject roomInfo = Instantiate(roomInfoPrefab, content);
            roomsInfo[i] = roomInfo;
            roomsInfo[i].SetActive(false);
        }
        for (int i = 0; i < createdRoomsInfo.Count; i++)
        {
            roomsInfo[i].SetActive(true);
            var roomInfo = createdRoomsInfo[i];
            var roomNumber = roomInfo.RoomNumber;
            var currentMember = roomInfo.CurrentMember;
            var roomName = roomInfo.RoomName;

            var infoPanel = roomsInfo[i].transform.Find("InfoPanel");

            var roomNumberText = infoPanel.gameObject.transform.Find("RoomNumber").GetComponent<Text>();
            roomNumberText.text = roomNumber.ToString();

            var currentMemberText = infoPanel.gameObject.transform.Find("Member").GetComponent<Text>();
            currentMemberText.text = currentMember.ToString();

            var namePanel = roomsInfo[i].transform.Find("NamePanel");

            var roomNameText = namePanel.gameObject.transform.Find("Name").GetComponent<Text>();
            roomNameText.text = roomName;

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

    }

    private void AddUserInfo()
    {
        var gameManager = GameManager.Instance;
        var createdRoomsInfo = gameManager.CreatedRoomsInfo;

        for (int i = 0; i < count; i++)
        {
            GameObject roomInfo = Instantiate(roomInfoPrefab, content);
            roomsInfo[i] = roomInfo;
            roomsInfo[i].SetActive(false);
        }

        for (int i = 0; i < createdRoomsInfo.Count; i++)
        {
            var createdRoomInfo = createdRoomsInfo[i];

            if (createdRoomInfo.RoomName == null)
            {
                if (!roomsInfo[i].activeSelf)
                {
                    roomsInfo[i].SetActive(false);
                }
                continue;
            }
            if (!roomsInfo[i].activeSelf)
            {
                roomsInfo[i].SetActive(true);
            }
            var roomNumber = createdRoomInfo.RoomNumber;
            var currentMember = createdRoomInfo.CurrentMember;
            var roomName = createdRoomInfo.RoomName;

            var infoPanel = roomsInfo[i].transform.Find("InfoPanel");

            var roomNumberText = infoPanel.gameObject.transform.Find("RoomNumber").GetComponent<Text>();
            roomNumberText.text = roomNumber.ToString();

            var currentMemberText = infoPanel.gameObject.transform.Find("Member").GetComponent<Text>();
            currentMemberText.text = currentMember.ToString();

            var namePanel = roomsInfo[i].transform.Find("NamePanel");

            var roomNameText = namePanel.gameObject.transform.Find("Name").GetComponent<Text>();
            roomNameText.text = roomName;

        }

    }
}

