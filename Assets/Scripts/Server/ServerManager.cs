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
    public string LoginServerURL;// = "http://13.125.254.231:8000/api";
    public string ServerIp;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public async Task SendGameMessage(Game.Packet packet)
    {
        var message = JsonConvert.SerializeObject(packet);
        var opcodeString = GetOpcodeString(packet.Opcode);
        //Debug.Log($"SendGameMessage [{opcodeString}]");
        await SendGameMessageAsync(message);
    }


    public async Task SendGameMessageAsync(string message)
    {
        try
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(message);
            //dataBytes = MessagePackSerializer.Serialize(message);
            // 데이터의 길이를 구하고 전송
            int sendDataLength = dataBytes.Length;

            byte[] byteLength = BitConverter.GetBytes(sendDataLength);
            await Task.Run(() =>
            {
                NetworkStream.Write(byteLength, 0, byteLength.Length);

                // 실제 데이터를 전송
                NetworkStream.Write(dataBytes, 0, dataBytes.Length);
            });

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

    public async Task SendChatMessage(Chat.Packet packet, int opcode = (int)Opcode.Chat)
    {
        packet.Opcode = opcode;
        var message = JsonConvert.SerializeObject(packet);
        await SendChatMessageAsync(message);
    }
    public async Task SendChatMessageAsync(string message)
    {
        try
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(message);
            //dataBytes = MessagePackSerializer.Serialize(message);
            // 데이터의 길이를 구하고 전송
            int sendDataLength = dataBytes.Length;

            byte[] byteLength = BitConverter.GetBytes(sendDataLength);
            await Task.Run(() =>
            {
                ChatNetworkStream.Write(byteLength, 0, byteLength.Length);

                // 실제 데이터를 전송
                ChatNetworkStream.Write(dataBytes, 0, dataBytes.Length);
                //Debug.Log($"Sent message: {message}");
            });

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
    public Action ChatServer;

    public Action<string> MessageBox;

    public void OpenMessageBox(string message)
    {
        MessageBox?.Invoke(message);
    }

    public string GetOpcodeString(int opcode)
    {
        switch (opcode)
        {
            case (int)Opcode.Ping:
                return "Ping";
            case (int)Opcode.Chat:
                return "Chat";
            case (int)Opcode.JoinGame:
                return "JoinGame";
            case (int)Opcode.AddUserLobbyMember:
                return "AddUserLobbyMember";
            case (int)Opcode.AddChatRoomLobbyMember: //-- 본인을 제외한 로비에 위치한 유저들 가져옴
                return "AddChatRoomLobbyMember";
            case (int)Opcode.RoomLobbyMember:
                return "RoomLobbyMember";
            case (int)Opcode.LobbyMember:
                return "LobbyMember";
            case (int)Opcode.JoinRoomMember:
                return "JoinRoomMember";
            case (int)Opcode.ExitRoomMember:
                return "ExitRoomMember";
            case (int)Opcode.Logout:
                return "Logout";
            default:
                return $"NotFound! [{opcode}]";
        }

    }
}
