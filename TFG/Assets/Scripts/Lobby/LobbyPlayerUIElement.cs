using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerUIElement : MonoBehaviour 
{
	public Text textPlayerName;
	public Text textCharacterName;
	public Image imageCharacter;
	public Image panelReady;

	public void setPlayerName(string playerName, string characterName, bool isReady)
	{
		textPlayerName.text = playerName;
		textCharacterName.text = characterName;

		if(isReady)
		{
			panelReady.enabled = true;
		}
		else
		{
			panelReady.enabled = false;
		}

		if(characterName != "")
		{
			//TODO: PONER IMAGEN
		}
	}
}
