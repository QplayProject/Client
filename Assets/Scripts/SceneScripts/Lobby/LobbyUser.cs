using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Util;

public class LobbyUser : MonoBehaviour
{
    public Text State;
    public Text UserName;

    public void SetLobbyUser(string userName, int state, int roomNumber = 0)
    {
        UserName.text = userName; 
        switch (state)
        {
            case (int)UserState.Room:
                State.text = $"{roomNumber}번방";
                break;
            case (int)UserState.Lobby:
                State.text = "로비";
                break;
            case (int)UserState.BeautyRoom:
                State.text = "분장실";
                break;
            case (int)UserState.Shop:
                State.text = "상점";
                break;
            case (int)UserState.Logout:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }

    }
}

