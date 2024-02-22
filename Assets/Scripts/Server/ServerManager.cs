using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    public Dictionary<int, Table.ItemInfo> ItemTable = new Dictionary<int, Table.ItemInfo>();

    public TcpClient TcpClient { get; set; }
    public NetworkStream NetworkStream { get; set; }
    public TcpClient ChatTcpClient { get; set; }
    public NetworkStream ChatNetworkStream { get; set; }
    public string GameServerIp;//"127.0.0.1";
    public int GameServerPort;
    public string ChatServerIp;
    public int ChatServerPort;
    public string ApiUrl;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public async Task SendMessageAsync(string message)
    {
        if (ChatTcpClient != null && ChatTcpClient.Connected)
        {
            try
            {

                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(message);

                //dataBytes = MessagePackSerializer.Serialize(message);

                // 데이터의 길이를 구하고 전송
                int sendDataLength = dataBytes.Length;

                byte[] byteLength = BitConverter.GetBytes(sendDataLength);
                await NetworkStream.WriteAsync(byteLength, 0, byteLength.Length);

                // 실제 데이터를 전송
                await NetworkStream.WriteAsync(dataBytes, 0, dataBytes.Length);
                //Debug.Log($"Sent message: {message}");
            }
            catch (ObjectDisposedException)
            {
                Debug.Log($"TcpClient는 이미 종료되었습니다");

            }
            catch (Exception e)
            {
                Debug.LogError($"Error SendMessageAsync : {e.Message}");
            }
        }
    }
    public async Task SendChatMessageAsync(string message)
    {
        if (ChatTcpClient != null && ChatTcpClient.Connected)
        {
            try
            {

                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(message);

                //dataBytes = MessagePackSerializer.Serialize(message);

                // 데이터의 길이를 구하고 전송
                int sendDataLength = dataBytes.Length;

                byte[] byteLength = BitConverter.GetBytes(sendDataLength);
                await ChatNetworkStream.WriteAsync(byteLength, 0, byteLength.Length);

                // 실제 데이터를 전송
                await ChatNetworkStream.WriteAsync(dataBytes, 0, dataBytes.Length);
                //Debug.Log($"Sent message: {message}");
            }
            catch (ObjectDisposedException)
            {
                Debug.Log($"ChatTcpClient는 이미 종료되었습니다");

            }
            catch (Exception e)
            {
                Debug.LogError($"Error SendChatMessageAsync : {e.Message}");
            }
        }
    }
    public Action ChatServer;

    public Action<string> MessageBox;

    public void OpenMessageBox(string message)
    {
        MessageBox?.Invoke(message);
    }

    public async Task SendGameMessage(Game.Packet packet)
    {
        var message = JsonConvert.SerializeObject(packet);

        await SendMessageAsync(message);
    }

    public async Task SendChatMessage(Chat.Packet packet)
    { 

        var message = JsonConvert.SerializeObject(packet);

        await SendChatMessageAsync(message);
    }

}
