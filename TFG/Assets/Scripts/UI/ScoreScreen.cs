using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreScreen : MonoBehaviour 
{
	public List<ScoreDetailPlayer> listaDetallesJugadores = new List<ScoreDetailPlayer>();
	public GameObject textoVictoria;
	public GameObject textoDerrota;
	public Text textoTiempo;
	public Text textoMonedasRobadas;
	public Text textoMonedasRestantes;


	public void Start()
	{
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			listaDetallesJugadores[i].SetDetail(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje, 
			                                    NetworkManager.networkManagerRef.listaJugadores[i].playerName,
			                                    NetworkManager.networkManagerRef.listaJugadores[i].kills,
			                                    NetworkManager.networkManagerRef.listaJugadores[i].deaths,
			                                    NetworkManager.networkManagerRef.listaJugadores[i].ownByClient);

			if(NetworkManager.networkManagerRef.listaJugadores[i].ownByClient)
			{
				if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje == EnumPersonaje.Humano && NetworkManager.networkManagerRef.humanWin
				   || NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano && !NetworkManager.networkManagerRef.humanWin)
				{
					textoVictoria.SetActive(true);
					textoDerrota.SetActive(false);
				}
				else
				{
					textoVictoria.SetActive(false);
					textoDerrota.SetActive(true);
				}
			}
		}
		/*
		textoTiempo.text = ((int)(NetworkManager.networkManagerRef.tiempoPartida / 60)).ToString()
								+ ":" 
								+ ((int)(NetworkManager.networkManagerRef.tiempoPartida % 60)).ToString();
		*/


		TimeSpan timeSpan = TimeSpan.FromSeconds(NetworkManager.networkManagerRef.tiempoPartida);
		textoTiempo.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);


		textoMonedasRobadas.text = NetworkManager.networkManagerRef.piezasRecogidas.ToString();
		textoMonedasRestantes.text = NetworkManager.networkManagerRef.piezasRestantes.ToString();
	}

	public void BackToMenu()
	{
		Application.LoadLevel("MenuScene");
	}
}
