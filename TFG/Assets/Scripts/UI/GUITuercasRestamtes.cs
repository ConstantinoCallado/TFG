using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUITuercasRestamtes : MonoBehaviour 
{
	public Text textoTuercas;
	public short cantidadActual;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(corutinaActualizarVidas());
	}

	IEnumerator corutinaActualizarVidas()
	{
		while(true)
		{
			if(cantidadActual != GameManager.gameManager.piezasRestantes)
			{
				cantidadActual = GameManager.gameManager.piezasRestantes;
				textoTuercas.text = "x" + cantidadActual;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}
