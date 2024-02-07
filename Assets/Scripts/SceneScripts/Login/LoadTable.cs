using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LoadTable : MonoBehaviour
{
    public float version = 0.1f;
    public Text versionTxt;
    // Start is called before the first frame update
    void Start()
    {
        var loginServer = new LoginServer();
        var request = new LoginRequest.LoadTable();
        request.Version = version;
        StartCoroutine(loginServer.LoadTable(request));
        versionTxt.text = $"Version. {version}";
    }
}

