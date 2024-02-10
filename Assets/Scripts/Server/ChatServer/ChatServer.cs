using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameInfo;
using Newtonsoft.Json;
using UnityEngine;
using Util;

public class ChatServer : MonoBehaviour
{
    private void Start()
    {
        ServerManager.Instance.ChatServer = ConnectChatServer;
    }
    public async void ConnectChatServer()
    {
        DontDestroyOnLoad(gameObject);
        var serverManager = ServerManager.Instance;
        var tcpClient = serverManager.ChatTcpClient;
        if (tcpClient == null)
        {
            await ConnectToServerAsync();
        }
    }

    
    private async Task ConnectToServerAsync()
    {
        try
        {
            var serverManager = ServerManager.Instance;
            var serverIp = serverManager.ChatServerIp;
            var serverPort = serverManager.ChatServerPort;

            var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(serverIp, serverPort);

            var networkStream = tcpClient.GetStream();

            serverManager.ChatTcpClient = tcpClient;
            serverManager.ChatNetworkStream = networkStream;

            // 연결 성공
            Debug.Log("Connected to the server!");

            var packet = new ChatRequest.JoinGame();
            packet.UserName = GameManager.Instance.User.UserName;
            var joinGame = ServerManager.Instance.SendChatMessage((int)Opcode.JoinGame, packet);

            var receive = StartReceiving(serverManager.ChatTcpClient, serverManager.ChatNetworkStream);

            await Task.WhenAll(joinGame, receive);
        }
        catch (Exception e)
        {
            // 연결 실패
            Debug.LogError($"Error connecting to the server: {e.Message}");
        }
    }

    private async Task StartReceiving(TcpClient tcpClient, NetworkStream networkStream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        var serverManager = ServerManager.Instance;


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
        Debug.Log("서버 연결 종료");
        tcpClient.Close();
        //serverManager.ChatNetworkStream = networkStream;

    }

    private async Task ReadMessage(ChatBase.Packet message)
    {
        await Task.Run(() =>
        {
            int opcode = message.Opcode;
            switch (opcode)
            {
                case (int)Opcode.AddUserLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.AddUserLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var userInfo = new GameInfo.LobbyUserInfo();
                        userInfo.RoomNumber = packet.RoomNumber;
                        userInfo.State = packet.State;
                        userInfo.UserName = packet.UserName;

                        var lobbyUserInfo = gameManager.LobbyUsersInfo;
                        gameManager.LobbyUsersInfo.Add(userInfo);
                    }
                    break;
                case (int)Opcode.AddChatRoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.AddChatRoomLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        //-- 생성된 방목록에 추가
                        var roomInfo = new GameInfo.CreatedRoomInfo();
                        roomInfo.CurrentMember = packet.CurrentMember;
                        roomInfo.RoomName = packet.RoomName;
                        roomInfo.RoomNumber = packet.RoomNumber;
                        roomInfo.OwnerName = packet.OwnerName;
                        //roomInfo.RoomUsersInfo = packet.JoinRoomUsersInfo;

                        var createdRoom = gameManager.CreatedRoomsInfo[packet.RoomNumber];
                        createdRoom = roomInfo;

                        //-- 접속 유저 목록에서 해당 유저 상태 변경
                        var userName = packet.UserName;

                        var lobbyUsersInfo = GameManager.Instance.LobbyUsersInfo;
                        foreach (var userInfo in lobbyUsersInfo)
                        {
                            if (userInfo.UserName != packet.UserName) continue;

                            userInfo.State = packet.State;
                            userInfo.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    break;
                case (int)Opcode.RoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.RoomLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var createdRoom = gameManager.CreatedRoomsInfo[packet.RoomNumber];
                        createdRoom.CurrentMember = packet.CurrentMember;

                        var lobbyUsersInfo = GameManager.Instance.LobbyUsersInfo;
                        foreach (var userInfo in lobbyUsersInfo)
                        {
                            if (userInfo.UserName != packet.UserName) continue;

                            userInfo.State = packet.State;
                            userInfo.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    break;
                case (int)Opcode.LobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<ChatBase.LobbyMember>(message.Message);

                        var lobbyUsersInfo = GameManager.Instance.LobbyUsersInfo;
                        foreach (var userInfo in lobbyUsersInfo)
                        {
                            if (userInfo.UserName != packet.UserName) continue;

                            userInfo.State = packet.State;
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

                        var userInfo = joinRoom.JoinRoomUsersInfo[packet.SlotNumber];
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

                        var userInfo = joinRoom.JoinRoomUsersInfo[packet.SlotNumber];
                        userInfo.UserName = null;
                    }
                    break;
            }


            GameManager.Instance.ReadMessages.Enqueue(message);
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
