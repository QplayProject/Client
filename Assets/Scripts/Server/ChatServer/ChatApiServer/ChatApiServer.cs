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
        var serverManager = ServerManager.Instance;
        using (UnityWebRequest server = UnityWebRequest.Post(serverManager.ChatApiServerUrl, json))
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
                
                Debug.LogError($"Connect URL : {serverManager.ChatApiServerUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
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
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.CreateRoom>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                CreateRoom(response);
                        }
                        break;
                    case (int)RequestHeader.JoinRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.JoinRoom>(data);
                            if (response.MessageCode != (int)MessageCode.Success)
                                serverManager.OpenMessageBox(response.Message);
                            else
                                JoinRoom(response);
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
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
        user.RoomNumber = response.RoomNumber;
        user.SlotNumber = response.SlotNumber;
        user.UserName = response.UserName;
        user.Gender = response.Gender;
        user.Model = response.Model;
        user.Money = response.Money;
        user.Items = response.Items;

        var roomsInfo = gameManager.Rooms;
        foreach (var room in response.Rooms)
        {
            var roomNumber = room.Key;
            roomsInfo[roomNumber] = room.Value;
        }
        gameManager.LoginUsers = response.LoginUsers;

        ServerManager.Instance.ChatServer?.Invoke();
        SceneManager.LoadScene("LobbyScene");

    }

    //-- CreateRoom/JoinRoom
    private void CreateRoom(ChatApiResponse.CreateRoom response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
        user.RoomNumber = response.RoomNumber;
        user.SlotNumber = response.SlotNumber;

        var room = gameManager.UserRoom;
        room.CurrentMember = response.CurrentMember;
        room.RoomNumber = response.RoomNumber;
        room.RoomName = response.RoomName;
        room.OwnerName = response.OwnerName;

        var character = gameManager.Characters[response.SlotNumber];

        character.UserName = user.UserName;
        character.Gender = user.Gender;
        character.Model = user.Model;
        var items = user.Items;
        foreach(var isEquip in items)
        {
            if (!isEquip.Value) continue;
            var equipItem = isEquip.Key;
            character.Items.Add(equipItem);
        }
        SceneManager.LoadScene("ChatRoomScene");

    }
    private void JoinRoom(ChatApiResponse.JoinRoom response)
    {
        if (response.MessageCode == (int)MessageCode.Success)
        {
            var gameManager = GameManager.Instance;
            var user = gameManager.User;
            user.State = response.State;
            user.RoomNumber = response.RoomNumber;
            user.SlotNumber = response.SlotNumber;

            var room = gameManager.UserRoom;
            room.CurrentMember = response.CurrentMember;
            room.RoomNumber = response.RoomNumber;
            room.RoomName = response.RoomName;
            room.OwnerName = response.OwnerName;

            var characters = gameManager.Characters;
            for (int i = 0; i < response.Characters.Count; i++)
            {
                characters[i] = response.Characters[i];    
            }
            

            SceneManager.LoadScene("ChatRoomScene");
        }
    }
    private void ExitRoom(ChatApiResponse.ExitRoom response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;

        user.State = response.State;
        var roomsInfo = gameManager.Rooms;
        foreach (var room in response.Rooms)
        {
            var roomNumber = room.Key;
            roomsInfo[roomNumber] = room.Value;
        }
        gameManager.LoginUsers = response.LoginUsers;
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

