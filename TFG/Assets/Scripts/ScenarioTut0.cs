using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScenarioTut0 : Scenario 
{
	public static Scenario scenarioRef;
	//public GameObject prefabMuro;
	public const int tamanyoMapaX = 22;
	public const int tamanyoMapaY = 14;
	public GameManager gameManager;

	public byte[,] arrayNivel = new byte[tamanyoMapaY, tamanyoMapaX] 
	{	
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 4 , 1 , 1 , 1 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 1 , 2 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 2 , 1 , 1 , 2 , 1 , 1 , 1 , 0 , 0 , 2 , 1 , 1 , 1 , 1 , 2 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 }, 
		{ 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 0 }, 
		{ 0 , 0 , 1 , 1 , 1 , 2 , 1 , 1 , 2 , 0 , 0 , 2 , 1 , 1 , 2 , 0 , 0 , 0 , 0 , 2 , 0 , 0 }, 
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
		{ 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 } 
	};

	public override void Awake()
	{

	}
}
