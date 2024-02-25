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
            ServerManager.Instance.OpenMessageBox("���� �õ����Դϴ�.");
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
            ServerManager.Instance.OpenMessageBox("Ŭ���̾�Ʈ ������ Ȯ�����ּ���!!");
        }
        isRunning = false;
    }
}

