using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager gameManager;
	//public NetworkView networkView;
	public PlayerFactory playerFactory;
	public const int timeForStartRound = 3;
	public bool roundStarted = false;
	public short humanTries = 3;
	public short robotsAlive = 0;
	public const short timeRespawnRobot = 15;

	void Awake () 
	{
		gameManager = this;
		
		if(Network.isServer)
		{
			SpawnearPersonajesEnServer();
		}
		NotificarPartidaCargada();
	}
	
	public void SpawnearPersonajesEnServer()
	{
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			// Instanciamos el personaje y lo asignamos al jugador
			NetworkManager.networkManagerRef.listaJugadores[i].player = playerFactory.InstanciarPlayerEnServidor(NetworkManager.networkManagerRef.listaJugadores[i].viewID,
			                                                                   (int)NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje);

			NetworkManager.networkManagerRef.listaJugadores[i].player.id = i;
			/*	networkView.RPC("instPlyr", RPCMode.OthersBuffered, viewID,
			                (int)NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje);*/
		}
	}

	public void NotificarPartidaCargada()
	{
		NetworkManager.networkManagerRef.NotificarPartidaCargada();
	}

	public void KillPlayerServer(int index)
	{
		NetworkManager.networkManagerRef.NotificarJugadorMatado(index);
		StartCoroutine(coroutineKillPlayerServer(index));
	}

	public IEnumerator coroutineKillPlayerServer(int index)
	{
		// Si es el humano el que muere... tras 2 segundos respawneamos a todos
		if(NetworkManager.networkManagerRef.listaJugadores[index].enumPersonaje == EnumPersonaje.Humano)
		{
			//GUIHumanLifes.GUIHumanLifesRef.RemoveLife();
			--NetworkManager.networkManagerRef.humanLifes;
			NetworkManager.networkManagerRef.syncHumanLifes();

			yield return new WaitForSeconds(2);

			NetworkManager.networkManagerRef.listaJugadores[index].player.SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());

			// Recorremos todos los jugadores y los respawneamos
			for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
			{
				NetworkManager.networkManagerRef.listaJugadores[i].player.Respawn();
			}

			robotsAlive = (short)(NetworkManager.networkManagerRef.listaJugadores.Length - 1);
		}
		else
		{
			yield return new WaitForSeconds(timeRespawnRobot);

			if(NetworkManager.networkManagerRef.listaJugadores[index].player.isDead)
			{
				NetworkManager.networkManagerRef.listaJugadores[index].player.Respawn();
			}
		}
	}
	
	public void KillPlayerClient(int index)
	{	
		StartCoroutine(coroutineKillPlayerClient(index));
	}

	public IEnumerator coroutineKillPlayerClient(int index)
	{
		// Si es el humano el que muere... tras 2 segundos respawneamos a todos
		if(NetworkManager.networkManagerRef.listaJugadores[index].enumPersonaje == EnumPersonaje.Humano)
		{
			NetworkManager.networkManagerRef.listaJugadores[index].player.Kill();
			//GUIHumanLifes.GUIHumanLifesRef.RemoveLife();
			//--humanTries;

			yield return new WaitForSeconds(2);

			// Recorremos todos los jugadores y los respawneamos
			for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
			{
				NetworkManager.networkManagerRef.listaJugadores[i].player.Respawn();
			}
		}
		else
		{
			NetworkManager.networkManagerRef.listaJugadores[index].player.Kill();

			yield return new WaitForSeconds(timeRespawnRobot);

			NetworkManager.networkManagerRef.listaJugadores[index].player.Respawn();
		}
	}
}
