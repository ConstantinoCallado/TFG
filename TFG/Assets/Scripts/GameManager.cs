//#define vidasInfinitas

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager gameManager;
	public PlayerFactory playerFactory;
	public const int timeForStartRound = 3;
	public bool roundStarted = false;
	public short robotsAlive = 0;
	public const short timeRespawnRobot = 10;
	public short piezasRestantes = 0;
	public short piezasRecogidas = 0;
	public float tiempoInicial;

	void Awake () 
	{
		gameManager = this;
	
		if(Network.isServer)
		{
			tiempoInicial = Time.time;
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

	public void KillPlayerServer(int killed, int killer)
	{
		++NetworkManager.networkManagerRef.listaJugadores[killed].deaths;
		++NetworkManager.networkManagerRef.listaJugadores[killer].kills;

		NetworkManager.networkManagerRef.NotificarJugadorMatado(killed, killer);
		StartCoroutine(coroutineKillPlayerServer(killed));
	}

	public IEnumerator coroutineKillPlayerServer(int index)
	{

		// Si es el humano el que muere... tras 2 segundos respawneamos a todos
		if(NetworkManager.networkManagerRef.listaJugadores[index].enumPersonaje == EnumPersonaje.Humano)
		{
			//GUIHumanLifes.GUIHumanLifesRef.RemoveLife();
			--NetworkManager.networkManagerRef.humanLifes;

			#if !vidasInfinitas
			if(NetworkManager.networkManagerRef.humanLifes == 0)
			{
				NetworkManager.networkManagerRef.TerminarPartida(false, Time.time - 	tiempoInicial, piezasRestantes, piezasRecogidas);
			}
			#endif
			
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


	public void KillPlayerClient(int killed, int killer)
	{	
		StartCoroutine(coroutineKillPlayerClient(killed));
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

	public void RecogerPieza ()
	{
		++piezasRecogidas;
		--piezasRestantes;
		if(piezasRestantes == 0)
		{
			NetworkManager.networkManagerRef.TerminarPartida(true, Time.time - tiempoInicial, piezasRestantes, piezasRecogidas);
		}
	}
}
