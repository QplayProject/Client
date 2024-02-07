using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatApiResponse;
using ChatBase;
using Newtonsoft.Json;
using UnityEngine;
using Util;

public class ChatServer : MonoBehaviour
{
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        var tcpClient = ServerManager.Instance.ChatTcpClient;
        if (tcpClient == null)
        {
            await ConnectToServerAsync();
            await StartReceiving();
        }
    }

    private async Task ConnectToServerAsync()
    {
        try
        {
            var serverManager = ServerManager.Instance;
            var tcpClient = serverManager.ChatTcpClient;
            var serverIp = serverManager.ChatServerIp;
            var serverPort = serverManager.ChatServerPort;
            var networkStream = serverManager.ChatNetworkStream;

            await tcpClient.ConnectAsync(serverIp, serverPort);

            // 연결 성공
            Debug.Log("Connected to the server!");

            networkStream = tcpClient.GetStream();
        }
        catch (Exception e)
        {
            // 연결 실패
            Debug.LogError($"Error connecting to the server: {e.Message}");
        }
    }

    private async Task StartReceiving()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        var serverManager = ServerManager.Instance;
        var tcpClient = serverManager.ChatTcpClient;
        var networkStream = serverManager.ChatNetworkStream;

        while (tcpClient.Connected)
        {
            try
            {
                bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string packet = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log($"Received message: {packet}");
                    var message = JsonConvert.DeserializeObject<ChatBase.Packet>(packet);
                    //-- 서버에서 들어온 메세지 처리
                    await ReadMessage(message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error receiving message: {e.Message}");
                break;
            }
        }
    }

    private async Task ReadMessage(ChatBase.Packet message)
    {
        await Task.Run(() =>
        {
            int opcode = message.Opcode;
            switch (opcode)
            {
                case (int)Opcode.Message: //-- 서버용
                    break;
                case (int)Opcode.JoinGame: //-- 서버용
                    break;
                case (int)Opcode.Chat: //-- 채팅 (서버<->클라이언트)
                    break;
                case (int)Opcode.AddUser:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.AddUser>(message.Message);
                        var gameManager = GameManager.Instance;

                        var userInfo = new GameInfo.LobbyUserInfo();
                        userInfo.RoomNumber = packet.RoomNumber;
                        userInfo.State = packet.State;
                        userInfo.UserName = packet.UserName;

                        var lobbyUserInfo = gameManager.LobbyUsersInfo;
                        gameManager.LobbyUsersInfo.Add(userInfo);
                        gameManager.IsAddUserInfo = true;
                    }
                    break;
                case (int)Opcode.AddChatRoom:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.AddChatRoom>(message.Message);
                        var gameManager = GameManager.Instance;

                        var roomInfo = new GameInfo.CreatedRoomInfo();
                        roomInfo.CurrentMember = packet.CurrentMember;
                        roomInfo.RoomName = packet.RoomName;
                        roomInfo.RoomNumber = packet.RoomNumber;
                        roomInfo.OwnerName = packet.OwnerName;
                        roomInfo.RoomUsersInfo = packet.RoomUsersInfo;

                        var createdRoom = gameManager.CreatedRoomsInfo[packet.RoomNumber];
                        createdRoom = roomInfo;

                        var userName = packet.UserName;

                        var lobbyUsersInfo = GameManager.Instance.LobbyUsersInfo;
                        foreach (var userInfo in lobbyUsersInfo)
                        {
                            if (userInfo.UserName != packet.UserName) continue;

                            userInfo.State = (int)UserState.Room;
                            userInfo.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    break;
                case (int)Opcode.RefreshChatRoom:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.RefreshChatRoom>(message.Message);
                        var gameManager = GameManager.Instance;

                        var createdRoomsInfo = gameManager.CreatedRoomsInfo;
                        foreach (var createdRoom in createdRoomsInfo)
                        {
                            if (createdRoom.RoomNumber != packet.RoomNumber) continue;
                            createdRoom.CurrentMember = packet.CurrentMember;
                            createdRoom.RoomUsersInfo = packet.RoomUsersInfo;
                            break;
                        }

                        var lobbyUsersInfo = GameManager.Instance.LobbyUsersInfo;
                        foreach (var userInfo in lobbyUsersInfo)
                        {
                            if (userInfo.UserName != packet.UserName) continue;

                            userInfo.State = (int)UserState.Room;
                            userInfo.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    break;
                case (int)Opcode.JoinRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.JoinRoomMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var joinRoom = gameManager.JoinRoom;
                        joinRoom.CurrentMember = packet.CurrentMember;
                        var userInfo = joinRoom.RoomUsersInfo[packet.SlotNumber];
                        userInfo.UserName = packet.UserName;
                        userInfo.Gender = packet.Gender;
                        userInfo.Model = packet.Model;
                        userInfo.EquipItems = packet.EquipItems;
                    }
                    break;
                case (int)Opcode.ExitRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.ExitRoomMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var joinRoom = gameManager.JoinRoom;
                        joinRoom.CurrentMember = packet.CurrentMember;
                        var userInfo = joinRoom.RoomUsersInfo[packet.SlotNumber];
                        userInfo.UserName = null;
                        //userInfo.Gender = packet.Gender;
                        //userInfo.Model = packet.Model;
                        //userInfo.EquipItems = packet.EquipItems;
                    }
                    break;
                case (int)Opcode.ChangeUserState:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.ChangeUserState>(message.Message);
                        var gameManager = GameManager.Instance;

                        var lobbyUsersInfo = gameManager.LobbyUsersInfo;
                        foreach (var lobbyUserInfo in lobbyUsersInfo)
                        {
                            if (lobbyUserInfo.UserName != packet.UserName) continue;
                            lobbyUserInfo.State = packet.State;
                            break;
                        }
                    }
                    break;
            }
        });
    }


    private void OnDestroy()
    {
        var serverManager = ServerManager.Instance;
        var tcpClient = serverManager.ChatTcpClient;

        if (tcpClient != null)
        {
            tcpClient.Close();
        }
    }
}
