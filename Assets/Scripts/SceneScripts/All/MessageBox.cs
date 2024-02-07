using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        gameObject.SetActive(false);
    }

    public void OnClickMessageBoxButton()
    {
        var Button = gameObject;
        Button.SetActive(false);
    }
}

