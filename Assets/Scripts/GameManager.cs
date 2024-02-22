using System;
using System.Collections.Generic;
using GameInfo;
using Newtonsoft.Json;
using UnityEngine;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Width;
    public int Height;
    private int count = 100;

    public User User = new User();
    public Room UserRoom = new Room();
    public Dictionary<int, Room> Rooms = new Dictionary<int, Room>();
    public List<LoginUser> LoginUsers = new List<LoginUser>();
    public List<Character> Characters = new List<Character>();
    public bool VersionCheck = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var room = new Room();
            room.RoomNumber = i;
            room.CurrentMember = 0;
            room.RoomName = "";
            room.OwnerName = "";
            Rooms[i] = room;
        }
        for (int i = 0; i < 6; i++)
        {
            var character = new Character();
            character.SlotNumber = i;
            character.UserName = "";
            character.Gender = -1;
            character.Model = -1;
            character.Items = new List<int>();
            Characters.Add(character);
        }
        Screen.SetResolution(Width, Height, false);
    }


    public string GetItemImagePath(int category, string imageId, bool isInventory = false)
    {
        int gender = User.Gender;
        string genderPath = "";
        switch(gender)
        {
            case (int)Util.Gender.Male:
                genderPath = "Male";
                break;
            case (int)Util.Gender.Female:
                genderPath = "Female";
                break;
        }
        string path = "";

        if (isInventory)
        {
            path += "Item/";
            switch (category)
            {
                case (int)Util.Category.Hair:
                    path += $"{genderPath}/Hair/{imageId}";
                    break;
                case (int)Util.Category.Cloth:
                    path += $"{genderPath}/Cloth/{imageId}";
                    break;
                case (int)Util.Category.Ears:
                    path += $"{genderPath}/Ears/{imageId}";
                    break;
                case (int)Util.Category.Eyes:
                    path += $"{genderPath}/Eyes/{imageId}";
                    break;
                case (int)Util.Category.EyesAcc:
                    path += $"{genderPath}/EyesAcc/{imageId}";
                    break;
                case (int)Util.Category.Face:
                    path += $"{genderPath}/Face/{imageId}";
                    break;
                case (int)Util.Category.Lip:
                    path += $"{genderPath}/Lip/{imageId}";
                    break;
                case (int)Util.Category.LipAcc:
                    path += $"{genderPath}/LipAcc/{imageId}";
                    break;
                case (int)Util.Category.Neck:
                    path += $"{genderPath}/Neck/{imageId}";
                    break;
                case (int)Util.Category.Background:
                    path += $"Background/{imageId}";
                    return path;
                case (int)Util.Category.Effect:
                    path += $"Effect/{imageId}";
                    return path;
                case (int)Util.Category.Pet:
                    path += $"Pet/{imageId}";
                    return path;
            }
            return path;
        }
        else
        {
            path += "Character/";
            switch (category)
            {
                case (int)Util.Category.Hair:
                    path += $"Item/{genderPath}/Hair/{imageId}";
                    break;
                case (int)Util.Category.Cloth:
                    path += $"Item/{genderPath}/Cloth/{imageId}";
                    break;
                case (int)Util.Category.Ears:
                    path += $"Item/{genderPath}/Ears/{imageId}";
                    break;
                case (int)Util.Category.Eyes:
                    path += $"Item/{genderPath}/Eyes/{imageId}";
                    break;
                case (int)Util.Category.EyesAcc:
                    path += $"Item/{genderPath}/EyesAcc/{imageId}";
                    break;
                case (int)Util.Category.Face:
                    path += $"Item/{genderPath}/Face/{imageId}";
                    break;
                case (int)Util.Category.Lip:
                    path += $"Item/{genderPath}/Lip/{imageId}";
                    break;
                case (int)Util.Category.LipAcc:
                    path += $"Item/{genderPath}/LipAcc/{imageId}";
                    break;
                case (int)Util.Category.Neck:
                    path += $"Item/{genderPath}/Neck/{imageId}";
                    break;
                case (int)Util.Category.Background:
                    path += $"Item/Background/{imageId}";
                    return path;
                case (int)Util.Category.Effect:
                    path += $"Item/Effect/{imageId}";
                    return path;
                case (int)Util.Category.Pet:
                    path += $"Item/Pet/{imageId}";
                    return path;
            }

            path += $"/{User.Model}";
            return path;
        }
       

    }

    public string GetModelImagePath(int gender, int model)
    {
        string genderPath = "Female";
        if (gender == (int)Util.Gender.Male) genderPath = "Male";
        string path = $"Character/Model/{genderPath}/{model}";
        return path;
    }

    public Queue<Chat.Packet> ChatMessages = new Queue<Chat.Packet>();
    public Action<Chat.Packet> Chat;

    public Queue<Game.ServerPacket> Messages = new Queue<Game.ServerPacket>();
    public Action<Game.AddUserLobbyMember> AddUserLobbyMember;
    public Action<Game.AddChatRoomLobbyMember> AddChatRoomLobbyMember;
    public Action<Game.RoomLobbyMember> RoomLobbyMember;
    public Action<Game.LobbyMember> LobbyMember;
    public Action<Game.JoinRoomMember> JoinRoomMember;
    public Action<Game.ExitRoomMember> ExitRoomMember;

    public int SelectRoomNumber = -1;
    public Action ExitRoom;

    public Action<ApiResponse.BuyItem> BuyItem;
    private void Update()
    {
        ReadGameServerMessages();
        ReadChatServerMessages();
    }

    private void ReadGameServerMessages()
    {
        if (Messages.Count > 0)
        {
            var message = Messages.Dequeue();
            int opcode = message.Opcode;

            string opcodeString = ServerManager.Instance.GetOpcodeString(opcode);
            Debug.Log($"Received message: {opcodeString}\n{message.Message}");

            switch (opcode)
            {
                case (int)Opcode.AddUserLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.AddUserLobbyMember>(message.Message);
                        AddUserLobbyMember?.Invoke(packet);
                        Debug.Log("AddUserLobbyMember Invoke");

                    }
                    break;
                case (int)Opcode.AddChatRoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.AddChatRoomLobbyMember>(message.Message);
                        AddChatRoomLobbyMember?.Invoke(packet);
                        Debug.Log("AddChatRoomLobbyMember Invoke");
                    }
                    break;
                case (int)Opcode.RoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.RoomLobbyMember>(message.Message);
                        RoomLobbyMember?.Invoke(packet);
                        Debug.Log("RoomLobbyMember Invoke");
                    }
                    break;
                case (int)Opcode.LobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.LobbyMember>(message.Message);
                        LobbyMember?.Invoke(packet);
                        Debug.Log("LobbyMember Invoke");
                    }
                    break;
                case (int)Opcode.JoinRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.JoinRoomMember>(message.Message);
                        JoinRoomMember?.Invoke(packet);
                        Debug.Log("JoinRoomMember Invoke");
                    }
                    break;
                case (int)Opcode.ExitRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.ExitRoomMember>(message.Message);
                        ExitRoomMember?.Invoke(packet);
                        Debug.Log("ExitRoomMember Invoke");
                    }
                    break;
            }


        }
    }

    private void ReadChatServerMessages()
    {
        if (ChatMessages.Count > 0)
        {
            var packet = ChatMessages.Dequeue();
            Chat?.Invoke(packet);
        }
    }

    void OnApplicationQuit()
    {
        /*
        var userName = GameManager.Instance.User.UserName;

        var gamePacket = new Game.Packet();
        gamePacket.Opcode = (int)Opcode.Logout;
        gamePacket.UserName = userName;
        _= ServerManager.Instance.SendGameMessage(gamePacket);

        var chatPacket = new Chat.Packet();
        chatPacket.UserName = userName;
        chatPacket.State = (int)UserState.Logout;
        _= ServerManager.Instance.SendChatMessage(chatPacket);
        */
    }


}