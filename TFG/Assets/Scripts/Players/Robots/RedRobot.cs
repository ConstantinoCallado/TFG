using UnityEngine;
using System.Collections;

public class RedRobot : Robot
{
	public static Color colorRobot = new Color(1, 0.3f, 0.3f);
	const float increaseSpeed = 1.25f;
	const float skillDuration = 5;

	public TrailRenderer trailRenderer;

	public override void Initialize()
	{
		base.Initialize();
		playerGraphics.robotGraphics.gameObject.AddComponent<TrailRenderer>();
		trailRenderer = playerGraphics.robotGraphics.gameObject.GetComponent<TrailRenderer>();
		trailRenderer.enabled = false;
		trailRenderer.time = 0.75f;
		trailRenderer.startWidth = 0.4f;
		trailRenderer.endWidth = 0;
		trailRenderer.material = playerGraphics.robotGraphics.materialTrail;
		trailRenderer.material.color = playerGraphics.robotGraphics.renderersDeColores[0].material.color;
		//trailRenderer.material.color = playerGraphics.robotGraphics.renderersDeColores[0].material.color;
		Debug.Log("Inicializando robot rojo");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder rojo");
		base.speed += increaseSpeed;
		trailRenderer.enabled = true;
		Invoke("DesactivatePower", skillDuration);
	}
	
	public void DesactivatePower()
	{
		Debug.Log("Desactivando poder rojo");
		trailRenderer.enabled = false;
		base.speed -= increaseSpeed;
	}

	public override Color GetColor()
	{
		return colorRobot;
	}

	public override string getColorString()
	{
		return "red";
	}
}
