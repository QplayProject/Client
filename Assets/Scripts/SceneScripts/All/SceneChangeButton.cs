using System.Collections;
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
        var webServer = new ApiServer();
        var packet = new ApiRequest.JoinRoom();
        packet.UserName = user.UserName;
        packet.RoomNumber = selectRoomNumber;
        StartCoroutine(webServer.ApiRequestJoinRoom(packet));
    }

    public void ExitRoom()
	{
        var user = GameManager.Instance.User;
        var webServer = new ApiServer();
        var packet = new ApiRequest.Packet();
        packet.UserName = user.UserName;
        
        StartCoroutine(webServer.ApiRequestExitRoom(packet));
	}

    public void SceneChange(int state)
    {
        var user = GameManager.Instance.User;
        var webServer = new ApiServer();
        var packet = new ApiRequest.SceneChange();
        packet.UserName = user.UserName;
        packet.State = state;
        StartCoroutine(webServer.ApiRequestSceneChange(packet));
    }
    public void EquipItemAndSceneChange(int state)
    {
        StartCoroutine(WaitForEquipItems(state));
    }
    private IEnumerator WaitForEquipItems(int state)
    {
        var user = GameManager.Instance.User;
        var webServer = new ApiServer();
        var equipItempacket = new ApiRequest.EquipItems();
        equipItempacket.UserName = user.UserName;
        equipItempacket.Items = user.Items;
        Debug.Log(equipItempacket.Items);
        yield return StartCoroutine(webServer.ApiRequestEquipItems(equipItempacket));


        var packet = new ApiRequest.SceneChange();
        packet.UserName = user.UserName;
        packet.State = state;
        StartCoroutine(webServer.ApiRequestSceneChange(packet));
    }
    public void EquipItems()
    {
        var user = GameManager.Instance.User;
        var webServer = new ApiServer();
        var packet = new ApiRequest.EquipItems();
        packet.UserName = user.UserName;
        packet.Items = user.Items;
        Debug.Log(packet.Items);
        StartCoroutine(webServer.ApiRequestEquipItems(packet));
    }


}

