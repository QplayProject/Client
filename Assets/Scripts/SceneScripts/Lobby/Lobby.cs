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
		InitLoginUsers();
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

			var path = GameManager.Instance.GetItemImagePath(user.Model, user.Gender, item.Category, item.ImgId);
			ItemImages[item.Category].sprite = Resources.Load<Sprite>(path);
			ItemImages[item.Category].gameObject.SetActive(true);
		}

		Name.text = gameManager.User.UserName;
		Money.text = gameManager.User.Money.ToString();
    }

	private void InitLoginUsers()
	{
		var loginUsers = GameManager.Instance.LoginUsers;
		foreach (var user in loginUsers)
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
		var rooms = gameManager.Rooms;
		
		
		for (int i= 0; i< rooms.Count; i++)
		{
            GameObject room = Instantiate(RoomPrefab, RoomsContent);

            var createdRoom = room.GetComponent<CreatedRoom>();
			string roomName;
			if (rooms[i].CurrentMember <= 0)
			{
				roomName = "";
				createdRoom.SetCreatedRoom(rooms[i].RoomNumber, rooms[i].CurrentMember, roomName);
                room.SetActive(false);
            }
			else
			{
				roomName = rooms[i].RoomName;
                createdRoom.SetCreatedRoom(rooms[i].RoomNumber, rooms[i].CurrentMember, roomName);
	            room.SetActive(true);
            }
			Rooms[i] = room;

        }
	}
    public void OpenCreateRoomPanel()
    {
		CreateRoomPanel.SetActive(true);
    }

	private void AddUserLobbyMember(Game.AddUserLobbyMember callback)
	{
		var userName = callback.UserName;
		Debug.Log("AddUserLobbyMember");
		if (LobbyUsers.ContainsKey(userName))
        {
			var LobbyUser = LobbyUsers[userName];
			var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
			if (!lobbyUserInfo.gameObject.activeSelf) lobbyUserInfo.gameObject.SetActive(true);
			lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);
			LobbyUsers[callback.UserName] = LobbyUser;
		}
		else
        {
			var LobbyUser = Instantiate(LobbyUserPrefab, LobbyUsersContent);
			var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
			lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);

			LobbyUsers[callback.UserName] = LobbyUser;
		}
        
    }

    private void AddChatRoomLobbyMember(Game.AddChatRoomLobbyMember callback)
    {
		var roomNumber = callback.RoomNumber;
		var room = Rooms[roomNumber];
        var createdRoom = room.GetComponent<CreatedRoom>();
		Debug.Log($"TEST: number:{roomNumber}/member:{callback.CurrentMember}/roomName:{callback.RoomName}");
		createdRoom.SetCreatedRoom(roomNumber, callback.CurrentMember, callback.RoomName);
		room.SetActive(true);
		var userName = callback.UserName;

		if (LobbyUsers.ContainsKey(userName))
		{
			var LobbyUser = LobbyUsers[userName];
			var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
			lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);

			LobbyUsers[callback.UserName] = LobbyUser;
		}
		else
		{
			var LobbyUser = Instantiate(LobbyUserPrefab, LobbyUsersContent);
			var lobbyUserInfo = LobbyUser.GetComponent<LobbyUser>();
			lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);

			LobbyUsers[callback.UserName] = LobbyUser;
		}
	}

	//-- 방 나가기
	private void RoomLobbyMember(Game.RoomLobbyMember callback)
	{
		var user = LobbyUsers[callback.UserName];
		var lobbyUserInfo = user.GetComponent<LobbyUser>();
		lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State, callback.RoomNumber);


		var roomNumber = callback.RoomNumber;
		var room = Rooms[roomNumber];
		var createdRoom = room.GetComponent<CreatedRoom>();
		createdRoom.Member.text = $"{callback.CurrentMember}";
		Debug.Log($"Lobby->RoomLobbyMember:{callback.RoomNumber}[{callback.CurrentMember}]");
		if (callback.CurrentMember <= 0)
        {
			room.SetActive(false);
        }
    }

    private void LobbyMember(Game.LobbyMember callback)
    {
		var state = callback.State;
        var user = LobbyUsers[callback.UserName];
		if (state == (int)Opcode.Logout)
        {
			Debug.Log($"유저 접속종료 !! {callback.UserName}");
			LobbyUsers.Remove(callback.UserName);
			Destroy(user);
			return;
        }
        var lobbyUserInfo = user.GetComponent<LobbyUser>();
        lobbyUserInfo.SetLobbyUser(callback.UserName, callback.State);
    }
}

