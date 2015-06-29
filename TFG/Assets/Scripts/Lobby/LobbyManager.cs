using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LobbyManager : MonoBehaviour 
{
	public static LobbyManager lobbyManager;
	public List<LobbyPlayerUIElement> listaInfoJugadores = new List<LobbyPlayerUIElement>();
	public Text textoMensaje;


	public void Awake()
	{
		lobbyManager = this;
	}

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
				                                    NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje,
				                                    AvatarManager.avatarManager.getPlayerClassName(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje),
				                                    NetworkManager.networkManagerRef.listaJugadores[i].isReady);
			}

			yield return new WaitForSeconds(.2f);
		}
	}

	public void SetPlayerReady()
	{
		NetworkManager.networkManagerRef.setPlayerReady(Network.player);
	}

	public void Atras()
	{
		Network.Disconnect();
	}

	public void StartCountDown(int nsegundos)
	{
		StartCoroutine(startCuentaAtras(nsegundos));
	}

	public IEnumerator startCuentaAtras(int segundos)
	{
		textoMensaje.text = "La partida empieza en " + segundos.ToString();
		do
		{
			yield return new WaitForSeconds(1);
			--segundos;
			textoMensaje.text = "La partida empieza en " + segundos.ToString();
		}while(segundos > 5);

		textoMensaje.fontSize = (int) (textoMensaje.fontSize * 1.75f);
		textoMensaje.text = segundos.ToString();

		do
		{
			yield return new WaitForSeconds(1);
			--segundos;
			textoMensaje.text = segundos.ToString();
		}while(segundos > 0);
	}


}
