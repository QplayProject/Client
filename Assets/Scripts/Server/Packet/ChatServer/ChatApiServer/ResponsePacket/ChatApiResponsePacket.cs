using GameInfo;
using System.Collections.Generic;

namespace ChatApiResponse
{
    public class Packet
    {
        public int MessageCode { get; set; }
        public string Message { get; set; }
    }
    public class JoinGame : Packet
    {
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public int Money { get; set; }
        public Dictionary<int, bool> Items { get; set; }
        public List<CreatedRoomInfo> CreatedRoomsInfo { get; set; }
        public List<LobbyUserInfo> LobbyUsersInfo { get; set; }
    }
    public class Room : Packet
    { 
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public int CurrentMember { get; set; }
        public string RoomName { get; set; }
        public string OwnerName { get; set; }
        public Dictionary<int, JoinRoomUserInfo> RoomUsersInfo { get; set; }
    }

    public class Lobby : Packet
    {
        public int State { get; set; }
        public List<CreatedRoomInfo> CreatedRoomsInfo { get; set; }
        public List<LobbyUserInfo> LobbyUsersInfo { get; set; }
    }

    public class ChangeState : Packet
    {
        public int State { get; set; }
    }

    public class BuyItem : Packet
    {
        public int ItemId { get; set; }
        public int Money { get; set; }
    }

    public class EquipItems : Packet
    {
        public Dictionary<int, bool> Items { get; set; }
    }
}