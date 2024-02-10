using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    void Start()
    {
        var serverManager = ServerManager.Instance;
        serverManager.MessageBox = OpenMessageBox;
        gameObject.SetActive(false);
    }
    public void OpenMessageBox(string message)
    {

        gameObject.SetActive(true);
        var Message = gameObject.transform.Find("Message").gameObject.GetComponent<Text>();
        Message.text = message;
        
    }
    public void OnClickMessageBoxButton()
    {
        var Button = gameObject;
        Button.SetActive(false);
    }
}

