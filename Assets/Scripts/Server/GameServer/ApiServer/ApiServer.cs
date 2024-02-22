using System;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using Util;
using UnityEngine.SceneManagement;

public class ApiServer
{

    public IEnumerator ApiRequestJoinGame(ApiRequest.Packet request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/joingame";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {
                
                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;
                
                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.JoinGame>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    JoinGame(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestCreateRoom(ApiRequest.CreateRoom request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/createroom";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.CreateRoom>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    CreateRoom(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestJoinRoom(ApiRequest.JoinRoom request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/joinroom";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.JoinRoom>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    JoinRoom(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestExitRoom(ApiRequest.Packet request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/exitroom";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.ExitRoom>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    ExitRoom(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestSceneChange(ApiRequest.SceneChange request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/scenechange";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.SceneChange>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    SceneChange(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestBuyItem(ApiRequest.BuyItem request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/buyitem";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.BuyItem>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    BuyItem(response);
            }

            server.Dispose();
        }
    }

    public IEnumerator ApiRequestEquipItems(ApiRequest.EquipItems request)
    {
        var json = JsonConvert.SerializeObject(request);
        var serverManager = ServerManager.Instance;
        var url = $"{serverManager.ApiUrl}/equipitems";
        using (UnityWebRequest server = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            server.uploadHandler.Dispose();
            server.uploadHandler = new UploadHandlerRaw(jsonToSend);
            server.downloadHandler = new DownloadHandlerBuffer();
            server.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return server.SendWebRequest();
            if (server.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError($"Connect URL : {serverManager.ApiUrl}\nHTTP Request failed: {server.error}");
            }
            else
            {
                var data = server.downloadHandler.text;

                Debug.Log($"ApiResponse:: Data::{data}");
                var response = JsonConvert.DeserializeObject<ApiResponse.EquipItems>(data);
                if (response.MessageCode != (int)MessageCode.Success)
                    serverManager.OpenMessageBox(response.Message);
                else
                    EquipItems(response);
            }

            server.Dispose();
        }
    }

    
  







    private void JoinGame(ApiResponse.JoinGame response)
    {
        var gameManager = GameManager.Instance;
        
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
    private void CreateRoom(ApiResponse.CreateRoom response)
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
    private void JoinRoom(ApiResponse.JoinRoom response)
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
    private void ExitRoom(ApiResponse.ExitRoom response)
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
    private void SceneChange(ApiResponse.SceneChange response)
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

    private void BuyItem(ApiResponse.BuyItem response)
    {
        var gameManager = GameManager.Instance;
        var itemId = response.ItemId;
        gameManager.User.Items[itemId] = false;
        gameManager.User.Money = response.Money;
        gameManager.BuyItem?.Invoke(response);
    }

    private void EquipItems(ApiResponse.EquipItems response)
    {
        var gameManager = GameManager.Instance;
        gameManager.User.Items = response.Items;

        gameManager.ExitRoom?.Invoke();
    }

}

