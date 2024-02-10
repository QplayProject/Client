using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameInfo;
using Newtonsoft.Json;
using UnityEngine;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private int count = 100;

    private void Start()
    {
        for (int i= 0; i < count; i++)
        {
            var createdRoomInfo = new CreatedRoomInfo();
            //createdRoomInfo.JoinRoomUsersInfo = new List<string>();
            CreatedRoomsInfo[i] = createdRoomInfo;

            //var lobbyUserInfo = new LobbyUserInfo();
            //LobbyUsersInfo.Add(lobbyUserInfo);
        }
    }
    public UserInfo User = new UserInfo();
    public JoinRoomInfo JoinRoom = new JoinRoomInfo();


    public Dictionary<int, CreatedRoomInfo> CreatedRoomsInfo = new Dictionary<int, CreatedRoomInfo>();
    public List<LobbyUserInfo> LobbyUsersInfo = new List<LobbyUserInfo>();

    public bool VersionCheck = false;

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

    public Queue<ChatBase.Packet> ReadMessages = new Queue<ChatBase.Packet>();
    public Action<ChatBase.Chat> Chat;
    public Action<ChatBase.AddUserLobbyMember> AddUserLobbyMember;
    public Action<ChatBase.AddChatRoomLobbyMember> AddChatRoomLobbyMember;
    public Action<ChatBase.RoomLobbyMember> RoomLobbyMember;
    public Action<ChatBase.LobbyMember> LobbyMember;
    public Action<ChatBase.JoinRoomMember> JoinRoomMember;
    public Action<ChatBase.ExitRoomMember> ExitRoomMember;
    public int SelectRoomNumber = -1;
    public Action ExitRoom;

    public Action<ChatApiResponse.BuyItem> BuyItem;
    private void Update()
    {
        if (ReadMessages.Count > 0)
        {
            var message = ReadMessages.Dequeue();
            int opcode = message.Opcode;
            
            {
                switch (opcode)
                {
                    case (int)Opcode.Chat: //-- 채팅 (서버<->클라이언트)
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.Chat>(message.Message);
                            Chat?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.AddUserLobbyMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.AddUserLobbyMember>(message.Message);
                            AddUserLobbyMember?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.AddChatRoomLobbyMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.AddChatRoomLobbyMember>(message.Message);
                            AddChatRoomLobbyMember?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.RoomLobbyMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.RoomLobbyMember>(message.Message);
                            RoomLobbyMember?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.LobbyMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.LobbyMember>(message.Message);
                            LobbyMember?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.JoinRoomMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.JoinRoomMember>(message.Message);
                            JoinRoomMember?.Invoke(packet);
                        }
                        break;
                    case (int)Opcode.ExitRoomMember:
                        {
                            var packet = JsonConvert.DeserializeObject<ChatBase.ExitRoomMember>(message.Message);
                            ExitRoomMember?.Invoke(packet);
                        }
                        break;
                }
            }
        }
    }
}