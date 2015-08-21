using UnityEngine;
using System.Collections;

public class Tuerca : MonoBehaviour 
{
	public static short cantidadInstanciada = 0;

	public short id;
	public bool recogida = false;
	public CircleCollider2D colliderTuerca;
	public Renderer graphics;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Human")
		{
			GameManager.gameManager.RecogerTuerca(id);
		}
	}

	public void Awake()
	{
		id = cantidadInstanciada;
		++cantidadInstanciada;
	}
	
	public void Start()
	{
		GameManager.gameManager.listaDeTuercas[id] = this;
		//TODO: Suscribirse a gameManager
	}

	public void SetCogida(bool param)
	{
		recogida = param;
		graphics.enabled = !param;
		colliderTuerca.enabled = !param;
	}
}
