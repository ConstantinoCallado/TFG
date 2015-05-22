using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerUIElement : MonoBehaviour 
{
	public Text textPlayerName;
	public Text textCharacterName;
	public Image imageCharacter;


	public void setPlayerName(string playerName, string characterName)
	{
		textPlayerName.text = playerName;
		textCharacterName.text = characterName;

		if(characterName != "")
		{
			//TODO: PONER IMAGEN
		}
	}
}
