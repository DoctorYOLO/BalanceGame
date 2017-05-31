using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private int window = 0;
	private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
	private RoomInfo[] roomsList;

	public string roomPassword;
	public string roomName = "RoomName";
	public string roomComment;
	public GUISkin skin;
	public string playerName = "player";
	public string verNum = "0.1";
	public Transform spawnPoint;
	public GameObject playerPref;
	public bool isInRoom = false;

	public InRoomChat chat;

	void Start () 
	{
		playerName = "player " + Random.Range (0, 999);
		roomName = "room " + Random.Range (0, 999);
		PhotonNetwork.ConnectUsingSettings(verNum);
	}

	void Update () 
	{
		if (isInRoom) {
			chat.enabled = true;
		} else {
			chat.enabled = false;
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;

		// Enter name
		if (window == 0) 
		{
			GUI.Label (new Rect (Screen.width * 0.50f, Screen.height * 0.25f, Screen.width * 0.15f, Screen.height * 0.12f), "Введите имя игрока");
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.40f, Screen.width * 0.15f, Screen.height * 0.12f), "Имя");
			playerName = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.39f, Screen.width * 0.15f, Screen.height * 0.05f), ""+playerName);
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.50f, Screen.width * 0.25f, Screen.height * 0.08f), "ОК")) 
			{
				window = 1;
			}
			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.10f), "Выход")) 
			{
				Application.Quit ();
			}
		}

		// Main menu
		if (window == 1) 
		{
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.35f, Screen.width * 0.30f, Screen.height * 0.10f), "Создать комнату")) 
			{
				window = 2;
			}
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.50f, Screen.width * 0.30f, Screen.height * 0.10f), "Присоединиться к комнате")) 
			{
				window = 3;
			}
			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.10f), "Выход")) 
			{
				Application.Quit ();
			}
		}

		// Create room
		if (window == 2) 
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
				window = 1;
			}
			if (PhotonNetwork.room == null) 
			{
				if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Создать"))
				{
					PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 3, isOpen = true, isVisible = true}, lobbyName);
					window = 5;
				}
			}

		}

		//Connect to room
		if (window == 3) 
		{

			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад"))
			{
				window = 1;
			}
				

			if (roomsList != null) 
			{
				for (int i = 0; i < roomsList.Length; i++) 
				{
					if (GUI.Button(new Rect(Screen.width * 0.10f, Screen.height * 0.10f + (110 * i), Screen.width * 0.15f, Screen.height * 0.12f), "Join " + roomsList[i].name)) 
					{
						PhotonNetwork.JoinOrCreateRoom(roomsList[i].name, null, null);
						window = 5;
					}
				}
			}

		}

		//Room for host
		if (window == 4) 
		{

			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Начать игру"))
			{
				PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 3, isOpen = true, isVisible = true}, lobbyName);

			}

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Покинуть комнату"))
			{
				PhotonNetwork.LeaveRoom ();
				window = 1;
			}
		}

		if (isInRoom) {
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.35f, Screen.width * 0.30f, Screen.height * 0.10f), "Начать игру")) 
			{
				spawnPlayer ();
			}
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Покинуть комнату"))
			{
				PhotonNetwork.LeaveRoom ();
				isInRoom = false;
				window = 1;
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
		PhotonNetwork.playerName = playerName;
		Debug.Log("Connected to Room");
		isInRoom = true;
		//spawnPlayer ();
	}

	public void spawnPlayer()
	{
		isInRoom = false;
		GameObject pl = PhotonNetwork.Instantiate (playerPref.name, spawnPoint.position, spawnPoint.rotation, 0) as GameObject;
		pl.GetComponent<ControlScript> ().enabled = true;
		pl.GetComponent<ControlScript> ().graphics.SetActive(false);
	}
}
