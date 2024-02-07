using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ChatMessagePool messagePool;
    public InputField chatInput;

    // 채팅 메시지 생성
    public void SendMessageTest()
    {
        string messageText = chatInput.text;
        if (!string.IsNullOrEmpty(messageText))
        {
            Text message = messagePool.GetMessage(messageText);
            // 여기에서 채팅 메시지를 UI에 추가하는 등의 작업을 수행할 수 있습니다.

            //ReturnMessage(message);
        }
    }

}