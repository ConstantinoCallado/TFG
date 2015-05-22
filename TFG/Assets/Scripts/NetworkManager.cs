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

	public NetworkView networkView;

	public static NetworkManager networkManagerRef;

	private string nombreJugador = "DefaultName";

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
		// Quitamos el jugador de nuestra lista de jugadores
		jugDescon(player);

		// Invocamos la RPC para todos los clientes
		networkView.RPC("jugDescon", RPCMode.OthersBuffered, player);
	}

	void OnPlayerConnected(NetworkPlayer player) 
	{
		// Anyadimos el jugador a nuestra lista de jugadores
		jugCon(player);

		// Invocamos la RPC para todos los clientes
		networkView.RPC("jugCon", RPCMode.OthersBuffered, player);
		networkView.RPC("askName", player);
	}
	
	public void entradaLocalNombre(string name)
	{
		Debug.Log(name);
		nombreJugador = name;
	}

	[RPC]
	void jugCon(NetworkPlayer player)
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

	[RPC]
	void jugDescon(NetworkPlayer player)
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

	[RPC]
	void setName(NetworkPlayer player, string name)
	{	
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(listaJugadores[i].networkPlayer == player)
			{
				listaJugadores[i].playerName = name;
				break;
			}
		}
	}

	[RPC]
	void askName()
	{	
		networkView.RPC("setName", RPCMode.AllBuffered, Network.player, nombreJugador);
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
