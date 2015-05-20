using UnityEngine;
using System.Collections;

public class BlueRobot : Robot
{
	Color colorRobot = new Color(0.16f, 0.58f, 1f);

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
