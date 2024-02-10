using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test: MonoBehaviour
{
    public GameObject test;
    private ChatBubble ttt;
    private void Start()
    {
        ttt = test.GetComponent<ChatBubble>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (test.activeSelf)
            {
                Debug.Log("강제 종료 시도!");

                ttt.CloseChatBubble();
            }

            Debug.Log("실행시킴!");
            test.SetActive(true);
            ttt.OpenChatBubble();
        }
    }
}
