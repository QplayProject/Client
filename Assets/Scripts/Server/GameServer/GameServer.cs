using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using ApiResponse;
using GameInfo;
using Newtonsoft.Json;
using UnityEngine;
using Util;

public class GameServer : MonoBehaviour
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
            var gameServer = ConnectToGameServerAsync();
            var chatServer = ConnectToChatServerAsync();
            await Task.WhenAll(gameServer, chatServer);

        }
    }

    private async Task ConnectToGameServerAsync()
    {
        try
        {
            var serverManager = ServerManager.Instance;
            var serverIp = serverManager.GameServerIp;
            var serverPort = serverManager.GameServerPort;

            var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(serverIp, serverPort);

            var networkStream = tcpClient.GetStream();

            serverManager.TcpClient = tcpClient;
            serverManager.NetworkStream = networkStream;

            // 연결 성공
            Debug.Log("Connected to the Game Server!");

            var packet = new Game.Packet();
            packet.Opcode = (int)Opcode.JoinGame;
            packet.UserName = GameManager.Instance.User.UserName;
            var joinGame = ServerManager.Instance.SendGameMessage(packet);

            var receive = StartGameServerReceiving(tcpClient, networkStream);

            await Task.WhenAll(joinGame, receive);
        }
        catch (Exception e)
        {
            // 연결 실패
            Debug.LogError($"Error connecting to the server: {e.Message}");
        }
    }

    
    private async Task StartGameServerReceiving(TcpClient tcpClient, NetworkStream networkStream)
    {
        byte[] lengthBytes = new byte[4];

        while (tcpClient.Connected)
        {
            try
            {
                await networkStream.ReadAsync(lengthBytes, 0, 4);
                int dataLength = BitConverter.ToInt32(lengthBytes, 0);
                // 실제 데이터를 읽음
                byte[] buffer = new byte[dataLength];
                int bytesRead = await networkStream.ReadAsync(buffer, 0, dataLength);

                if (bytesRead > 0)
                {
                    string packet = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //int opcode = message.Opcode;
                    string opcodeText = "[NotFoundOpcode]";
                    /*
                    switch (opcode)
                    {
                        case (int)Opcode.AddUserLobbyMember:
                            opcodeText = "[AddUserLobbyMember]";
                            break;
                        case (int)Opcode.AddChatRoomLobbyMember:
                            opcodeText = "[AddChatRoomLobbyMember]";
                            break;
                        case (int)Opcode.RoomLobbyMember:
                            opcodeText = "[RoomLobbyMember]";
                            break;
                        case (int)Opcode.LobbyMember:
                            opcodeText = "[LobbyMember]";
                            break;
                        case (int)Opcode.JoinRoomMember:
                            opcodeText = "[JoinRoomMember]";
                            break;
                        case (int)Opcode.ExitRoomMember:
                            opcodeText = "[ExitRoomMember]";
                            break;
                    }
                    */
                    Debug.Log($"Received message: {opcodeText}\n{packet}");
                    var message = JsonConvert.DeserializeObject<Game.ServerPacket>(packet);
                    //-- 서버에서 들어온 메세지 처리
         
                    await ReadMessage(message);
                }
            }
            catch (IOException)
            {
                break;
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

    private async Task ConnectToChatServerAsync()
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
            Debug.Log("Connected to the Chat Server!");

            var user = GameManager.Instance.User;
            var packet = new Chat.Packet();
            packet.UserName = GameManager.Instance.User.UserName;
            packet.State = user.State;
            packet.RoomNumber = user.RoomNumber;
            packet.Message = "Connect To Chat Server!!";
            var joinChat = ServerManager.Instance.SendChatMessage(packet, (int)Opcode.Ping);

            var test = StartChatServerReceiving(tcpClient, networkStream);
            
            await Task.WhenAll(test);
        }
        catch (Exception e)
        {
            // 연결 실패
            Debug.LogError($"Error connecting to the server: {e.Message}");
        }
    }

    private async Task StartChatServerReceiving(TcpClient tcpClient, NetworkStream networkStream)
    {
        byte[] lengthBytes = new byte[4];

        while (tcpClient.Connected)
        {
            try
            {
                await networkStream.ReadAsync(lengthBytes, 0, 4);
                int dataLength = BitConverter.ToInt32(lengthBytes, 0);
                // 실제 데이터를 읽음
                byte[] buffer = new byte[dataLength];
                int bytesRead = await networkStream.ReadAsync(buffer, 0, dataLength);
                if (bytesRead > 0)
                {
                    string packet = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var message = JsonConvert.DeserializeObject<Chat.Packet>(packet);
                    Debug.Log($"Received Chat Message: {message.Message}");
                    if(message.Opcode == (int)Opcode.Ping)
                    {
                        await ServerManager.Instance.SendChatMessage(message, (int)Opcode.Ping);
                        continue;
                    }
                    await ReadChatMessage(message);
                }
            }
            catch (IOException)
            {
                Debug.Log("채팅 서버와의 연결이 끊겼습니다");
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

    private async Task ReadChatMessage(Chat.Packet message)
    {
        await Task.Run(() =>
        {
            GameManager.Instance.ChatMessages.Enqueue(message);
        });
    }

    private async Task ReadMessage(Game.ServerPacket message)
    {
        await Task.Run(() =>
        {
            int opcode = message.Opcode;
            switch (opcode)
            {
                case (int)Opcode.AddUserLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.AddUserLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var loginUser = new LoginUser();
                        loginUser.State = packet.State;
                        loginUser.RoomNumber = packet.RoomNumber;
                        loginUser.UserName = packet.UserName;

                        gameManager.LoginUsers.Add(loginUser);
                    }
                    Debug.Log($"AddUserLobbyMember:{message.Message}");

                    break;
                case (int)Opcode.AddChatRoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.AddChatRoomLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        //-- 생성된 방목록에 추가
                        var room = new Room();
                        room.CurrentMember = packet.CurrentMember;
                        room.RoomName = packet.RoomName;
                        room.RoomNumber = packet.RoomNumber;
                        //room.OwnerName = packet.OwnerName;

                        var createdRoom = gameManager.Rooms[packet.RoomNumber];
                        createdRoom = room;

                        //-- 접속 유저 목록에서 해당 유저 상태 변경
                        var userName = packet.UserName;

                        var loginUsers = GameManager.Instance.LoginUsers;
                        foreach (var loginUser in loginUsers)
                        {
                            if (loginUser.UserName != packet.UserName) continue;

                            loginUser.State = packet.State;
                            loginUser.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    Debug.Log($"AddChatRoomLobbyMember:{message.Message}");

                    break;
                case (int)Opcode.RoomLobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.RoomLobbyMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var room = gameManager.Rooms[packet.RoomNumber];
                        room.CurrentMember = packet.CurrentMember;

                        var loginUsers = GameManager.Instance.LoginUsers;
                        foreach (var loginUser in loginUsers)
                        {
                            if (loginUser.UserName != packet.UserName) continue;

                            loginUser.State = packet.State;
                            loginUser.RoomNumber = packet.RoomNumber;
                            break;
                        }
                    }
                    Debug.Log($"RoomLobbyMember:{message.Message}");

                    break;
                case (int)Opcode.LobbyMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.LobbyMember>(message.Message);

                        var loginUsers = GameManager.Instance.LoginUsers;
                        foreach (var loginUser in loginUsers)
                        {
                            if (loginUser.UserName != packet.UserName) continue;

                            loginUser.State = packet.State;
                            break;
                        }
                    }
                    Debug.Log($"LobbyMember:{message.Message}");

                    break;
                case (int)Opcode.JoinRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.JoinRoomMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var userRoom = gameManager.UserRoom;
                        userRoom.CurrentMember = packet.CurrentMember;

                        var characters = gameManager.Characters;
                        var character = characters[packet.SlotNumber];
                        character.SlotNumber = packet.SlotNumber;
                        character.UserName = packet.UserName;
                        character.Gender = packet.Gender;
                        character.Model = packet.Model;
                        character.Items = packet.EquipItems;
                    }
                    Debug.Log($"JoinRoomMember:{message.Message}");
                    break;
                case (int)Opcode.ExitRoomMember:
                    {
                        var packet = JsonConvert.DeserializeObject<Game.ExitRoomMember>(message.Message);
                        var gameManager = GameManager.Instance;

                        var userRoom = gameManager.UserRoom;
                        userRoom.CurrentMember = packet.CurrentMember;

                        var characters = gameManager.Characters;
                        var character = characters[packet.SlotNumber];
                        character.SlotNumber = packet.SlotNumber;
                        character.UserName = "";
                        character.Gender = -1;
                        character.Model = -1;
                        character.Items.Clear();
                    }
                    Debug.Log($"ExitRoomMember:{message.Message}");

                    break;
            }


            GameManager.Instance.Messages.Enqueue(message);
        });
    }

    private void OnDestroy()
    {
        /*
        var serverManager = ServerManager.Instance;
        var chatTcpClient = serverManager.ChatTcpClient;
        var gameTcpClient = serverManager.TcpClient;
        var userName = $"{GameManager.Instance.User.UserName}";
        var packet = new Game.Packet();
        packet.UserName = userName;
        packet.Opcode = (int)Opcode.Logout;
        _ = ServerManager.Instance.SendGameMessage(packet);
        if (gameTcpClient != null)
        {
            gameTcpClient.Close();
        }
        if (chatTcpClient != null)
        {
            chatTcpClient.Close();
        }
        */
        
    }

}
