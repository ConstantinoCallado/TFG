using UnityEngine;
using System.Collections;

public class Human : Player 
{
	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("inicializado Humano");
	}
	
	public override void Kill()
	{
		Debug.Log("Matando Humano");

	}
}
