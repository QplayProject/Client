using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Dictionary<int, Table.ItemInfo> ItemTable = new Dictionary<int, Table.ItemInfo>();
    public Dictionary<int, Table.ShopItemInfo> ShopTable = new Dictionary<int, Table.ShopItemInfo>();


    public TcpClient ChatTcpClient { get; set; }
    public NetworkStream ChatNetworkStream { get; set; }
    public string ChatServerIp = "13.125.254.231";//"127.0.0.1";
    public int ChatServerPort = 8080;

    public async Task SendMessageAsync(string message)
    {
        if (ChatTcpClient != null && ChatTcpClient.Connected)
        {
            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                await ChatNetworkStream.WriteAsync(data, 0, data.Length);
                //Debug.Log($"Sent message: {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error sending message: {e.Message}");
            }
        }
    }

    public Action ChatServer;

    public Action<string> MessageBox;

    public void OpenMessageBox(string message)
    {
        MessageBox?.Invoke(message);
    }


    public async Task SendChatMessage(int opcode, object packet)
    { 
        var chatPacket = new ChatBase.Packet();
        chatPacket.Opcode = opcode;
        chatPacket.Message = JsonConvert.SerializeObject(packet);

        var message = JsonConvert.SerializeObject(chatPacket);

        //Debug.Log($"Test: {message}");
        await SendMessageAsync(message);
    }

}
