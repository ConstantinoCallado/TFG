﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerUIElement : MonoBehaviour 
{
	public Text textPlayerName;
	public Text textCharacterName;
	public Image imageCharacter;
	public Text textoInterrogacion;
	public Image panelReady;
	public GameObject panelAvatar;

	private bool imageSetted = false;


	public void setPlayerName(string playerName, EnumPersonaje enumPersonaje, bool isReady)
	{
		textPlayerName.text = playerName;
		textCharacterName.text = enumPersonaje.ToString();

		panelReady.enabled = isReady;

		if(enumPersonaje != EnumPersonaje.Ninguno && !imageSetted)
		{
			imageSetted = true;
			StartCoroutine(coritinaGirarCarta(enumPersonaje));
		}
	}

	public IEnumerator coritinaGirarCarta(EnumPersonaje enumPersonaje)
	{
		float gradosGirados = 0;
		float gradosAGirar = 0;

		while(gradosGirados < 90)
		{
			gradosAGirar = Mathf.Min((90 - gradosGirados), 250 * Time.deltaTime);
			panelAvatar.transform.Rotate(new Vector3(0, gradosAGirar, 0));
			gradosGirados += gradosAGirar;

			yield return new WaitForEndOfFrame();
		}

		imageCharacter.sprite = AvatarManager.avatarManager.getAvatar(enumPersonaje);
		imageCharacter.enabled = true;
		textoInterrogacion.enabled = false;

		while(gradosGirados < 180)
		{
			gradosAGirar = Mathf.Min((180 - gradosGirados), 250 * Time.deltaTime);
			panelAvatar.transform.Rotate(new Vector3(0, gradosAGirar, 0));
			gradosGirados += gradosAGirar;

			yield return new WaitForEndOfFrame();
		}
	}
}
