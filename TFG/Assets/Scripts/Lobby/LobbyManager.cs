using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LobbyManager : MonoBehaviour 
{
	public List<LobbyPlayerUIElement> listaInfoJugadores = new List<LobbyPlayerUIElement>();

	public void Start()
	{
		StartCoroutine(corutinaMostrarJugadores());
	}

	public IEnumerator corutinaMostrarJugadores()
	{
		while(true)
		{
			for(int i=0; i< NetworkManager.networkManagerRef.listaJugadores.Length && i < listaInfoJugadores.Count; i++)
			{
				listaInfoJugadores[i].setPlayerName(NetworkManager.networkManagerRef.listaJugadores[i].playerName,
				                                    NetworkManager.networkManagerRef.listaJugadores[i].characterName);
			}

			yield return new WaitForSeconds(1f);
		}
	}
}
