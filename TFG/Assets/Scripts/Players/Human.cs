using UnityEngine;
using System.Collections;

public class Human : Player 
{
	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Human");
		gameObject.tag = "Human";
		speed = 3.5f;
		base.playerGraphics.setHuman();
		gameObject.name = "Human";
		Debug.Log("inicializado Humano");
	}
	
	public override void Kill()
	{
		Debug.Log("Matando Humano");

		SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());
		Respawn();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Robot")
		{
			Debug.Log("He tocado un robot");
			Kill ();
		}
	}
}
