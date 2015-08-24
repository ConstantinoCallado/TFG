using UnityEngine;
using System.Collections;

public class PinkRobot : Robot
{
	public static Color colorRobot = new Color(1, 0.6f, 0.6f);

	public override void Initialize()
	{
		base.Initialize();

		//Debug.Log("Inicializando robot rosa");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder rosa");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}

	public override string getColorString()
	{
		return "pink";
	}
}
