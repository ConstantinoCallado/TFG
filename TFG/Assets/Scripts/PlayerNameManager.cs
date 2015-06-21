using UnityEngine;
using System.Collections;

public class PlayerNameManager : MonoBehaviour 
{
	public GameObject panelPreguntarNombre;
	public NetworkManager networkManager;

	// Use this for initialization
	void Awake () 
	{
		if(!PlayerPrefs.HasKey("PlayerName"))
		{
			panelPreguntarNombre.SetActive(true);
		}
		else
		{
			networkManager.nombreJugador = PlayerPrefs.GetString("PlayerName");
		}
	}
	

	public void SetPlayerName(string name)
	{
		if(PlayerPrefs.HasKey("PlayerName"))
		{
			PlayerPrefs.DeleteKey("PlayerName");
		}

		PlayerPrefs.SetString("PlayerName", name);
		networkManager.nombreJugador = name;
	}
}
