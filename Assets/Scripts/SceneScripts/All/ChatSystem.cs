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
            var user = GameManager.Instance.User;
            var packet = new Chat.Packet();
            packet.UserName = user.UserName;
            packet.State = user.State;
            packet.RoomNumber = user.RoomNumber;
            packet.Message = message;
            await ServerManager.Instance.SendChatMessage(packet);

            ChatInputField.text = string.Empty;
            ChatInputField.ActivateInputField();
        }
    }
    void AddChatMessage(Chat.Packet packet)
    {
        var message = $"{packet.UserName} : {packet.Message}";

        if (State == (int)UserState.Room)
        {
            var characters = GameManager.Instance.Characters;
            for (int i = 0; i < characters.Count; i++)
            {
                var character = characters[i];
                if (character.UserName == packet.UserName)
                {
                    UserChat?.Invoke(character.SlotNumber, message);
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

