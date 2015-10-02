using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScenarioTut0 : Scenario 
{
	public static Scenario scenarioRef;
	//public GameObject prefabMuro;
	private int tamanyoMapaX = 22;
	private int tamanyoMapaY = 14;

	public byte[,] arrayNivelP = new byte[14, 22] 
	{	
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 1 , 1 , 2 , 1 , 1 , 2 , 0 , 0 , 2 , 1 , 1 , 2 , 0 , 0 , 0 , 0 , 2 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 2 , 1 , 1 , 2 , 1 , 1 , 1 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 2 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 4 , 1 , 1 , 1 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }
	};

	public override void Awake()
	{
		base.arrayNivel = arrayNivelP;
		Scenario.tamanyoMapaX = tamanyoMapaX;
		Scenario.tamanyoMapaY = tamanyoMapaY;

		base.Awake();
	}
}
