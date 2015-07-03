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
	public GameObject panelAvatar;

	private bool imageSetted = false;

	public Animation animationCard;
	public static float lastTimeFlipped;

	public void setPlayerName(string playerName, EnumPersonaje enumPersonaje, string nombrePersonaje, bool isReady)
	{
		textPlayerName.text = playerName;

		if(panelReady.enabled != isReady)
		{
			panelReady.enabled = isReady;
		}

		if(!imageSetted && enumPersonaje != EnumPersonaje.Ninguno && Time.time >= lastTimeFlipped)
		{
			lastTimeFlipped = Time.time + 0.25f;
			imageSetted = true;
			animationCard.Play();
			StartCoroutine(corutinaPonerImagen(enumPersonaje, nombrePersonaje));
			//StartCoroutine(coritinaGirarCarta(enumPersonaje, nombrePersonaje));
		}
	}

	public IEnumerator corutinaPonerImagen(EnumPersonaje enumPersonaje, string nombrePersonaje)
	{
		yield return new WaitForSeconds(animationCard.clip.length/2);

		imageCharacter.sprite = AvatarManager.avatarManager.getAvatar(enumPersonaje);
		imageCharacter.enabled = true;
		textoInterrogacion.enabled = false;
		textCharacterName.text = nombrePersonaje;
	}
}
