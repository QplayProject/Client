using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameInfo;

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
            ChatBubbles[i].SetActive(false);
            Characters[i].SetActive(false);
        }
        var gameManager = GameManager.Instance;
        var room = gameManager.UserRoom;
        var characters = gameManager.Characters;

        var member = room.CurrentMember;
        var roomName = room.RoomName;
        var roomNumber = gameManager.User.RoomNumber;
        RoomName.text = $"[No.{roomNumber}] [{member}/6] {roomName}";

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            if (character.UserName == "") continue;

            int slotNumber = character.SlotNumber;
            var chatCharacter = Characters[character.SlotNumber].GetComponent<ChatCharacter>();
            chatCharacter.SetChatCharacter(character);
            Characters[slotNumber].SetActive(true);
        }
    }

    private void JoinRoomMember(Game.JoinRoomMember callback)
    {
        var character = Characters[callback.SlotNumber];
        var gameManager = GameManager.Instance;

        var room = gameManager.UserRoom;
        room.CurrentMember = callback.CurrentMember;
        RoomName.text = $"[No.{room.RoomNumber}] [{room.CurrentMember}/6] {room.RoomName}";

        var joinUser = new Character();
        joinUser.SlotNumber = callback.SlotNumber;
        joinUser.UserName = callback.UserName;
        joinUser.Model = callback.Model;
        joinUser.Gender = callback.Gender;
        joinUser.Items = callback.EquipItems;
        var chatCharacter = character.GetComponent<ChatCharacter>();
        chatCharacter.SetChatCharacter(joinUser);


        character.SetActive(true);
    }
    private void ExitRoomMember(Game.ExitRoomMember callback)
    {
        var gameManager = GameManager.Instance;
        var room = gameManager.UserRoom;
        room.CurrentMember = callback.CurrentMember;
       
        
        var exitUser = gameManager.Characters[callback.SlotNumber];

        RoomName.text = $"[No.{room.RoomNumber}] [{room.CurrentMember}/6] {room.RoomName}";
        exitUser.UserName = "";
        exitUser.Gender = -1;
        exitUser.Model = -1;
        exitUser.Items.Clear();
        
        Characters[callback.SlotNumber].SetActive(false);


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

