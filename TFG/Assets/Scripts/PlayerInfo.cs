using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo 
{
	public string playerName = "Bot";
	public string characterName = "";
	public bool activePlayer = false;

	public NetworkPlayer networkPlayer;

	public void resetPlayer()
	{
		playerName = "Bot";
		activePlayer = false;
		characterName = "";
	}
}
