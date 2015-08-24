using UnityEngine;
using System.Collections.Generic;


public class Tutorial : MonoBehaviour 
{
	public GameObject botonSiguiente;
	public GameObject botonAnterior;

	public List<GameObject> listaPanelesTutorial = new List<GameObject>();
	private short panelSeleccionado = 0;

	void OnEnable()
	{
		OcultarTodos();
		panelSeleccionado = 0;

		CargarPanel(panelSeleccionado);
	}

	void OcultarTodos()
	{
		for(int i=0; i<listaPanelesTutorial.Count; i++)
		{
			listaPanelesTutorial[i].SetActive(false);
		}
	}

	public void BotonSiguiente()
	{
		if(panelSeleccionado < listaPanelesTutorial.Count-1)
		{
			listaPanelesTutorial[panelSeleccionado].SetActive(false);
			++panelSeleccionado;
			CargarPanel(panelSeleccionado);
		}
	}

	public void BotonAnterior()
	{
		if(panelSeleccionado > 0)
		{
			listaPanelesTutorial[panelSeleccionado].SetActive(false);
			--panelSeleccionado;
			CargarPanel(panelSeleccionado);
		}
	}

	void CargarPanel(int numero)
	{
		listaPanelesTutorial[numero].SetActive(true);

		if(numero == listaPanelesTutorial.Count-1)
		{
			botonSiguiente.SetActive(false);
		}
		else
		{
			botonSiguiente.SetActive(true);
		}

		if(numero == 0)
		{
			botonAnterior.SetActive(false);
		}
		else
		{
			botonAnterior.SetActive(true);
		}
	}
}
