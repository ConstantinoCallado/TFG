using UnityEngine;
using System.Collections;

public class RedRobot : Robot
{
	Color colorRobot = new Color(1, 0.3f, 0.3f);

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

	public override string getColorString()
	{
		return "red";
	}
}
