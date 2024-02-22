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
        var user = GameManager.Instance.User;
        var server = new ApiServer();
        var packet = new ApiRequest.CreateRoom();
        packet.UserName = user.UserName;
        packet.RoomName = RoomName.text;
        StartCoroutine(server.ApiRequestCreateRoom(packet));
    }

    public void CreateRoomCancelButton()
    {

        gameObject.SetActive(false);
    }
}

