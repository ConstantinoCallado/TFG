using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerUIElement : MonoBehaviour 
{
	public Text textPlayerName;
	public Text textCharacterName;
	public Image imageCharacter;
	public Text textoInterrogacion;
	public Image panelReady;

	public void setPlayerName(string playerName, EnumPersonaje enumPersonaje, bool isReady)
	{
		textPlayerName.text = playerName;
		textCharacterName.text = enumPersonaje.ToString();

		panelReady.enabled = isReady;

		if(enumPersonaje != EnumPersonaje.Ninguno)
		{
			imageCharacter.sprite = AvatarManager.avatarManager.getAvatar(enumPersonaje);
			imageCharacter.enabled = true;
			textoInterrogacion.enabled = false;
		}
	}
}
