using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class ChatRoom : MonoBehaviour
{
    public GameObject[] Characters;
    public GameObject[] ChatBubbles;
    public Text[] ChatBubblesText;
    public Text RoomName;
    // Use this for initialization
    void Start()
    {
        GameManager.Instance.JoinRoomMember = JoinRoomMember;
        GameManager.Instance.ExitRoomMember = ExitRoomMember;
        for(int i= 0; i< Characters.Length; i++)
        {
            //ChatBubbles[i] = Characters[i].transform.Find("ChatBubble").gameObject;
            //ChatBubblesText[i] = ChatBubbles[i].transform.Find("Text").GetComponent<Text>();
            ChatBubbles[i].SetActive(false);
            Characters[i].SetActive(false);
        }
        var gameManager = GameManager.Instance;
        var joinRoomInfo = gameManager.JoinRoom;

        var member = joinRoomInfo.CurrentMember;
        var roomName = joinRoomInfo.RoomName;
        var roomNumber = gameManager.User.RoomNumber;
        RoomName.text = $"[No.{roomNumber}] [{member}/6] {roomName}";

        foreach(var joinUser in joinRoomInfo.JoinRoomUsersInfo)
        {
            if (joinUser.Value.UserName == "" || joinUser.Value.UserName == null) continue;

            int slotNumber = joinUser.Key;
            var character = Characters[slotNumber].GetComponent<ChatCharacter>();
            character.SetChatCharacter(joinUser.Value);
            Characters[slotNumber].SetActive(true);
        }
    }

    private void JoinRoomMember(ChatBase.JoinRoomMember callback)
    {
        var character = Characters[callback.SlotNumber];
        var gameManager = GameManager.Instance;

        var roomNumber = gameManager.User.RoomNumber;
        if (roomNumber == callback.RoomNumber)
        {
            var joinUser = new GameInfo.JoinRoomUserInfo();
            joinUser.UserName = callback.UserName;
            joinUser.Model = callback.Model;
            joinUser.Gender = callback.Gender;
            joinUser.EquipItems = callback.EquipItems;
            var chatCharacter = character.GetComponent<ChatCharacter>();
            chatCharacter.SetChatCharacter(joinUser);

            character.SetActive(true);
        }
        else
            ServerManager.Instance.OpenMessageBox("JoinRoomMember! 잘못된 메시지 들어옴");
    }
    private void ExitRoomMember(ChatBase.ExitRoomMember callback)
    {
        var gameManager = GameManager.Instance;
        var joinRoomInfo = gameManager.JoinRoom;

        var roomName = joinRoomInfo.RoomName;
        var roomNumber = gameManager.User.RoomNumber;
        if (roomNumber == callback.RoomNumber)
        {
            var exitUser = joinRoomInfo.JoinRoomUsersInfo[callback.SlotNumber];
            if (exitUser.UserName == callback.UserName)
            {
                RoomName.text = $"[No.{roomNumber}] [{callback.CurrentMember}/6] {roomName}";
                exitUser.UserName = null;
                Characters[callback.SlotNumber].SetActive(false);
            }
        }
        else
            ServerManager.Instance.OpenMessageBox("ExitRoomMember! 잘못된 메시지 들어옴");

    }

    public void OpenChatBubble(int slotNumber, string message)
    {
        Debug.Log($"TEST: [{slotNumber}] {message}");
        var chatBubble = ChatBubbles[slotNumber].GetComponent<ChatBubble>();
        ChatBubbles[slotNumber].SetActive(true);

        //chatBubble.CloseChatBubble();
        ChatBubblesText[slotNumber].text = message;
        chatBubble.OpenChatBubble();
    }
}

