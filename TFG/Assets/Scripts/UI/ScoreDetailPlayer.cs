using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDetailPlayer : MonoBehaviour 
{
	public Image avatarPersonaje;
	public Text textoNombreYClase;
	public Text textoAsesinatos;
	public Text textoMuertes;
	public GameObject panelControlado;

	public void SetDetail(EnumPersonaje enumPersonaje, string nombre, int asesinatos, int muertes, bool controlado)
	{
		avatarPersonaje.sprite = AvatarManager.avatarManager.getAvatar(enumPersonaje);

		textoNombreYClase.text = nombre + "\n" + AvatarManager.avatarManager.getPlayerClassName(enumPersonaje);
		textoAsesinatos.text = asesinatos.ToString();
		textoMuertes.text = muertes.ToString();

		panelControlado.SetActive(controlado);
	}
}
