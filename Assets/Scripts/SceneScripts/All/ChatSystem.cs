using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Util;
using System;

public class ChatSystem: MonoBehaviour
{
    public int State;
    public ChatMessagePool MessagePool;
    public InputField ChatInputField;
    public GameObject Scroll;
    public GameObject SceneObject;
    private Action<int, string> UserChat;
    //private Scrollbar Bar;

    private void Start()
    {
        ChatInputField.onEndEdit.AddListener(SendChatMessage);
        ChatInputField.ActivateInputField();

        GameManager.Instance.Chat = AddChatMessage;
        if (State == (int)UserState.Room)
        {
            var chat = SceneObject.GetComponent<ChatRoom>();
            UserChat = chat.OpenChatBubble;
        }
        //Bar = Scroll.GetComponent<Scrollbar>();
    }
    async void SendChatMessage(string message)
    {
        // 입력된 텍스트를 지웁니다.
        if (!string.IsNullOrEmpty(message))
        {
            var packet = new ChatBase.Chat();
            packet.ChatType = (int)ChatType.All;
            packet.UserName = GameManager.Instance.User.UserName;
            packet.Message = message;
            await ServerManager.Instance.SendChatMessage((int)Opcode.Chat, packet);

            ChatInputField.text = string.Empty;
            ChatInputField.ActivateInputField();
        }
    }
    void AddChatMessage(ChatBase.Chat packet)
    {
        var message = $"{packet.UserName} : {packet.Message}";

        if (State == (int)UserState.Room)
        {
            var joinRoomUsers = GameManager.Instance.JoinRoom.JoinRoomUsersInfo;
            foreach (var user in joinRoomUsers)
            {
                string userName = user.Value.UserName;
                if (userName == packet.UserName)
                {
                    int slotNumber = user.Key;
                    UserChat?.Invoke(slotNumber, message);
                    break;
                }
            }

        }

        // 입력된 텍스트를 지웁니다.
        if (!string.IsNullOrEmpty(message))
        {
            MessagePool.GetMessage(message);
            //Bar.value = 0f;
        }
    }
}

