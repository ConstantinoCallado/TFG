using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public void Start()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
	}

	public virtual void Kill()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}
}
