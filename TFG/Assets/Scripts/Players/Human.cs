using UnityEngine;
using System.Collections;

public class Human : Player 
{
	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Human");
		speed = 3.75f;
		base.playerGraphics.setHuman();
		gameObject.name = "Human";
		Debug.Log("inicializado Humano");
	}
	
	public override void Kill()
	{
		Debug.Log("Matando Humano");

	}
}
