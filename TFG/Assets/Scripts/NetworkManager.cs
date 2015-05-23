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
	public PlayerInfo[] listaJugadores = new PlayerInfo[numJugadores];

	public NetworkView networkView;

	public static NetworkManager networkManagerRef;

	private string nombreJugador = "DefaultName";

	private const bool dedicatedServer = false;

	private bool personajesGenerados = false;

	//TODO: DESACOPLAR ESTO DE AQUI, PLS
	// Funcion para meter el nombre a pelo
	public void entradaLocalNombre(string name)
	{
		nombreJugador = name;
	}

	public void Awake()
	{
		if(networkManagerRef != null)
		{
			Destroy(networkManagerRef.gameObject);
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

	// Funcion que se invocara cuando se inicialize el servidor
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		Application.LoadLevel("LobbyScene");

		// Si ademas tambien es un cliente
		if(!dedicatedServer)
		{
			// Lo insertamos en la lista
			InsertarClienteEnLista(Network.player);

			// Broadcasteamos el nombre al resto de clientes
			bcstName(Network.player, nombreJugador);
			networkView.RPC("bcstName", RPCMode.OthersBuffered, Network.player, nombreJugador);
		}
	}

	// Evento que recibe el servidor al conectarse un jugador
	void OnPlayerConnected(NetworkPlayer player) 
	{
		InsertarClienteEnLista(player);

		// Preguntamos el nombre al jugador recien conectado
		networkView.RPC("askName", player);
	}

	// Funcion que inserta un cliente en la lista de jugadores y lo notifica a los demas clientes
	public void InsertarClienteEnLista(NetworkPlayer player)
	{
		for(int i=0; i<listaJugadores.Length; i++)
		{
			// Cuando encontremos un hueco libre
			if(!listaJugadores[i].activePlayer)
			{
				// Lo asignamos
				listaJugadores[i].activePlayer = true;
				listaJugadores[i].networkPlayer = player;
				listaJugadores[i].isReady = false;
				// Sincronizamos el jugador y su posicion en la lista con el resto de clientes
				networkView.RPC("jugCon", RPCMode.OthersBuffered, player, i);
				break;
			}
		}
	}

	// Evento que recibe el servidor al desconectarse un jugador (manualmente o con timeout)
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);

		// Quitamos el jugador de nuestra lista de jugadores
		for(int i=0; i<listaJugadores.Length; i++)
		{
			// Y cuando encontremos el que es lo descoenctamos
			if(listaJugadores[i].networkPlayer == player)
			{
				Debug.Log(listaJugadores[i].playerName + "se ha desconectado"); 
				listaJugadores[i].resetPlayer();

				// Notificamos a los clientes su desconexion
				networkView.RPC("jugDescon", RPCMode.OthersBuffered, player, i);
				break;
			}
		}
	}

	// RPC que se invocara cuando se conecte un jugador nuevo, sirve para sincronizarlo con el resto de clientes
	[RPC]
	void jugCon(NetworkPlayer player, int playerIndex)
	{
		listaJugadores[playerIndex].activePlayer = true;
		listaJugadores[playerIndex].networkPlayer = player;
	}

	// RPC que se invocara cuando se desconecte un jugador, sirve para sincronizarlo con el resto de clientes
	[RPC]
	void jugDescon(NetworkPlayer player, int playerIndex)
	{
		Debug.Log(listaJugadores[playerIndex].playerName + "se ha desconectado"); 
		listaJugadores[playerIndex].resetPlayer();
	}

	[RPC]
	void askName()
	{	
		networkView.RPC("bcstName", RPCMode.AllBuffered, Network.player, nombreJugador);
	}
	
	[RPC]
	void bcstName(NetworkPlayer player, string name)
	{	
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(listaJugadores[i].networkPlayer == player)
			{
				listaJugadores[i].playerName = name;
				listaJugadores[i].isReady = false;
				Debug.Log(listaJugadores[i].playerName + "se ha conectado");
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

	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		Application.LoadLevel("MenuScene");
	}

	public void setPlayerReady (NetworkPlayer player)
	{
		networkView.RPC("plyrIsRdy", RPCMode.AllBuffered, Network.player);
	}

	// FUncion que recibira el servidor y los clientes cuando un jugador pulse que esta listo
	[RPC]
	public void plyrIsRdy(NetworkPlayer player)
	{
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(listaJugadores[i].networkPlayer == player)
			{
				listaJugadores[i].isReady = !listaJugadores[i].isReady;
				break;
			}
		}

		if(Network.isServer && !personajesGenerados)
		{
			comprobarTodosListos();
		}
	}

	public void comprobarTodosListos()
	{
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(!listaJugadores[i].isReady)
			{
				return;
			}
		}		 

		Debug.Log("Todos los jugadores listos");
		StartCoroutine(corutinaGenerarPersonajesAleatorios());
		networkView.RPC("strtCountdwn", RPCMode.All, 10);
	}

	// Genera personajes aleatorios y los comunica a los clientes
	IEnumerator corutinaGenerarPersonajesAleatorios()
	{
		personajesGenerados = true;

		yield return new WaitForSeconds(.5f);

		// Asignamos el humano a un jugador y lo comunicamos al resto de clientes
		int indiceHumano = (int)Random.Range(0, listaJugadores.Length); 
		listaJugadores[indiceHumano].enumPersonaje = EnumPersonaje.Humano;
		networkView.RPC("bcstChar", RPCMode.OthersBuffered, indiceHumano, (int)listaJugadores[indiceHumano].enumPersonaje);

		yield return new WaitForSeconds(2f);

		// Creamos una lista con todos los personajes restantes
		List<EnumPersonaje> listaPersonajes = new List<EnumPersonaje>();
		for(int i=2; i< System.Enum.GetValues(typeof(EnumPersonaje)).Length; i++)
		{
			listaPersonajes.Add((EnumPersonaje) i);
		}
		
		// Recorremos la lista de jugadores y les vamos asignando personajes aleatorios
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(i != indiceHumano)
			{
				int randomCharacterIndex = Random.Range(0, listaPersonajes.Count);
				
				listaJugadores[i].enumPersonaje = listaPersonajes[randomCharacterIndex];
				networkView.RPC("bcstChar", RPCMode.OthersBuffered, i, (int)listaJugadores[i].enumPersonaje);
				listaPersonajes.RemoveAt(randomCharacterIndex);
				yield return new WaitForSeconds(0.25f);
			}
		}


	//	yield return new WaitForSeconds(5f);
	//	CargarPantallaJuego();
	}

	public void CargarPantallaJuego()
	{
		Application.LoadLevel("test");
	}

	// FUncion que recibiran los clientes para comunicar el personaje que le toca a cada uno
	[RPC]
	void bcstChar(int indiceJugador, int enumPersonajeEntero)
	{
		listaJugadores[indiceJugador].enumPersonaje = (EnumPersonaje) enumPersonajeEntero;
	}

	// Funcion que recibiran los clientes para empezar la cuenta atras
	[RPC]
	void strtCountdwn(int segundos)
	{
		LobbyManager.lobbyManager.StartCountDown(segundos);
	}
}