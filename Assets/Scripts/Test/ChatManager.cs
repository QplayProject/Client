using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ChatMessagePool messagePool;
    public InputField chatInput;

    public GameObject Scroll;
    private Scrollbar Bar;
    public GameObject Content;
    private RectTransform rect;
    private void Start()
    {
        Bar = Scroll.GetComponent<Scrollbar>();
        rect = Content.GetComponent<RectTransform>();
    }
    // 채팅 메시지 생성
    public void SendMessageTest()
    {
        string messageText = chatInput.text;
        if (!string.IsNullOrEmpty(messageText))
        {
            Text message = messagePool.GetMessage(messageText);

            Bar.value = 0f;
            float currentHeight = rect.sizeDelta.y;
            var pos = rect.anchoredPosition.y + 20;//currentHeight - 200;
            var x = rect.anchoredPosition.x;
            Debug.Log($"변경전 요청 위치 :{pos}/ 현재 위치 : {rect.anchoredPosition.y}");
            rect.anchoredPosition = new Vector2(x, pos);
            Debug.Log($"변경후 요청 위치 :{pos}/ 현재 위치 : {rect.anchoredPosition.y}");
        }
    }

}