using UnityEngine;
using System.Collections;

public class GreenRobot : Robot
{
	Color colorRobot = new Color(0.29f, 0.97f, 0.41f);
	const float skillDuration = 5;
	const float wardRadius = 3;

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot verde");
	}

	public override void ActivatePower()
	{
		GameObject wardInstanciado = (GameObject)GameObject.Instantiate(playerGraphics.robotGraphics.prefabWard);
		wardInstanciado.GetComponent<Ward>().TurnOn(wardRadius, playerGraphics.robotGraphics.renderersDeColores[0].material.color,
		                                            skillDuration, new Vector3((int)transform.position.x, (int)transform.position.y, 0));
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
	
	public override string getColorString()
	{
		return "green";
	}
}
