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

		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			GameObject textoInstanciado = GameObject.Instantiate(textPlayerPrefab);

			textoInstanciado.transform.parent = canvas.transform;
			textoInstanciado.transform.localScale = Vector3.one;
			listaTextos.Add(textoInstanciado.GetComponent<Text>());

			//TODO: Esto en el cliente no funciona
			NetworkManager.networkManagerRef.listaJugadores[i].player.playerGraphics.hoveringName.textUI = listaTextos[i];
			NetworkManager.networkManagerRef.listaJugadores[i].player.playerGraphics.hoveringName.active = true;
			NetworkManager.networkManagerRef.listaJugadores[i].player.playerGraphics.hoveringName.index = i;
		}

		HoveringName.canvasRect = canvas.GetComponent<RectTransform>();
	}
}
