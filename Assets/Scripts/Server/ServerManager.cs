using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public GameObject MessageBox;

    public Dictionary<int, Table.ItemInfo> ItemTable = new Dictionary<int, Table.ItemInfo>();
    public Dictionary<int, Table.ShopItemInfo> ShopTable = new Dictionary<int, Table.ShopItemInfo>();


    public TcpClient ChatTcpClient = new TcpClient();
    public NetworkStream ChatNetworkStream;
    public string ChatServerIp = "127.0.0.1";
    public int ChatServerPort = 12345;

    public async Task SendMessageAsync(string message)
    {
        if (ChatTcpClient != null && ChatTcpClient.Connected)
        {
            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                await ChatNetworkStream.WriteAsync(data, 0, data.Length);
                Debug.Log($"Sent message: {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error sending message: {e.Message}");
            }
        }
    }

    public void OpenMessageBox(string message)
    {
        MessageBox.SetActive(true);
        var Message = MessageBox.transform.Find("Message").gameObject.GetComponent<Text>();
        Message.text = message;
    }
}
