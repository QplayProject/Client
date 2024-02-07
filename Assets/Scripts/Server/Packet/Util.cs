using System.Collections.Concurrent;
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
    }
    public class ShopItemInfo
    {
        public int Id;
        public int Price;
    }
}
namespace GameInfo
{
    #region 플레이어
    public class UserInfo
    {
        //-- enum : UserState
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public int Money { get; set; }
        public Dictionary<int, bool> Items { get; set; }
    }
    #endregion
    #region 로비
    public class LobbyUserInfo
    {
        //-- enum : UserState
        public int State { get; set; }
        public int RoomNumber { get; set; }
        public string UserName { get; set; }
    }

    public class CreatedRoomInfo
    {
        public int RoomNumber { get; set; }
        public int CurrentMember { get; set; }
        public string RoomName { get; set; }
        public string OwnerName { get; set; }
        public List<string> RoomUsersInfo { get; set; }
    }
    #endregion

   
    public class JoinRoomInfo
    {
        public int CurrentMember { get; set; }
        public string RoomName { get; set; }
        public string OwnerName { get; set; }
        public Dictionary<int, JoinRoomUserInfo> RoomUsersInfo { get; set; }
    }

    public class JoinRoomUserInfo
    {
        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Model { get; set; }
        public List<int> EquipItems { get; set; }
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
        Message, //-- 기본 응답 (서버->클라이언트 Tcp메시지 호출 응답용)
        JoinGame, //-- 게임 접속 (서버<->클라이언트)
        Chat, //-- 채팅 (서버<->클라이언트)
        AddUser,
        AddChatRoom,
        RefreshChatRoom,
        JoinRoomMember,
        ExitRoomMember,
        ChangeUserState,
    }
    enum ChatType
    {
        Notice,
        All
    }

    enum RequestHeader
    {
        JoinGame,
        CreateRoom,
        JoinRoom,
        ExitRoom,
        ChangeState,
        BuyItem,
        EquipItems
    }

    enum UserState
    {
        Lobby,
        Room,
        Shop,
        BeautyRoom
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
