using UnityEngine;
using System.Collections;

public class OrangeRobot : Robot
{
	Color colorRobot = new Color(0.97f, 0.61f, 0f);

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot naranja");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder naranja");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
}
