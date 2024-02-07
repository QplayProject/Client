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
    public const string ChatServerURL = "http://localhost:5001/api/";


    public IEnumerator ChatApiRequest(int requestHeader, object request)
    {
        var json = JsonConvert.SerializeObject(request);
        Debug.Log($"Test:: Request::{json}");

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
                var data = server.downloadHandler.text;
                Debug.Log($"Test:: Response::{data}");
                switch (requestHeader)
                {
                    case (int)RequestHeader.JoinGame:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.JoinGame>(data);
                            JoinGame(response);
                        }
                        break;
                    case (int)RequestHeader.CreateRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.Room>(data);
                            Room(response);
                        }
                        break;
                    case (int)RequestHeader.JoinRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.Room>(data);
                            Room(response);
                        }
                        break;
                    case (int)RequestHeader.ExitRoom:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.Lobby>(data);
                            Lobby(response);
                        }
                        break;
                    case (int)RequestHeader.ChangeState:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.ChangeState>(data);
                            ChangeState(response);
                        }
                        break;
                    case (int)RequestHeader.BuyItem:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.BuyItem>(data);
                            BuyItem(response);
                        }
                        break;
                    case (int)RequestHeader.EquipItems:
                        {
                            var response = JsonConvert.DeserializeObject<ChatApiResponse.EquipItems>(data);
                            EquipItems(response);
                        }
                        break;
                }
            }
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

            gameManager.CreatedRoomsInfo = response.CreatedRoomsInfo;
            gameManager.LobbyUsersInfo = response.LobbyUsersInfo;

            SceneManager.LoadScene("LobbyScene");
        }
    }

    //-- CreateRoom/JoinRoom
    private void Room(ChatApiResponse.Room response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        var room = gameManager.JoinRoom;
        user.State = response.State;
        user.RoomNumber = response.RoomNumber;

        room.CurrentMember = response.CurrentMember;
        room.RoomName = response.RoomName;
        room.OwnerName = response.OwnerName;
        room.RoomUsersInfo = response.RoomUsersInfo;
    }

    private void Lobby(ChatApiResponse.Lobby response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
        gameManager.CreatedRoomsInfo = response.CreatedRoomsInfo;
        gameManager.LobbyUsersInfo = response.LobbyUsersInfo;
    }

    private void ChangeState(ChatApiResponse.ChangeState response)
    {
        var gameManager = GameManager.Instance;
        var user = gameManager.User;
        user.State = response.State;
    }

    private void BuyItem(ChatApiResponse.BuyItem response)
    {
        var gameManager = GameManager.Instance;
        var itemId = response.ItemId;
        gameManager.User.Items[itemId] = false;
        gameManager.User.Money = response.Money;
    }

    private void EquipItems(ChatApiResponse.EquipItems response)
    {
        var gameManager = GameManager.Instance;
        gameManager.User.Items = response.Items;
    }

}

