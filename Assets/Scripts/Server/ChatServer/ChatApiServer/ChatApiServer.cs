using System;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using Util;
using UnityEngine.SceneManagement;

public class ChatApiServer
{
    public const string ChatServerURL = "http://13.125.254.231:5001/api/";


    public IEnumerator ChatApiRequest(int requestHeader, object request)
    {
        var json = JsonConvert.SerializeObject(request);
        string headerString = $"알수없는 헤더 [{requestHeader}]";
        switch (requestHeader)
        {
            case (int)RequestHeader.JoinGame:
                headerString = "JoinGame";
                break;
            case (int)RequestHeader.CreateRoom:
                headerString = "CreateRoom";
                break;
            case (int)RequestHeader.JoinRoom:
                headerString = "JoinRoom";
                break;
            case (int)RequestHeader.ExitRoom:
                headerString = "ExitRoom";
                break;
            case (int)RequestHeader.SceneChange:
                headerString = "SceneChange";
                break;
            case (int)RequestHeader.BuyItem:
                headerString = "BuyItem";
                break;
            case (int)RequestHeader.EquipItems:
                headerString = "EquipItems";
                break;
        }
        Debug.Log($"ChatApiRequest:: Header:[{headerString}] Data::{json}");

        using (UnityWebRequest server = UnityWebRequest.Post(ChatServerURL, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
            server.SetRequestHeader("MessageType", requestHeader.ToString());

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("HTTP Request failed: " + server.error);
            }
            else
            {
                var serverManager = ServerManager.Instance;
                var data = server.downloadHandler.text;
                
                Debug.Log($"ChatApiResponse:: Header:[{headerString}] Data::{data}");
                switch (requestHeader)
                {
                    case (int)RequestHeader.JoinGame:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.JoinGame>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                            JoinGame(response);
                        }
                        break;
                    case (int)RequestHeader.CreateRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.Room>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                Room(response);
                        }
                        break;
                    case (int)RequestHeader.JoinRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.Room>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                Room(response);
                        }
                        break;
                    case (int)RequestHeader.ExitRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.ExitRoom>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                ExitRoom(response);
                        }
                        break;
                    case (int)RequestHeader.SceneChange:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.SceneChange>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                SceneChange(response);
                        }
                        break;
                    case (int)RequestHeader.BuyItem:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.BuyItem>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                BuyItem(response);
                        }
                        break;
                    case (int)RequestHeader.EquipItems:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.EquipItems>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                EquipItems(response);
                        }
                        break;
                    default:
                        serverManager.OpenMessageBox($"Error! Header:{headerString}\n{data}");
                        break;

                }
            }

            server.Dispose();
        }
        
    }

    private void JoinGame(ChatApiResponse.JoinGame response)
    {
        if (response.MessageCode == (int)MessageCode.Success)
        {
            var gameManager = GameManager.Instance;
            var user = gameManager.User;
            user.State = response.State;
            user.RoomNumber = response.RoomNumber;
            user.UserName = response.UserName;
            user.Gender = response.Gender;
            user.Model = response.Model;
            user.Money = response.Money;
            user.Items = response.Items;

            var roomsInfo = gameManager.CreatedRoomsInfo;
            for (int i = 0; i< response.CreatedRoomsInfo.Count; i++)
            {
                roomsInfo[i] = response.CreatedRoomsInfo[i];
            }
            //gameManager.CreatedRoomsInfo = response.CreatedRoomsInfo;
            foreach(var t in roomsInfo)
            {
                if (t.Value.RoomName == null) continue;
                Debug.Log($"Test:: RoomNaem:{t.Value.RoomName}");
            }
            gameManager.LobbyUsersInfo = response.LobbyUsersInfo;
            ServerManager.Instance.ChatServer?.Invoke();
            SceneManager.LoadScene("LobbyScene");
        }
    }

    //-- CreateRoom/JoinRoom
    private void Room(ChatApiResponse.Room response)
    {
        if (response.MessageCode == (int)MessageCode.Success)
        {
            var gameManager = GameManager.Instance;
            var user = gameManager.User;
            var room = gameManager.JoinRoom;
            user.State = response.State;
            user.RoomNumber = response.RoomNumber;

            room.CurrentMember = response.CurrentMember;
            room.RoomName = response.RoomName;
            room.OwnerName = response.OwnerName;
            room.JoinRoomUsersInfo = response.JoinRoomUsersInfo;
            SceneManager.LoadScene("ChatRoomScene");
        }
        

    }

    private void ExitRoom(ChatApiResponse.ExitRoom response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
        var roomsInfo = gameManager.CreatedRoomsInfo;
        for (int i = 0; i < response.CreatedRoomsInfo.Count; i++)
        {
            roomsInfo[i] = response.CreatedRoomsInfo[i];
        }
        gameManager.LobbyUsersInfo = response.LobbyUsersInfo;
        SceneManager.LoadScene("LobbyScene");

    }
    private void SceneChange(ChatApiResponse.SceneChange response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
        switch (user.State)
        {
            case (int)UserState.Shop:
                SceneManager.LoadScene("ShopScene");
                break;
            case (int)UserState.BeautyRoom:
                SceneManager.LoadScene("BeautyRoomScene");
                break;
        }

    }

    private void BuyItem(ChatApiResponse.BuyItem response)
    {
        var gameManager = GameManager.Instance;
        var itemId = response.ItemId;
        gameManager.User.Items[itemId] = false;
        gameManager.User.Money = response.Money;
        gameManager.BuyItem?.Invoke(response);
    }

    private void EquipItems(ChatApiResponse.EquipItems response)
    {
        var gameManager = GameManager.Instance;
        gameManager.User.Items = response.Items;

        gameManager.ExitRoom?.Invoke();
    }

}

