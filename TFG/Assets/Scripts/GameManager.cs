using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager gameManager;
	//public NetworkView networkView;
	public PlayerFactory playerFactory;

	void Awake () 
	{
		gameManager = this;

		if(Network.isServer)
		{
			SpawnearPersonajesEnServer();
		}
	}

	public void SpawnearPersonajesEnServer()
	{
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			// Reservamos una ID nueva para cada jugador
			NetworkViewID viewID = Network.AllocateViewID();

			// Instanciamos el personaje y lo asignamos al jugador
			NetworkManager.networkManagerRef.listaJugadores[i].player = playerFactory.InstanciarPlayerEnServidor(viewID,
			                                                                          (int)NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje);

		/*	networkView.RPC("instPlyr", RPCMode.OthersBuffered, viewID,
			                (int)NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje);*/
		}
	}

	/*
	// Funcion que recibiran los clientes para instanciar cada jugador
	[RPC]
	void instPlyr(NetworkViewID viewID, int enumPersonajeInt)
	{
		playerFactory.InstanciarPlayerEnCliente(viewID, enumPersonajeInt);
	}*/
}
