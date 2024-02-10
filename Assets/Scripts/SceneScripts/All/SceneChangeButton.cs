using UnityEngine;
using Util;

public class SceneChangeButton : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.ExitRoom = ExitRoom;
    }
    public void JoinRoom()
    {
        var selectRoomNumber = GameManager.Instance.SelectRoomNumber;
        if (selectRoomNumber == -1)
        {
            ServerManager.Instance.OpenMessageBox("입장하실 방을 선택해주세요.");
            return;
        }
        var user = GameManager.Instance.User;
        var webServer = new ChatApiServer();
        var packet = new ChatApiRequest.JoinRoom();
        packet.UserName = user.UserName;
        packet.RoomNumber = selectRoomNumber;
        StartCoroutine(webServer.ChatApiRequest((int)RequestHeader.JoinRoom, packet));
    }

    public void ExitRoom()
	{
        var user = GameManager.Instance.User;
        var webServer = new ChatApiServer();
        var packet = new ChatApiRequest.Packet();
        packet.UserName = user.UserName;
        
        StartCoroutine(webServer.ChatApiRequest((int)RequestHeader.ExitRoom, packet));
	}

    public void SceneChange(int state)
    {
        var user = GameManager.Instance.User;
        var webServer = new ChatApiServer();
        var packet = new ChatApiRequest.SceneChange();
        packet.UserName = user.UserName;
        packet.State = state;
        StartCoroutine(webServer.ChatApiRequest((int)RequestHeader.SceneChange, packet));
    }

    public void EquipItems()
    {
        var user = GameManager.Instance.User;
        var webServer = new ChatApiServer();
        var packet = new ChatApiRequest.EquipItems();
        packet.UserName = user.UserName;
        packet.Items = user.Items;
        StartCoroutine(webServer.ChatApiRequest((int)RequestHeader.EquipItems, packet));
    }


}

