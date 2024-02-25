using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class Login : MonoBehaviour
{
    public Text username;
    public InputField password;
    private bool isRunning = false;
    public void LoginEvent()
    {
        if (isRunning)
        {
            ServerManager.Instance.OpenMessageBox("접속 시도중입니다.");
            return;
        }
        if (username.text != "" && password.text != "")
        {
            isRunning = true;
            StartCoroutine(JoinGame());
        }
        
    }

    private IEnumerator JoinGame()
    {
        if (GameManager.Instance.VersionCheck)
        {
            LoginRequest.Login login = new LoginRequest.Login();
            login.Id = username.text;
            login.Password = password.text;
            LoginServer loginServer = new LoginServer();
            yield return StartCoroutine(loginServer.Login(login));
            if (GameManager.Instance.IsConnect)
            {
                var gameManager = GameManager.Instance;
                var joinGame = new ApiRequest.Packet();
                joinGame.UserName = gameManager.User.UserName;
                var webServer = new ApiServer();
                yield return StartCoroutine(webServer.ApiRequestJoinGame(joinGame));
            }
        }
        else
        {
            ServerManager.Instance.OpenMessageBox("클라이언트 버전을 확인해주세요!!");
        }
        isRunning = false;
    }
}

