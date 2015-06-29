using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo 
{
	public string playerName = "Bot";
	public bool activePlayer = false;
	public bool isReady = true;
	public Player player;
	public EnumPersonaje enumPersonaje = EnumPersonaje.Ninguno;
	public NetworkPlayer networkPlayer;
	public NetworkViewID viewID;
	public int index;
	public short kills = 0;
	public short deaths = 0;
	public bool ownByClient = false;

	public void resetPlayer()
	{
		playerName = "Bot";
		activePlayer = false;
		isReady = true;
	}
}
