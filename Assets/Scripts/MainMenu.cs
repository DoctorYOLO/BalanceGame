using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private int window = 0;
	private string roomPassword;
	private string roomName = "RoomName";
	private string roomComment;
	public GUISkin skin;
	private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
	private RoomInfo[] roomsList;

	void Start () 
	{
		PhotonNetwork.ConnectUsingSettings("v4.2");
	}

	void Update () 
	{

	}

	void OnGUI()
	{
		GUI.skin = skin;

		if (window == 0) 
		{
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.35f, Screen.width * 0.30f, Screen.height * 0.10f), "Создать комнату")) 
			{
				window = 1;
			}
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.50f, Screen.width * 0.30f, Screen.height * 0.10f), "Присоединиться к комнате")) 
			{
				window = 2;
			}
			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.10f), "Выход")) 
			{
				Application.Quit ();
			}
		}

		// Create room
		if (window == 1) 
		{
			GUI.Label (new Rect (Screen.width * 0.50f, Screen.height * 0.25f, Screen.width * 0.15f, Screen.height * 0.12f), "Создание комнаты");

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.40f, Screen.width * 0.15f, Screen.height * 0.12f), "Название");
			roomName = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.39f, Screen.width * 0.15f, Screen.height * 0.05f), ""+roomName);

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.50f, Screen.width * 0.15f, Screen.height * 0.12f), "Пароль");
			roomPassword = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.49f, Screen.width * 0.15f, Screen.height * 0.05f), ""+roomPassword);

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.60f, Screen.width * 0.15f, Screen.height * 0.12f), "Описание");
			roomComment = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.59f, Screen.width * 0.15f, Screen.height * 0.05f), ""+roomComment);

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад")) 
			{
				window = 0;
			}
			if (PhotonNetwork.room == null) 
			{
				if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Создать"))
				{
					PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 3, isOpen = true, isVisible = true}, lobbyName);
					window = 3;
				}
			}

		}

		//Connect to room
		if (window == 2) 
		{

			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад"))
			{
				window = 0;
			}
				

			if (roomsList != null) 
			{
				for (int i = 0; i < roomsList.Length; i++) 
				{
					if (GUI.Button(new Rect(Screen.width * 0.10f, Screen.height * 0.10f + (110 * i), Screen.width * 0.15f, Screen.height * 0.12f), "Join " + roomsList[i].name)) 
					{
						PhotonNetwork.JoinRoom(roomsList[i].name);
						window = 3;
					}
				}
			}

		}

		//Room for host
		if (window == 3) 
		{

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Покинуть комнату"))
			{
				PhotonNetwork.LeaveRoom ();
				window = 0;
			}
		}			
	}

	void OnConnectedToMaster() 
	{
		PhotonNetwork.JoinLobby(lobbyName);
	}

	void OnReceivedRoomListUpdate()
	{
		Debug.Log ("Room was created");
		roomsList = PhotonNetwork.GetRoomList();
	}

	void OnJoinedLobby () 
	{
		Debug.Log ("Joined Lobby");
	}

	void OnJoinedRoom ()
	{
		Debug.Log("Connected to Room");
	}
}
