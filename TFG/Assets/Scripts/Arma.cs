using UnityEngine;
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
}
