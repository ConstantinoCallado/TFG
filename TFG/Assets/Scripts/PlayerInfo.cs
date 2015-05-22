using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo 
{
	public string playerName = "IA";
	public string characterName = "";
	public bool activePlayer = false;
	public bool isReady = true;

	public NetworkPlayer networkPlayer;

	public void resetPlayer()
	{
		playerName = "IA";
		activePlayer = false;
		isReady = true;
		characterName = "";
	}
}
