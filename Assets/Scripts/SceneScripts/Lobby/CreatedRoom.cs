using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatedRoom : MonoBehaviour
{
    public Text RoomNumberText;
    public Text Member;
    public Text RoomName;

    private int RoomNumber;
    public void SetCreatedRoom(int roomNumber, int member, string roomName)
    {
        RoomNumber = roomNumber;
        RoomNumberText.text = $"{roomNumber} 번방";
        Member.text = member.ToString();
        RoomName.text = roomName;
    }

    public void SelectRoom()
    {
        var selectRoomNumber = GameManager.Instance.SelectRoomNumber;
        if (selectRoomNumber == RoomNumber)
        {
            GameManager.Instance.SelectRoomNumber = -1;
        }
        else
        {
            GameManager.Instance.SelectRoomNumber = RoomNumber;
        }
    }
}

