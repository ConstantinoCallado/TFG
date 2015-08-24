using UnityEngine;
using System.Collections;

public class WhiteRobot : Robot
{
	const float skillDuration = 5;
	public static Color colorRobot = new Color(0.9f, 0.9f, 0.9f);

	public bool hiding = false;

	public override void Initialize()
	{
		base.Initialize();

		//Debug.Log("Inicializando robot blanco");
	}
	
	public override void ActivatePower()
	{
		hiding = true;
		if(NetworkManager.networkManagerRef.listaJugadores[LocalInput.localInputRef.playerRef.id].enumPersonaje != EnumPersonaje.Humano)
		{
			playerGraphics.SetTransparent(0.5f);
		}
		else
		{
			playerGraphics.EnableGraphics(false);
			sightScript.EnableSight(false);
		}

		Invoke ("DesactivatePower", skillDuration);
		//Debug.Log("Activando poder blanco");

		playerGraphics.robotGraphics.particulasFlash.Emit(20);
	}

	public void DesactivatePower()
	{
		if(hiding)
		{
			hiding = false;
		
			if(NetworkManager.networkManagerRef.listaJugadores[LocalInput.localInputRef.playerRef.id].enumPersonaje != EnumPersonaje.Humano)
			{
				playerGraphics.SetTransparent(1);
			}
			else
			{
				playerGraphics.EnableGraphics(true);
				sightScript.EnableSight(true);
			}
			playerGraphics.robotGraphics.particulasFlash.Emit(20);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		DesactivatePower();
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
	
	public override string getColorString()
	{
		return "white";
	}
}
