using UnityEngine;
using System.Collections;

public class WhiteRobot : Robot
{
	Color colorRobot = new Color(0.9f, 0.9f, 0.9f);

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot blanco");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder blanco");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
}
