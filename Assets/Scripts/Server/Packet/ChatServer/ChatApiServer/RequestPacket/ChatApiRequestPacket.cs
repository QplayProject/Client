using System.Collections.Generic;

namespace ChatApiRequest
{
    public class Packet
    {
        public string Name { get; set; }
    }

    //-- Response -> ResponseRoom
    public class CreateRoom : Packet
    {
        public string RoomName { get; set; }
    }

    public class Room : Packet
    {
        public int RoomNumber { get; set; }
    }

    public class ChangeState : Packet
    {
        public int State { get; set; }
    }

    public class BuyItem : Packet
    {
        public int ItemId { get; set; }
    }

    public class ChangeCharacter : Packet
    {
        public Dictionary<int, bool> Items { get; set; }
    }
}