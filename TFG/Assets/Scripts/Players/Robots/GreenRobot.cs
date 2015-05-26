using UnityEngine;
using System.Collections;

public class GreenRobot : Robot
{
	Color colorRobot = new Color(0.29f, 0.97f, 0.41f);

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot verde");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder verde");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
}
