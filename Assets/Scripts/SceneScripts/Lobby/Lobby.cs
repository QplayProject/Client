using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Lobby : MonoBehaviour
{
	public GameObject RoomPrefab;
	public Transform RoomsContent;
	public GameObject CreateRoomPanel;

	public GameObject LobbyUserPrefab;
	public Transform LobbyUsersContent;

	public Image[] ItemImages;
	public Image Model;
	public Text Name;
	public Text Money;

	private Dictionary<string, GameObject> LobbyUsers = new Dictionary<string, GameObject>();
	private Dictionary<int, GameObject> Rooms = new Dictionary<int, GameObject>();
    // Use this for initialization
    private void Start()
    {
        GameManager.Instance.AddUserLobbyMember = AddUserLobbyMember;
        GameManager.Instance.AddChatRoomLobbyMember = AddChatRoomLobbyMember;
        GameManager.Instance.RoomLobbyMember = RoomLobbyMember;
        GameManager.Instance.LobbyMember = LobbyMember;

		InitUserInfo();
		InitLobbyUsers();
		InitCreatedRooms();
    }

    private void InitUserInfo()
	{
        for (int i = 0; i < ItemImages.Length; i++)
        {
			if (ItemImages[i] == null) continue;
            ItemImages[i].gameObject.SetActive(false);
        }

        var gameManager = GameManager.Instance;

		var user = gameManager.User;
		var items = user.Items;
		var itemTable = ServerManager.Instance.ItemTable;

		string modelPath = gameManager.GetModelImagePath(user.Gender, user.Model);
		Model.sprite = Resources.Load<Sprite>(modelPath);

        foreach (var itemData in items)
		{
			bool isEquip = itemData.Value;
			if (!isEquip) continue;
			var item = itemTable[itemData.Key];

			var path = GameManager.Instance.GetItemImagePath(item.Category, item.ImgId);
			ItemImages[item.Category].sprite = Resources.Load<Sprite>(path);
			ItemImages[item.Category].gameObject.SetActive(true);
		}

		Name.text = gameManager.User.UserName;
		Money.text = gameManager.User.Money.ToString();
    }

	private void InitLobbyUsers()
	{
		var lobbyUsers = GameManager.Instance.LobbyUsersInfo;
		foreach (var user in lobbyUsers)
		{
			GameObject LobbyUser = Instantiate(LobbyUserPrefab, LobbyUsersContent);
			var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
			lobbyUserInfo.SetLobbyUser(user.UserName, user.State, user.RoomNumber);

			LobbyUsers[user.UserName] = LobbyUser;
		}
	}

	private void InitCreatedRooms()
	{
		var gameManager = GameManager.Instance;
		var roomsInfo = gameManager.CreatedRoomsInfo;
		foreach (var t in gameManager.CreatedRoomsInfo)
		{
			if (t.Value.RoomName == null) continue;
			Debug.Log($"{t.Value.RoomName} / COUNT: {roomsInfo.Count}");
		}
		
		for (int i= 0; i< roomsInfo.Count; i++)
		{
            GameObject room = Instantiate(RoomPrefab, RoomsContent);

            var createdRoom = room.GetComponent<CreatedRoom>();
			string roomName;
			if (roomsInfo[i].RoomName == null)
			{
				roomName = "";
				createdRoom.SetCreatedRoom(roomsInfo[i].RoomNumber, roomsInfo[i].CurrentMember, roomName);
                room.SetActive(false);
            }
			else
			{
				roomName = roomsInfo[i].RoomName;
                createdRoom.SetCreatedRoom(roomsInfo[i].RoomNumber, roomsInfo[i].CurrentMember, roomName);
	            room.SetActive(true);
            }
			Rooms[roomsInfo[i].RoomNumber] = room;

        }
        foreach (var roomInfo in roomsInfo)
		{
			
			
		}
	}
    public void OpenCreateRoomPanel()
    {
		CreateRoomPanel.SetActive(true);
    }

	private void AddUserLobbyMember(ChatBase.AddUserLobbyMember callback)
	{
        GameObject LobbyUser = Instantiate(LobbyUserPrefab, LobbyUsersContent);
        var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
        lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);

        LobbyUsers[callback.UserName] = LobbyUser;
    }

    private void AddChatRoomLobbyMember(ChatBase.AddChatRoomLobbyMember callback)
    {
		var roomNumber = callback.RoomNumber;
		var room = Rooms[roomNumber];
        var createdRoom = room.GetComponent<CreatedRoom>();

		createdRoom.SetCreatedRoom(roomNumber, callback.CurrentMember, callback.RoomName);
		room.SetActive(true);
    }

	private void RoomLobbyMember(ChatBase.RoomLobbyMember callback)
	{
		var user = LobbyUsers[callback.UserName];
		var lobbyUserInfo = user.GetComponent<LobbyUser>();
		lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);


		var roomNumber = callback.RoomNumber;
		var room = Rooms[roomNumber];
		var createdRoom = room.GetComponent<CreatedRoom>();
		createdRoom.Member.text = $"{callback.CurrentMember}";
    }

    private void LobbyMember(ChatBase.LobbyMember callback)
    {
        var user = LobbyUsers[callback.UserName];
        var lobbyUserInfo = user.GetComponent<LobbyUser>();
        lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State);
    }
}

