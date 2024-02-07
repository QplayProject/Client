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

public class LoginServer
{
    public const string LoginServerURL = "http://localhost:5000/api/";

    public IEnumerator LoadTable(LoginRequest.LoadTable request)
    {
        var json = JsonConvert.SerializeObject(request);
        var api = "loadtable";
        using (UnityWebRequest server = UnityWebRequest.Post(LoginServerURL + $"{api}", json))
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
        ServerManager.Instance.ShopTable = response.ShopTable;
    }


    public IEnumerator Login(LoginRequest.Login request)
    {

        var json = JsonConvert.SerializeObject(request);
        
        var api = "login";
        using (UnityWebRequest server = UnityWebRequest.Post(LoginServerURL + $"{api}", json))
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
                var response = JsonConvert.DeserializeObject<LoginResponse.Packet>(data);
                if (response.MessageCode == (int)MessageCode.Success)
                {
                    GameManager.Instance.User.UserName = request.Id;

                }

            }
        }
    }
}
