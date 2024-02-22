using System.Collections.Generic;

namespace Table
{
    public class ItemInfo
    {
        public int Id;
        public string Name;
        public int Category;
        public int Gender;
        public string ImgId;
        public int Price;
    }
}
namespace GameInfo
{
    #region 플레이어
    public class User
    {
        //-- enum : UserState
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public int SlotNumber { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public int Money { get; set; }
        public Dictionary<int, bool> Items { get; set; }
    }
    #endregion
    #region 로비
    public class LoginUser
    {
        //-- enum : UserState
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public string UserName { get; set; }
    }

    public class Room
    {
        public int RoomNumber { get; set; }
        public int CurrentMember { get; set; }
        public string RoomName { get; set; }
        public string OwnerName { get; set; }
    }
    #endregion
    public class Character
    {
        public int SlotNumber { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public List<int> Items { get; set; }
    }


}
namespace Util
{
    enum MessageCode
    {
        Success = 200,
        Fail = 204,
        BadRequest = 400,
        NotFound = 404
    }
    enum Opcode
    {
        JoinGame, //-- 게임 접속 (서버<->클라이언트)
        AddUserLobbyMember,
        AddChatRoomLobbyMember,
        RoomLobbyMember,
        LobbyMember,
        JoinRoomMember,
        ExitRoomMember,
        Logout
    }
   
    enum UserState
    {
        Lobby,
        Room,
        Shop,
        BeautyRoom,
        Logout,
    }

    enum DB
    {
        UserDB,
        TableDB
    }

    enum Gender
    {
        Female,
        Male
    }

    enum Category
    {
        Hair,
        Cloth,
        Ears,
        Eyes,
        EyesAcc,
        Face,
        Lip,
        LipAcc,
        Neck,
        Background,
        Effect,
        Pet,
    }
}
