using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIHumanLifes : MonoBehaviour 
{
	public Text textoVidas;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(corutinaActualizarVidas());
	}

	IEnumerator corutinaActualizarVidas()
	{
		while(true)
		{
			textoVidas.text = "x" + NetworkManager.networkManagerRef.humanLifes;

			yield return new WaitForSeconds(0.25f);
		}
	}
}
