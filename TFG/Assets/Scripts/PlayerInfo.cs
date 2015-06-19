﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo 
{
	public string playerName = "IA";
	public bool activePlayer = false;
	public bool isReady = true;
	public Player player;
	public EnumPersonaje enumPersonaje = EnumPersonaje.Ninguno;
	public NetworkPlayer networkPlayer;
	public NetworkViewID viewID;
	public int index;

	public void resetPlayer()
	{
		playerName = "IA";
		activePlayer = false;
		isReady = true;
	}

	public void playerDisconnected ()
	{
		playerName = "IA";
		activePlayer = false;
		isReady = true;

		if(player!= null)
		{
			player.EnableIA(true);
		}
	}
}
