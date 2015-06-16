using UnityEngine;
using System.Collections;

public class Robot : Player 
{
	public Sight sightScript;

	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Robot");
		gameObject.tag = "Robot";
		base.playerGraphics.setRobot(GetColor());
		gameObject.name = "Robot " + getColorString();
		sightScript = gameObject.GetComponentInChildren<Sight>();

		sightScript.SetSight(2.5f);
	}

	public override void Kill()
	{
		base.Kill();
		sightScript.EnableSight(false);
	}

	public override void Respawn()
	{
		base.Respawn();
		sightScript.EnableSight(true);
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
