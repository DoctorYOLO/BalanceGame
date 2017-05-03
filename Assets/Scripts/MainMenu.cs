using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{

	private int window = 0;
	private int port = 25000;
	private string serverPassword;
	private string serverName;
	private string serverComment;
	private bool startServer = false;
	public GUISkin skin;

	// Use this for initialization
	void Start () 
	{
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "127.0.0.1";
		Network.natFacilitatorPort = 50005;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnGUI () 
	{
		GUI.skin = skin;

		// Main menu
		if (window == 0) 
		{
			if (GUI.Button (new Rect (Screen.width * 0.45f, Screen.height * 0.35f, Screen.width * 0.15f, Screen.height * 0.10f), "Создать комнату")) 
			{
				window = 1;
			}
			if (GUI.Button (new Rect (Screen.width * 0.45f, Screen.height * 0.50f, Screen.width * 0.15f, Screen.height * 0.10f), "Присоединиться к комнате")) 
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
			//port = GUI.TextField (new Rect (Screen.width / 2 + 15, Screen.height / 2 - 100, 85, 25), port);

			GUI.Label (new Rect (Screen.width * 0.50f, Screen.height * 0.25f, Screen.width * 0.15f, Screen.height * 0.12f), "Создание комнаты");

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.40f, Screen.width * 0.15f, Screen.height * 0.12f), "Название");
			serverName = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.39f, Screen.width * 0.15f, Screen.height * 0.05f), ""+serverName);

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.50f, Screen.width * 0.15f, Screen.height * 0.12f), "Пароль");
			serverPassword = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.49f, Screen.width * 0.15f, Screen.height * 0.05f), ""+serverPassword);

			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height * 0.60f, Screen.width * 0.15f, Screen.height * 0.12f), "Описание");
			serverComment = GUI.TextField (new Rect (Screen.width * 0.50f, Screen.height * 0.59f, Screen.width * 0.15f, Screen.height * 0.05f), ""+serverComment);

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Назад")) 
			{
				window = 0;
			}
			if (startServer == false) 
			{
				if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.80f, Screen.width * 0.15f, Screen.height * 0.12f), "Создать"))
				{
					Network.incomingPassword = serverPassword;
					Network.InitializeServer (3, port, true);
					MasterServer.RegisterHost("Rooms", serverName, serverComment);
					startServer = true;
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

			if (GUI.Button (new Rect (Screen.width * 0.80f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.10f), "Обновить")) 
			{
				MasterServer.ClearHostList ();
				MasterServer.RequestHostList("Rooms");
			}

			HostData[] hostData = MasterServer.PollHostList ();
			for(int i = 0; i<hostData.Length; i++)
			{
				GUI.Label(new Rect (Screen.width * 0.05f, Screen.height * 0.05f + i*25, Screen.width * 0.15f, Screen.height * 0.12f), hostData[i].gameName);
				GUI.Label(new Rect (Screen.width * 0.25f, Screen.height * 0.05f + i*25, Screen.width * 0.15f, Screen.height * 0.12f), hostData[i].comment);
				GUI.Label(new Rect (Screen.width * 0.50f, Screen.height * 0.05f + i*25, Screen.width * 0.15f, Screen.height * 0.12f), hostData[i].connectedPlayers+"/"+hostData[i].playerLimit);
				if (hostData [i].passwordProtected == true) 
				{
					GUI.Label(new Rect (Screen.width * 0.65f, Screen.height * 0.05f + i*25, Screen.width * 0.15f, Screen.height * 0.12f), "с паролем");
				}
				GUI.Label(new Rect (Screen.width * 0.65f, Screen.height * 0.05f + i*25, Screen.width * 0.15f, Screen.height * 0.12f), "без пароля");
			}
		}

		//Room for host
		if (window == 3) 
		{

			if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.12f), "Удалить комнату"))
			{
				startServer = false;
				Network.Disconnect ();
			}
		}
	
	}
}
