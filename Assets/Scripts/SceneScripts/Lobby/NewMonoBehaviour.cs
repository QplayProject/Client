using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
	public Text RoomNumberText;
	public Text RoomMemberText;
	private int RoomNumber;
	private int CurrentMember;
	// Use this for initialization
	void Start()
	{
		RoomNumber = int.Parse(RoomNumberText.text);
		CurrentMember = int.Parse(RoomMemberText.text);
	}

	public void JoinRoom()
	{

	}
	// Update is called once per frame
	void Update()
	{
			
	}
}

