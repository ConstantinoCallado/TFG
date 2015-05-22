using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
	private const string masterServerIP = "127.0.0.1";
	private const int numJugadores = 5;
	private const string typeName = "Robot-Name";
	private string gameName = "RoomName";

	private HostData[] hostList;
	public PlayerInfo[] listaJugadores = new PlayerInfo[5];

	public static NetworkManager networkManagerRef;

	public void Awake()
	{
		if(networkManagerRef != null)
		{
			Destroy(networkManagerRef);
		}

		networkManagerRef = this;
		DontDestroyOnLoad(gameObject);
	}
	
	// Lado del servidor
	public void StartServer()
	{
		//MasterServer.ipAddress = masterServerIP;
		Network.InitializeServer(numJugadores, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		Application.LoadLevel("LobbyScene");
	}

	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);


		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(listaJugadores[i].networkPlayer == player)
			{
				listaJugadores[i].resetPlayer();
				break;
			}
		}
	}

	void OnPlayerConnected(NetworkPlayer player) 
	{
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(!listaJugadores[i].activePlayer)
			{
				listaJugadores[i].activePlayer = true;
				listaJugadores[i].networkPlayer = player;
				break;
			}
		}
	}

	// Lado del cliente
	public void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();

			if(hostList.Length > 0)
			{
				JoinServer(hostList[0]);
			}
		}
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		Application.LoadLevel("LobbyScene");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
