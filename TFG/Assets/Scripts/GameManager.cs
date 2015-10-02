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
	public const short timeRespawnRobot = 15;
	public short piezasRestantes = 0;
	public float tiempoInicial;

	public float ultimaVezRecogidoAlgo;
	
	public Tuerca[] listaDeTuercas = new Tuerca[28];

	void Awake () 
	{
		gameManager = this;
		Tuerca.cantidadInstanciada = 0;
		piezasRestantes = (short)listaDeTuercas.Length;

		if(!Network.isClient)
		{
			tiempoInicial = Time.time;
			SpawnearPersonajesEnServer();
		}

		NotificarPartidaCargada();
	}

	public void Start()
	{
		//Debug.Log("Empezando con " + getTuercasEmpaquetadas());
	}

	public int getTuercasEmpaquetadas()
	{
		int resultado = 0;

		for(int i=0; i<listaDeTuercas.Length; i++)
		{
			if(listaDeTuercas[i]!=null && !listaDeTuercas[i].recogida)
			{
				resultado++;
			}

			if(i < listaDeTuercas.Length-1)
			{
				resultado = resultado << 1;
			}
		}

		return resultado;
	}

	public void actualizarTuercasDesdePaquete(int paquete)
	{
		piezasRestantes = (short)listaDeTuercas.Length;

		for(int i = listaDeTuercas.Length-1; i>= 0; i--)
		{
			// Comprobamos el ultimo bit del paquete para comprobar si la tuerca esta cogida o no...
			if((paquete & 1) == 1)
			{
				if(listaDeTuercas[i]!=null && listaDeTuercas[i].recogida)
				{
					listaDeTuercas[i].SetCogida(false);
				}
			}
			else
			{
				--piezasRestantes;
				if(listaDeTuercas[i] && !listaDeTuercas[i].recogida)
				{
					listaDeTuercas[i].SetCogida(true);
				}
			}

			paquete = paquete >> 1;
		}
	}

	public void SpawnearPersonajesEnServer()
	{
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje == EnumPersonaje.Ninguno)
			{
				break;
			}

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
				NetworkManager.networkManagerRef.TerminarPartida(false, Time.time - tiempoInicial, piezasRestantes, listaDeTuercas.Length - piezasRestantes);
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
			AudioManager.audioManagerRef.PlayMuerteHumano();

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
			AudioManager.audioManagerRef.PlayMuerteRobot();

			yield return new WaitForSeconds(timeRespawnRobot);

			NetworkManager.networkManagerRef.listaJugadores[index].player.Respawn();
		}
	}

	public void RecogerTuerca(short index)
	{
		listaDeTuercas[index].SetCogida(true);

		--piezasRestantes;

		if(Network.isServer)
		{
			// Cada 10 segundos actualizamos los recogibles en todoos los dispositivos
			if(Time.time > ultimaVezRecogidoAlgo + 10)
			{
				ultimaVezRecogidoAlgo = Time.time;

				NetworkManager.networkManagerRef.SpamearRecogibles();
			}

			if(piezasRestantes == 0)
			{
				NetworkManager.networkManagerRef.TerminarPartida(true, Time.time - tiempoInicial, piezasRestantes, listaDeTuercas.Length - piezasRestantes);
			}
		}
	}


	public void RecogerPieza ()
	{
		--piezasRestantes;
		if(piezasRestantes == 0)
		{
			NetworkManager.networkManagerRef.TerminarPartida(true, Time.time - tiempoInicial, piezasRestantes, listaDeTuercas.Length - piezasRestantes);
		}
	}
}
