using UnityEngine;
using System.Collections;

public class Human : Player 
{
	public bool aggressiveMode = false;

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

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Robot")
		{
			Debug.Log("He tocado un robot");

			if(!aggressiveMode)
			{
				if(!isDead)
				{
					base.Kill();

					GameManager.gameManager.KillPlayerServer(base.id);
				}
			}
		}
	}
}
