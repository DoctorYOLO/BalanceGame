using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenu : MonoBehaviour 
{
	/*
	 * window = 0: авторизация
	 * window = 1: меню
	 * window = 2: создание комнаты
	 * window = 3: присоединение к комнате
	 * window = 4: чат
	 * window = 5: комната
	 * window = 6: конец игры
	 * 
	 * 
	 * 
	 * 
	 */
	private int window = 0;
	private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
	private RoomInfo[] roomsList;
	private Vector2 scrollPos = Vector2.zero;

	public string roomPassword;
	public string roomName = "RoomName";
	public string roomComment;
	public GUISkin skin1;
	public GUISkin skin2;
	public string playerName = "player";
	public string verNum = "0.1";
	public Transform spawnPoint;
	public Transform[] spawnPoints;
	public GameObject[] playerPref;
	public bool isInRoom = false;
	public GameObject minimap;
	public GameObject disk;
	public GameObject background;
	public float timer;
	public bool isPaused;

	public PhotonView spawn;

	public InRoomChat chat;

	void Start () 
	{
		playerName = "Игрок " + Random.Range (0, 999);
		roomName = "Группа " + Random.Range (0, 999);
		PhotonNetwork.ConnectUsingSettings(verNum);
	}

	void Update () 
	{
		if ((window == 4) || (window == 8)) {
			chat.enabled = true;
		} else {
			chat.enabled = false;
		}
		/*
		if (window == 15) 
		{
			if (((disk.GetComponent<Transform> ().rotation.x) > -3) && ((disk.GetComponent<Transform> ().rotation.x) < 3) && ((disk.GetComponent<Transform> ().rotation.z) < 3) && ((disk.GetComponent<Transform> ().rotation.z) > -3)) 
			{
				if (timer < 2) 
				{
					timer += Time.deltaTime;
				}
				if (timer > 2) 
				{
					if (PhotonNetwork.isMasterClient) 
					{
						window = 6;
					} else 
					{
						window = 7;
					}
				}
			}
			if (((disk.GetComponent<Transform> ().rotation.x) < -3) && ((disk.GetComponent<Transform> ().rotation.x) > 3) && ((disk.GetComponent<Transform> ().rotation.z) > 3) && ((disk.GetComponent<Transform> ().rotation.z) < -3)) 
			{
				timer = 0;
			}
		
		}
		*/
	}

	void OnGUI()
	{
		GUI.skin = skin2;



		// Enter name
		if (window == 0) 
		{
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.15f, Screen.width * 0.50f, Screen.height * 0.30f), "Равновесие");
			GUI.skin = skin1;
			GUI.Label (new Rect (Screen.width * 0.42f, Screen.height * 0.40f, Screen.width * 0.30f, Screen.height * 0.12f), "Введите имя игрока");
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.60f, Screen.width * 0.15f, Screen.height * 0.12f), "Имя");
			playerName = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.60f, Screen.width * 0.15f, Screen.height * 0.05f), ""+playerName, 10);
			if (GUI.Button (new Rect (Screen.width * 0.40f, Screen.height * 0.70f, Screen.width * 0.25f, Screen.height * 0.08f), "ОК")) 
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
			GUI.skin = skin1;
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.35f, Screen.width * 0.30f, Screen.height * 0.10f), "Создать группу")) 
			{
				window = 2;
			}
			if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.50f, Screen.width * 0.30f, Screen.height * 0.10f), "Присоединиться")) 
			{
				window = 3;
			}
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Смена имени")) 
			{
				PhotonNetwork.Disconnect ();
				Application.LoadLevel (0);
			}
			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.10f), "Выход")) 
			{
				Application.Quit ();
			}
		}

		// Create room
		if (window == 2) 
		{
			GUI.skin = skin1;
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.25f, Screen.width * 0.30f, Screen.height * 0.12f), "Создание группы");

			GUI.Label (new Rect (Screen.width * 0.30f, Screen.height * 0.40f, Screen.width * 0.15f, Screen.height * 0.12f), "Название");
			roomName = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.39f, Screen.width * 0.20f, Screen.height * 0.05f), ""+roomName, 20);
			/*
			GUI.Label (new Rect (Screen.width * 0.30f, Screen.height * 0.50f, Screen.width * 0.15f, Screen.height * 0.12f), "Пароль");
			roomPassword = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.49f, Screen.width * 0.20f, Screen.height * 0.05f), ""+roomPassword);
*/
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад")) 
			{
				window = 1;
			}
			if (PhotonNetwork.room == null) 
			{
				if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Создать"))
				{
					PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 3, isOpen = true, isVisible = true}, lobbyName);
				}
			}

		}

		//Connect to room
		if (window == 3) 
		{
			GUI.skin = skin1;
			GUI.Label (new Rect (Screen.width * 0.45f, Screen.height * 0.15f, Screen.width * 0.50f, Screen.height * 0.30f), "Список групп");
			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад"))
			{
				window = 1;
			}
				

			if (roomsList != null) 
			{
				for (int i = 0; i < roomsList.Length; i++) 
				{
					if (GUI.Button(new Rect (Screen.width * 0.3f, Screen.height * 0.3f + (110 * i), Screen.width * 0.35f, Screen.height * 0.08f), roomsList[i].Name)) 
					{
						PhotonNetwork.JoinOrCreateRoom(roomsList[i].Name, null, null);
					}
				}
			}

		}

		//Chat in Room
		if (window == 4) 
		{
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад")) 
			{
				window = 5;
			}
		}

		//Chat in Game
		if (window == 8) 
		{
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад")) 
			{
				window = 15;
			}
		}
			
		//Room
		if (window == 5) 
		{
			GUI.skin = skin2;
			GUI.Label (new Rect (Screen.width * 0.30f, Screen.height * 0.15f, Screen.width * 0.50f, Screen.height * 0.30f), "Список игроков");
			GUI.skin = skin1;
			if (PhotonNetwork.isMasterClient) 
			{
				if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Начать"))
				{
					spawn.RPC ("spawnPlayer", PhotonTargets.AllBuffered, null);
				}
			}
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Чат")) 
			{
				window = 4;
			}
			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Выйти"))
			{
				PhotonNetwork.LeaveRoom ();
				isInRoom = false;
				chat.messages.Clear();
				window = 1;
			}
				
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				GUI.Box(new Rect (Screen.width * 0.3f, Screen.height * 0.3f + (110 * i), Screen.width * 0.35f, Screen.height * 0.08f), PhotonNetwork.playerList [i].NickName);
			}
		}

		//GameOver
		if (window == 6) 
		{
			GUI.skin = skin2;
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.15f, Screen.width * 0.50f, Screen.height * 0.30f), "Поздравляем!");
			GUI.skin = skin1;
			GUI.Label (new Rect (Screen.width * 0.42f, Screen.height * 0.40f, Screen.width * 0.30f, Screen.height * 0.12f), "Вы справились с поставленной задачей");

			spawn.RPC ("gameOver", PhotonTargets.AllBuffered, null);

			if (GUI.Button (new Rect (Screen.width * 0.40f, Screen.height * 0.70f, Screen.width * 0.25f, Screen.height * 0.08f), "Выйти"))
			{
				spawn.RPC ("resume", PhotonTargets.AllBuffered, null);
			}
		}

		if (window == 7) 
		{
			GUI.skin = skin2;
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.15f, Screen.width * 0.50f, Screen.height * 0.30f), "Поздравляем!");
			GUI.skin = skin1;
			GUI.Label (new Rect (Screen.width * 0.42f, Screen.height * 0.40f, Screen.width * 0.30f, Screen.height * 0.12f), "Вы справились с поставленной задачей");
		}

		//Game
		if (window == 15) 
		{
			GUI.skin = skin1;
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				GUI.Box(new Rect (Screen.width * 0.80f, Screen.height * 0.75f + (90 * i), Screen.width * 0.20f, Screen.height * 0.06f), PhotonNetwork.playerList [i].NickName + ",   " + "Масса: " + PhotonNetwork.playerList[i].GetScore() + " г.");
			}

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Чат")) 
			{
				window = 8;
			}

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Выйти"))
			{
				PhotonNetwork.LeaveRoom ();
				background.SetActive (true);
				minimap.SetActive(false);
				isInRoom = false;
				chat.messages.Clear();
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
		window = 5;
	}

	[PunRPC]
	public void spawnPlayer()
	{
		background.SetActive (false);
		window = 15;
		minimap.SetActive(true);
		GameObject pl = PhotonNetwork.Instantiate (playerPref[Random.Range(0, playerPref.Length)].name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, spawnPoint.rotation, 0) as GameObject;
		pl.GetComponent<ControlScript> ().enabled = true;
		pl.GetComponent<ControlScript> ().graphics.SetActive(false);
		PhotonNetwork.player.AddScore ((int) pl.GetComponent<ControlScript> ().mass);
	}

	[PunRPC]
	public void gameOver()
	{
		Time.timeScale = 0;
	}

	[PunRPC]
	public void resume()
	{
		Time.timeScale = 1;
		PhotonNetwork.LeaveRoom ();
		minimap.SetActive(false);
		background.SetActive (true);
		isInRoom = false;
		chat.messages.Clear();
		window = 1;
	}
}
