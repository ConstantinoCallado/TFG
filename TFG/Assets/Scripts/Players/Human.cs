using UnityEngine;
using System.Collections;

public class Human : Player 
{
	const float speed = 3.75f;

	public override void Initialize()
	{
		base.Initialize();

		base.basicMovementRef.speed = speed;

		Debug.Log("inicializado Humano");
	}
	
	public override void Kill()
	{
		Debug.Log("Matando Humano");

	}
}
