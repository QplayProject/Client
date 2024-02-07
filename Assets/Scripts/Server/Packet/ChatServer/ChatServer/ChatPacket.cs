using System.Collections.Generic;

namespace ChatBase
{
    public class Packet
    {
        public int Opcode { get; set; }
        public string Message { get; set; }
    }
    public class Chat
    {
        public int ChatType { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }

    public class AddUser
    {
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public string UserName { get; set; }
    }

    public class AddChatRoom
    {
        public int State { get; set; }
        public string UserName { get; set; }
        public int RoomNumber { get; set; }
        public int CurrentMember { get; set; }
        public string RoomName { get; set; }
        public string OwnerName { get; set; }
        public List<string> RoomUsersInfo { get; set; }
    }

    public class RefreshChatRoom
    {
        public string UserName { get; set; }
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public int CurrentMember { get; set; }
        public List<string> RoomUsersInfo { get; set; }
    }

    public class JoinRoomMember
    {
        public int CurrentMember { get; set; }
        public int SlotNumber { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public List<int> EquipItems { get; set; }
    }

    public class ExitRoomMember
    {
        public int SlotNumber { get; set; }
        public int CurrentMember { get; set; }
    }

    public class ChangeUserState
    {
        public int State { get; set; }
        public string UserName { get; set; }
    }
}