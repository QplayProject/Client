using UnityEngine;
using System.Collections;

public class ChatBubble : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }
    // Update is called once per frame
    IEnumerator ActiveTimeCheck()
    {
        Debug.Log("코루틴 시작됨");
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        Debug.Log("자동 종료됨");
    }

    public void CloseChatBubble()
    {
        Debug.Log("강제 종료됨");
        StopCoroutine(ActiveTimeCheck());
        gameObject.SetActive(false);
    }

    public void OpenChatBubble()
    {
        StartCoroutine(ActiveTimeCheck());
    }
}

