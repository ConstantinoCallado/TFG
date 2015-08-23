using UnityEngine;
using System.Collections;

public class OrangeRobot : Robot
{
	public static Color colorRobot = new Color(0.97f, 0.61f, 0f);

	const float increaseRadius = 1.33f;
	const float skillDuration = 7;

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot naranja");
	}


	public override void ActivatePower()
	{
		Debug.Log("Activando poder naranja");
		sightScript.gameObject.transform.localScale = new Vector3(increaseRadius, increaseRadius, increaseRadius);
		Invoke("DesactivatePower", skillDuration);
	}
	
	public void DesactivatePower()
	{
		Debug.Log("Desactivando poder naranja");
		sightScript.gameObject.transform.localScale = Vector3.one;
	}


	public override Color GetColor()
	{
		return colorRobot;
	}
	
	public override string getColorString()
	{
		return "orange";
	}
}
