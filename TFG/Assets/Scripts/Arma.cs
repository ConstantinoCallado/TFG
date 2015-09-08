﻿using UnityEngine;
using System.Collections;

public class Arma : MonoBehaviour 
{
	public static short cantidadInstanciada = 0;
	
	public short id;


	public void Awake()
	{
		id = cantidadInstanciada;
		++cantidadInstanciada;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Human")
		{
			AudioManager.audioManagerRef.PlayPocion();
		}
	}
}
