using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NameManager : MonoBehaviour 
{
	public Canvas canvas;
	public GameObject textPlayerPrefab;
	public List<Text> listaTextos;
	public static NameManager nameManagerRef;

	void Start()
	{
		nameManagerRef = this;

		// Creamos los 5 textos que asignaremos a cada jugador
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			GameObject textoInstanciado = GameObject.Instantiate(textPlayerPrefab);

			textoInstanciado.transform.parent = canvas.transform;
			textoInstanciado.transform.localScale = Vector3.one;
			listaTextos.Add(textoInstanciado.GetComponent<Text>());
		}

		HoveringName.canvasRect = canvas.GetComponent<RectTransform>();
	}
}
