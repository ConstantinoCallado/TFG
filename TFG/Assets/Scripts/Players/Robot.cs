using UnityEngine;
using System.Collections;

public class Robot : Player 
{
	private Sight sightScript;

	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Robot");
		gameObject.tag = "Robot";
		base.playerGraphics.setRobot(GetColor());
		gameObject.name = "Robot " + getColorString();
		sightScript = gameObject.GetComponent<Sight>();
	}

	public override void Kill()
	{
		base.Kill();
		sightScript.isEnabled = false;
	}

	public override void Respawn()
	{
		base.Respawn();
		sightScript.isEnabled = true;
	}

	public virtual Color GetColor()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
		return Color.black;
	}

	public virtual string getColorString()
	{
		return "";
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("He tocado al humano");

		if(other.gameObject.GetComponent<Human>().aggressiveMode)
		{
			GameManager.gameManager.KillPlayerServer(base.id);
			Kill();
		}
	}
}
