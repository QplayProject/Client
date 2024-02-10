using UnityEngine;
using System.Collections;
using Util;
using UnityEngine.UI;
using GameInfo;

public class CreateRoom : MonoBehaviour
{
    public Text RoomName;

    public void CreateRoomButton()
    {
        Debug.Log($"방생성 테스트 : {RoomName.text}");
        
        var user = GameManager.Instance.User;
        var server = new ChatApiServer();
        var packet = new ChatApiRequest.CreateRoom();
        packet.UserName = user.UserName;
        packet.RoomName = RoomName.text;
        StartCoroutine(server.ChatApiRequest((int)RequestHeader.CreateRoom, packet));
    }

    public void CreateRoomCancelButton()
    {

        gameObject.SetActive(false);
    }
}

