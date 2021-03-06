﻿using UnityEngine;
using System.Collections;

public class GreenRobot : Robot
{
	public static Color colorRobot = new Color(0.29f, 0.97f, 0.41f);
	const float skillDuration = 15;
	const float wardRadius = 3.5f;

	public override void Initialize()
	{
		base.Initialize();

		//Debug.Log("Inicializando robot verde");
	}

	public override void ActivatePower()
	{
		GameObject wardInstanciado = (GameObject)GameObject.Instantiate(playerGraphics.robotGraphics.prefabWard);
		wardInstanciado.GetComponent<Ward>().TurnOn(wardRadius, playerGraphics.robotGraphics.renderersDeColores[0].material.color,
		                                            skillDuration, new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0));
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
