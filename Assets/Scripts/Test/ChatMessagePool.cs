using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessagePool : MonoBehaviour
{
    public Text messagePrefab;
    public Transform content;
    public int poolSize = 100;

    private List<Text> messagePool;

    void Start()
    {
        InitializeObjectPool();
    }

    // 오브젝트 풀 초기화
    void InitializeObjectPool()
    {
        messagePool = new List<Text>();

        for (int i = 0; i < poolSize; i++)
        {
            Text message = Instantiate(messagePrefab, content);
            message.gameObject.SetActive(false);
            messagePool.Add(message);
        }
    }

    // 채팅 메시지 가져오기
    public Text GetMessage(string messageText)
    {
        Text messageObj = GetObjectFromPool();
        messageObj.text = messageText;

        // 풀의 크기가 설정된 poolSize보다 크다면 가장 오래된 메시지의 내용을 변경하고 삭제합니다.
        if (messagePool.Count > poolSize)
        {
            Text oldMessage = messagePool[0];
            oldMessage.text = messageText;

            // 리스트에서 해당 오브젝트를 제거하고 파괴합니다.
            messagePool.RemoveAt(0);
            Destroy(oldMessage.gameObject);
        }

        return messageObj;
    }

    // 오브젝트 풀에서 채팅 메시지 가져오기
    Text GetObjectFromPool()
    {
        foreach (Text message in messagePool)
        {
            if (!message.gameObject.activeInHierarchy)
            {
                message.gameObject.SetActive(true);
                return message;
            }
        }

        Debug.LogWarning("Object pool is empty. Creating a new object.");
        Text newMessage = Instantiate(messagePrefab, content);
        messagePool.Add(newMessage);

        return newMessage;
    }

    // 채팅 메시지 반환하기
    public void ReturnMessage(Text message)
    {
        message.gameObject.SetActive(false);
    }
}
