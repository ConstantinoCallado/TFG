using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager gameManager;
	//public NetworkView networkView;
	public PlayerFactory playerFactory;
	public const int timeForStartRound = 3;
	public bool roundStarted = false;
	public int humanTries = 3;

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
			
			/*	networkView.RPC("instPlyr", RPCMode.OthersBuffered, viewID,
			                (int)NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje);*/
		}
	}

	public void NotificarPartidaCargada()
	{
		NetworkManager.networkManagerRef.NotificarPartidaCargada();
	}

	public void KillHumanServer()
	{
		NetworkManager.networkManagerRef.NotificarHumanoMatado();
		--humanTries;
		StartCoroutine(coroutineKillHumanServer());
	}

	public IEnumerator coroutineKillHumanServer()
	{
		yield return new WaitForSeconds(2);

		// Recorremos todos los jugadores y los respawneamos
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano)
			{
				NetworkManager.networkManagerRef.listaJugadores[i].player.Respawn();
			}
			else
			{
				NetworkManager.networkManagerRef.listaJugadores[i].player.SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());
				NetworkManager.networkManagerRef.listaJugadores[i].player.Respawn();
			}
		}
	}
	
	public void KillHumanClient()
	{	
		--humanTries;
		StartCoroutine(coroutineKillHumanClient());
	}

	public IEnumerator coroutineKillHumanClient()
	{
		// Recorremos todos los jugadores y los respawneamos
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje == EnumPersonaje.Humano)
			{
				NetworkManager.networkManagerRef.listaJugadores[i].player.Kill();
			}
		}

		yield return new WaitForSeconds(2);

		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			NetworkManager.networkManagerRef.listaJugadores[i].player.Respawn();
		}
	}
}
