using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerUIElement : MonoBehaviour 
{
	public Text textPlayerName;
	public Text textCharacterName;
	public Image imageCharacter;
	public Text textoInterrogacion;
	public Image panelNombre;
	public Image panelAvatar;
	//public GameObject panelAvatar;

	private bool imageSetted = false;

	public Animation animationCard;
	public static float lastTimeFlipped;

	private bool readySet = true;

	public void setPlayerName(string playerName, EnumPersonaje enumPersonaje, string nombrePersonaje, bool isReady)
	{
		textPlayerName.text = playerName;

		if(readySet != isReady)
		{
			readySet = isReady;

			if(isReady)
			{
				panelNombre.color = Color.white;
				panelAvatar.color = Color.white;
			}
			else
			{
				Debug.Log("ASD");
				panelNombre.color = new Color(0.3f, 0.3f, 0.3f, 1);
				panelAvatar.color = panelNombre.color;
			}
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
