//#define jumpNetworkConnectionCheck

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;


public class NetworkManager : MonoBehaviour 
{
	private const string masterServerIP = "localhost";
	private const int masterServerPort = 23466;

	private const int numJugadores = 5;
	private const string typeName = "RobotMan";
	private string gameName = "room";
	private HostData[] hostList;
	public PlayerInfo[] listaJugadores = new PlayerInfo[numJugadores];
	public NetworkPlayer networkPlayerServer;
	public NetworkView networkView;
	
	public static NetworkManager networkManagerRef;
	
	public string nombreJugador = "DefaultName";
	
	private const bool dedicatedServer = false;
	
	private bool personajesGenerados = false;
	private bool partidaEmpezada = false;

	public GameObject panelNoPartidasEncontradas;
	public GameObject panelNoInternet;
	public GameObject panelInternetRestringido;
	public GameObject panelSpinner;

	private bool serverLaunched = false;

	public short humanLifes = 5;
	
	public short piezasRecogidas;
	public short piezasRestantes;
	public float tiempoPartida;
	public bool humanWin = false;

	public void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if(networkManagerRef != null)
		{
			Destroy(networkManagerRef.gameObject);
		}
		
		networkManagerRef = this;
		DontDestroyOnLoad(gameObject);

		//MasterServer.ipAddress = masterServerIP;
		//MasterServer.port = masterServerPort;
	}
	
	// Lado del servidor
	public void RequestLaunchServer()
	{
		panelSpinner.SetActive(true);

		#if !jumpNetworkConnectionCheck
			if(checkInternet())
			{
				RegisterServer ();
			}
		#else
			RegisterServer();
			LaunchServer();
		#endif
	}

	private void RegisterServer()
	{
		Debug.Log("Registrando");
		Network.InitializeServer(numJugadores, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	
	}

	public bool checkInternet()
	{
		WebResponse response = null;

		try
		{
			WebRequest webResquest = WebRequest.Create("http://www.google.com/");
			webResquest.Timeout = 2000;

			response = webResquest.GetResponse();

			if(response.GetResponseStream() != null)
			{
				response.Close();

				return true;
			}
		}
		catch(Exception e)
		{
			if(response != null) response.Close();
		}
	
		panelSpinner.SetActive(false);
		panelNoInternet.SetActive(true);
		return false;
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
				listaJugadores[i].index = i;

				// Sincronizamos el jugador y su posicion en la lista con el resto de clientes
				networkView.RPC("jugCon", RPCMode.OthersBuffered, player, i);
				break;
			}
		}
	}
	
	// Evento que recibe el servidor al desconectarse un jugador (manualmente o con timeout)
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		if(Application.loadedLevelName != "ScoreScene")
		{
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
			
			// Quitamos el jugador de nuestra lista de jugadores
			for(int i=0; i<listaJugadores.Length; i++)
			{
				// Y cuando encontremos el que es lo descoenctamos
				if(listaJugadores[i].networkPlayer == player)
				{
					Debug.Log(listaJugadores[i].playerName + " se ha desconectado"); 
					listaJugadores[i].resetPlayer();
					
					// Notificamos a los clientes su desconexion
					networkView.RPC("jugDescon", RPCMode.OthersBuffered, player, i);
					break;
				}
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
		Debug.Log(listaJugadores[playerIndex].playerName + " se ha desconectado"); 
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
				Debug.Log(listaJugadores[i].playerName + " se ha conectado");

				if(listaJugadores[i].networkPlayer == Network.player)
				{
					listaJugadores[i].ownByClient = true;
				}
				
				break;
			}
		}
	}

	// Funcion que comprueba internet y conecta a un host
	public void RequestRefreshHost()
	{
		panelSpinner.SetActive(true);

		#if !jumpNetworkConnectionCheck
		if(checkInternet())
		{
			RefreshHostList();
		}
		#else
		RefreshHostList();
		#endif

		
	}
	
	public void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	public void LaunchServer()
	{
		if(!serverLaunched)
		{
			serverLaunched = true;
			Debug.Log("Server Initializied");
			panelSpinner.SetActive(false);
			
			Application.LoadLevel("LobbyScene");
			
			// Enviamos al resto de cliente la informacion del servidor
			networkView.RPC("stsrvr", RPCMode.OthersBuffered, Network.player);
			
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
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		Debug.Log("MasterServerEvent");
	

		// Si el servidor se registra bien... cargamos la escena de looby
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
		{
			LaunchServer();
		}
		else if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();

			if(hostList.Length > 0)
			{
				for(int i=0; i< hostList.Length; i++)
				{
					if(hostList[i].connectedPlayers < hostList[i].playerLimit)
					{
						JoinServer(hostList[i]);
					
						return;
					}
				}
			}

			Debug.Log("No se han encontrado partidas");
			panelNoPartidasEncontradas.SetActive(true);
			panelSpinner.SetActive(false);

			MasterServer.ClearHostList();
		}
	}
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info) 
	{
		Debug.Log("Could not connect to master server: " + info);
		panelInternetRestringido.SetActive(true);
		panelSpinner.SetActive(false);
	
	}
	
	void OnFailedToConnect(NetworkConnectionError error) 
	{
		Debug.Log("Could not connect to server: " + error);
		panelInternetRestringido.SetActive(true);
		panelSpinner.SetActive(false);
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
		if(Application.loadedLevelName != "ScoreScene")
		{
			Application.LoadLevel("MenuScene");
		}
	}
	
	public void setPlayerReady (NetworkPlayer player)
	{
		networkView.RPC("plyrdy", RPCMode.AllBuffered, Network.player);
	}
	
	// FUncion que recibira el servidor y los clientes cuando un jugador pulse que esta listo
	[RPC]
	public void plyrdy(NetworkPlayer player)
	{
		if(!partidaEmpezada)
		{
			for(int i=0; i<listaJugadores.Length; i++)
			{
				if(listaJugadores[i].networkPlayer == player)
				{
					listaJugadores[i].isReady = true;
					break;
				}
			}
			
			if(Network.isServer && !personajesGenerados)
			{
				comprobarTodosListos();
			}
		}
		// Si un jugador se conecta a mitad de la partida el servidor le dira que cargue la pantalla de juego
		else if(Network.isServer)
		{
			networkView.RPC("LdGame", player);
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
		StartCoroutine(CargarPantallaJuegoDelayed(10));
	}
	
	// Genera personajes aleatorios y los comunica a los clientes
	IEnumerator corutinaGenerarPersonajesAleatorios()
	{
		personajesGenerados = true;
		
		yield return new WaitForSeconds(.5f);

		int indiceHumano = -1;
		List<int> listaJugadoresActivos = new List<int>();

		// Asignamos el humano a un jugador aleatorio y lo comunicamos al resto de clientes
		for(int i=0; i<listaJugadores.Length; i++)
		{
			if(listaJugadores[i].activePlayer)
			{
				listaJugadoresActivos.Add(i);
			}
		}
		indiceHumano = listaJugadoresActivos[UnityEngine.Random.Range(0, listaJugadoresActivos.Count)];

		//indiceHumano = 1;

		listaJugadores[indiceHumano].enumPersonaje = EnumPersonaje.Humano;
		listaJugadores[indiceHumano].viewID = Network.AllocateViewID();
		networkView.RPC("bcstChar", RPCMode.OthersBuffered, indiceHumano, (int)listaJugadores[indiceHumano].enumPersonaje, listaJugadores[indiceHumano].viewID);
		
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
				int randomCharacterIndex = UnityEngine.Random.Range(0, listaPersonajes.Count);
//				listaJugadores[i].enumPersonaje = EnumPersonaje.RobotAzul;
				listaJugadores[i].enumPersonaje = listaPersonajes[randomCharacterIndex];
				listaJugadores[i].viewID = Network.AllocateViewID();
				networkView.RPC("bcstChar", RPCMode.OthersBuffered, i, (int)listaJugadores[i].enumPersonaje, listaJugadores[i].viewID);
				listaPersonajes.RemoveAt(randomCharacterIndex);
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		//		GameManager.gameManager.SpawnearPersonajes();
	}
	
	public IEnumerator CargarPantallaJuegoDelayed(int waitTime)
	{
		networkView.RPC("strtCount", RPCMode.All, waitTime);
		yield return new WaitForSeconds(waitTime + 1);

		partidaEmpezada = true;
		networkView.RPC("LdGame", RPCMode.All);
	}
	
	// FUncion que recibiran los clientes para comunicar el personaje que le toca a cada uno
	[RPC]
	void bcstChar(int indiceJugador, int enumPersonajeEntero, NetworkViewID networkViewID)
	{
		listaJugadores[indiceJugador].enumPersonaje = (EnumPersonaje) enumPersonajeEntero;
		listaJugadores[indiceJugador].viewID = networkViewID;
	}
	
	// Funcion que recibiran los clientes para empezar la cuenta atras
	[RPC]
	void strtCount(int segundos)
	{
		LobbyManager.lobbyManager.StartCountDown(segundos);
	}
	
	[RPC]
	void LdGame()
	{
		Application.LoadLevel("GameScene");
	}

	// Funcion que se invocara cada cliente al cargarse la escena de juego
	public void NotificarPartidaCargada()
	{
		if(Network.isClient)
		{
			networkView.RPC("LdedGame", RPCMode.Server, Network.player);
		}
		else
		{
			ctrl (0);
		}
	}

	// Funcion que recibe el servidor cuando un cliente carga la partida
	[RPC]
	public void LdedGame(NetworkPlayer networkPlayer)
	{
		for(int i=0; i<listaJugadores.Length; i++)
		{
			networkView.RPC("spwn", networkPlayer, i, listaJugadores[i].viewID, (int)listaJugadores[i].enumPersonaje);

			// Si los objetos recogibles no son los iniciales los actualizamos...
			if(GameManager.gameManager.piezasRestantes != GameManager.gameManager.listaDeTuercas.Length-1)
			{
				networkView.RPC("updtRcg", networkPlayer, GameManager.gameManager.getTuercasEmpaquetadas());
			}

			// Si el personaje pertenece al jugador se le notificara para que le anyada control local
			if(networkPlayer == listaJugadores[i].networkPlayer)
			{
				networkView.RPC("ctrl", networkPlayer, i);
			}
		}

		networkView.RPC("sthumlif", networkPlayer, (int) humanLifes);
	}

	// Funcion que recibe un cliente para spawnear 
	[RPC]
	void spwn(int playerIndex, NetworkViewID viewID, int enumPersonaje)
	{
		listaJugadores[playerIndex].player = PlayerFactory.playerFactoryRef.InstanciarPlayerEnCliente(
													viewID,
													enumPersonaje);
		listaJugadores[playerIndex].player.id = playerIndex;
	}

	// Funcion que recibe un cliente para notificar que debe tomar el control 
	[RPC]
	void ctrl(int playerIndex)
	{
		listaJugadores[playerIndex].player.gameObject.AddComponent<LocalInput>();
		listaJugadores[playerIndex].player.localInput = listaJugadores[playerIndex].player.gameObject.GetComponent<LocalInput>();

		/*if(listaJugadores[playerIndex].enumPersonaje == EnumPersonaje.Humano)
		{
			listaJugadores[playerIndex].player.RemoveFOW();
		}

		listaJugadores[playerIndex].ownByClient = true;*/
	}

	// Funcion que envia a los jugadores la informacion del servidor 
	[RPC]
	void stsrvr(NetworkPlayer networkPlayerServer)
	{
		this.networkPlayerServer = networkPlayerServer;
	}

	public void NotificarJugadorMatado (int killed, int killer)
	{
		networkView.RPC("pklld", RPCMode.Others, killed, killer);
		networkView.RPC("incrScore", RPCMode.OthersBuffered, killed, killer);

	}

	// Funcion que envia a los jugadores la informacion del servidor 
	[RPC]
	void incrScore(int killed, int killer)
	{
		++NetworkManager.networkManagerRef.listaJugadores[killed].deaths;
		++NetworkManager.networkManagerRef.listaJugadores[killer].kills;
	}

	// Funcion que envia a los jugadores la informacion del servidor 
	[RPC]
	void pklld(int killed, int killer)
	{
		GameManager.gameManager.KillPlayerClient(killed, killer);
	}

	public void syncHumanLifes()
	{
		networkView.RPC("sthumlif", RPCMode.Others, (int)humanLifes);
	}
	
	//RPC para sincronizar las vidas del humano con todos los clientes
	[RPC]
	void sthumlif(int number)
	{
		humanLifes = (short)number;
	}


	public void TerminarPartida (bool humanWin, float time, int piezasRestantes, int piezasRecogidas)
	{
		if(!humanWin)
		{
			EndMtch(0, time, piezasRestantes, piezasRecogidas);
			networkView.RPC("EndMtch", RPCMode.OthersBuffered, 0, time, piezasRestantes, piezasRecogidas);
		}
		else
		{
			EndMtch(1, time, piezasRestantes, piezasRecogidas);
			networkView.RPC("EndMtch", RPCMode.OthersBuffered, 1, time, piezasRestantes, piezasRecogidas);
		}
	}
	
	//RPC para terminar la partida
	[RPC]
	void EndMtch(int humanWin, float time, int piezasRestantes, int piezasRecogidas)
	{
		if(humanWin != 0) this.humanWin = true;

		this.piezasRecogidas = (short)piezasRecogidas;
		this.piezasRestantes = (short)piezasRestantes;
		tiempoPartida = time;
		Application.LoadLevel("ScoreScene");
	}

	public void BroadcastSkill (int playerID)
	{
		networkView.RPC ("bcstSkll", RPCMode.Others, playerID); 
	}

	//RPC para terminar la partida
	[RPC]
	void bcstSkll(int id)
	{
		listaJugadores[id].player.ActivatePower();
	}
	
	[RPC]
	void updtRcg(int paqueteRecogibles)
	{
		if(GameManager.gameManager)
		{
			GameManager.gameManager.actualizarTuercasDesdePaquete(paqueteRecogibles);
		}
	}

	public void SpamearRecogibles()
	{
		int recogiblesEmpaquetados = GameManager.gameManager.getTuercasEmpaquetadas();

		networkView.RPC ("updtRcg", RPCMode.Others, recogiblesEmpaquetados); 
	}

	public void BroadcastIndicador (Vector3 posicion, EnumTipoInidcador indicadorAPoner, int indiceJugador)
	{
		networkView.RPC ("bcstInd", RPCMode.Others, Mathf.RoundToInt(posicion.x), Mathf.RoundToInt(posicion.y), (int)indicadorAPoner, indiceJugador); 
	}

	[RPC]
	void bcstInd(int posicionX, int posicionY, int indicadorAPoner, int indiceJugador)
	{
		if(WarFog.warfogEnabled)
		{
			listaJugadores[indiceJugador].player.ponerIndicadorEn(new Vector2(posicionX, posicionY), (EnumTipoInidcador)indicadorAPoner);
		}
	}

	public void LaunchTutorial()
	{
		Application.LoadLevel("TutorialLobbyScene");
		listaJugadores[0].activePlayer = true;
		listaJugadores[0].networkPlayer = Network.player;
		listaJugadores[0].isReady = false;
		listaJugadores[0].index = 0;
		listaJugadores[0].playerName = nombreJugador;

		NetworkManager.networkManagerRef.listaJugadores[0].ownByClient = true;
	}

	public void PlayerReadyTutorial()
	{
		listaJugadores[0].isReady = true;
		listaJugadores[0].enumPersonaje = EnumPersonaje.Humano;
		StartCoroutine(CargarTutorialDelayed(5));
		//listaJugadores[0].viewID = Network.AllocateViewID();
		//networkView.RPC("bcstChar", RPCMode.OthersBuffered, indiceHumano, (int)listaJugadores[indiceHumano].enumPersonaje, listaJugadores[indiceHumano].viewID);
	}

	public IEnumerator CargarTutorialDelayed(int waitTime)
	{
		LobbyManager.lobbyManager.StartCountDownTutorial(waitTime);
		partidaEmpezada = true;
		yield return new WaitForSeconds(waitTime+1);

		Application.LoadLevel("TutorialGameScene0");
	}
}