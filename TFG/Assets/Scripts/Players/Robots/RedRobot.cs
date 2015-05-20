using UnityEngine;
using System.Collections;

public class RedRobot : Robot
{
	Color colorRobot = new Color(25, 25, 25);

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot rojo");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder rojo");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
}
