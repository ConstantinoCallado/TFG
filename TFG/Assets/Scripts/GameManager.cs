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
		else
		{
			NotificarPartidaCargada();
		}
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
}
