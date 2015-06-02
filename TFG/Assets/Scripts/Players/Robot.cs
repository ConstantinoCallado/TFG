using UnityEngine;
using System.Collections;

public class Robot : Player 
{
	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Robot");
		gameObject.tag = "Robot";
		base.playerGraphics.setRobot(GetColor());
		gameObject.name = "Robot " + getColorString();
	}

	public virtual void ActivatePower()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
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
		if(other.tag == "Human")
		{
			Debug.Log("He tocado al humano");
	
			if(other.gameObject.GetComponent<Human>().aggressiveMode)
			{
				GameManager.gameManager.KillPlayerServer(base.id);
				base.Kill();
			}
		}
	}
}
