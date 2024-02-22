using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.TextCore.Text;
using System.Text;
using UnityEngine.SceneManagement;
using Util;
using System;
using ApiResponse;

public class LoginServer
{
    //public const string LoginServerURL = "http://13.125.254.231:5000/api/";
    public const string LoginServerURL = "http://localhost:8000/api";
    public IEnumerator LoadTable(LoginRequest.LoadTable request)
    {
        var json = JsonConvert.SerializeObject(request);
        var url = $"{LoginServerURL}/loadtable";
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
                Debug.LogError("HTTP Request failed: " + server.error);
            }
            else
            {
                var data = server.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<LoginResponse.LoadTable>(data);
                if (response.MessageCode == (int)MessageCode.Success)
                {
                    LoadTable(response);
                }

            }
        }
    }
    private void LoadTable(LoginResponse.LoadTable response)
    {
        GameManager.Instance.VersionCheck = true;
        ServerManager.Instance.ItemTable = response.ItemTable;
    }


    public IEnumerator Login(LoginRequest.Login request)
    {

        var json = JsonConvert.SerializeObject(request);

        var url = $"{LoginServerURL}/login";
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
                Debug.LogError("HTTP Request failed: " + server.error);
            }
            else
            {
                var data = server.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<LoginResponse.Login>(data);
                if (response.MessageCode == (int)MessageCode.Success)
                {
                    var gameManager = GameManager.Instance;
                    var user = gameManager.User;
                    user.UserName = response.UserName;
                    user.State = response.State;
                    user.RoomNumber = response.RoomNumber;
                    user.SlotNumber = response.SlotNumber;
                    user.UserName = response.UserName;
                    user.Gender = response.Gender;
                    user.Model = response.Model;
                    user.Money = response.Money;
                    user.Items = response.Items;

                }

            }
        }
    }

    
}
