using UnityEngine;
using System.Collections;

public class Human : Player 
{
	const float speed = 3.75f;

	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Human");
		base.basicMovementRef.speed = speed;
		base.playerGraphics.setHuman();

		Debug.Log("inicializado Humano");
	}
	
	public override void Kill()
	{
		Debug.Log("Matando Humano");

	}
}
