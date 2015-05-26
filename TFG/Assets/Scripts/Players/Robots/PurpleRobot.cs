using UnityEngine;
using System.Collections;

public class PurpleRobot : Robot
{
	Color colorRobot = new Color(0.91f, 0.19f, 1f);

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot morado");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder morado");
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
	
	public override string getColorString()
	{
		return "purple";
	}
}
