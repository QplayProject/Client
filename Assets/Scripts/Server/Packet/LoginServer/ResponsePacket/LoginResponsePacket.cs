using System.Collections.Generic;

namespace LoginResponse
{
    public class Packet
    {
        public int MessageCode { get; set; }
        public string Message { get; set; }
    }

    public class LoadTable : Packet
    {
        public Dictionary<int, Table.ItemInfo> ItemTable { get; set; }
        public Dictionary<int, Table.ShopItemInfo> ShopTable { get; set; }
    }
}